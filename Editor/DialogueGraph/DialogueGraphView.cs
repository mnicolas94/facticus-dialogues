using System.Collections.Generic;
using System.Linq;
using Dialogues.Checks;
using Dialogues.Editor.DialogueGraph.Nodes;
using Dialogues.Editor.DialogueGraph.Ports;
using Dialogues.Editor.DialogueGraph.SearchWindow;
using Dialogues.Editor.DialogueGraph.Utils;
using Dialogues.Triggers;
using TNRD;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogues.Editor.DialogueGraph
{
    public class DialogueGraphView : GraphView
    {
        private DialoguesDatabase _database;
        
        private static CreateNodeSearchWindowProvider _searchProvider;
        private EntryNode _entryNode;

        public DialoguesDatabase Database
        {
            get => _database;
            set
            {
                _database = value;
                _searchProvider.Database = value;
            }
        }

        public DialogueGraphView(EditorWindow editorWindow, DialoguesDatabase database)
        {
            _database = database;
            AddNodeSearchWindow(editorWindow);
            SetupUi();
            CreateEntryNode();
        }

        
        public void SaveGraph(Dialogue dialogue)
        {
            // clear dialogue data
            dialogue.EditorClearLines();
            dialogue.EditorClearConnections();

            var dialogueNodes = nodes.Where(node => node is DialogueNode).Cast<DialogueNode>();
            foreach (var dialogueNode in dialogueNodes)
            {
            }
            // foreach dialogue node
            //     add line to dialogue
            //     connect check to line
            //     connect triggers to line
            
            // serialize nodes
            var serializableNodes = nodes
                .Where(node => node is ISerializableNode)
                .ToList();
            var serializableNodesPositions = serializableNodes
                .Select(node => ((ISerializableNode) node, node.GetPosition().position))
                .Select(tuple =>
                {
                    var (node, position) = tuple;
                    var serializableInterface = new SerializableInterface<ISerializableNode> {Value = node};
                    var serializedNodePosition = new SerializedNodePosition
                    {
                        node = serializableInterface,
                        position = position
                    };
                    return serializedNodePosition;
                })
                .ToList();

            // serialize edges
            var nodesPorts = new Dictionary<ISerializableNode, (int, List<Port>)>();
            for (int i = 0; i < serializableNodes.Count; i++)
            {
                var serializableNode = (ISerializableNode) serializableNodes[i];
                var ports = serializableNode.GetPorts();
                nodesPorts.Add(serializableNode, (i, ports));
            }

            var serializableEdges = new List<SerializableEdge>();
            foreach (var edge in edges)
            {
                var (inputNodeIndex, inputPortIndex) = GetNodePortIndices(edge.input.node, edge.input, nodesPorts);
                var (outputNodeIndex, outputPortIndex) = GetNodePortIndices(edge.output.node, edge.output, nodesPorts);

                var serializableEdge = new SerializableEdge
                {
                    inputNodeIndex = inputNodeIndex,
                    inputPortIndex = inputPortIndex,
                    outputNodeIndex = outputNodeIndex,
                    outputPortIndex = outputPortIndex,
                };
                
                serializableEdges.Add(serializableEdge);
            }
            
            var editorData = new DialogueEditorData(serializableNodesPositions, serializableEdges);
            dialogue.EditorData = editorData;
            
            EditorUtility.SetDirty(dialogue);
            AssetDatabase.SaveAssets();
        }

        private static (int, int) GetNodePortIndices(
            Node node, Port port, Dictionary<ISerializableNode, (int, List<Port>)> nodesPorts)
        {
            if (node is ISerializableNode serializableNode)
            {
                var (inputNodeIndex, inputPorts) = nodesPorts[serializableNode];
                var inputPortIndex = inputPorts.IndexOf(port);
                return (inputNodeIndex, inputPortIndex);
            }

            return (-1, -1);
        }

        public void LoadGraph(Dialogue dialogue)
        {
            ClearGraph();
            
            if (dialogue == null)
            {
                return;
            }
            
            var editorData = dialogue.EditorData;
            var tempNodes = new List<Node>();
            foreach (var (serializedNode, position) in editorData.Nodes)
            {
                var node = serializedNode.Value.Deserialize();
                AddNode(node, position);
                tempNodes.Add(node);
            }
            
            // deserialize edges
            foreach (var serializedEdge in editorData.Edges)
            {
                var inputPort = GetPortFromSerializedEdgeIndices(
                    tempNodes, serializedEdge.inputNodeIndex, serializedEdge.inputPortIndex);
                
                var outputPort = GetPortFromSerializedEdgeIndices(
                    tempNodes, serializedEdge.outputNodeIndex, serializedEdge.outputPortIndex);

                var edge = new Edge
                {
                    input = inputPort,
                    output = outputPort
                };
                AddElement(edge);
                inputPort.Connect(edge);
                outputPort.Connect(edge);
            }
        }

        private Port GetPortFromSerializedEdgeIndices(List<Node> tempNodes, int nodeIndex, int portIndex)
        {
            if (nodeIndex >= 0)
            {
                var inputNode = tempNodes[nodeIndex];
                var inputPorts = ((ISerializableNode) inputNode).GetPorts();
                var inputPort = inputPorts[portIndex];
                return inputPort;
            }

            return _entryNode.Port;
        }

        private void ClearGraph()
        {
            IEnumerable<GraphElement> elements = nodes.Cast<GraphElement>().Concat(edges);
            
            foreach (var element in elements)
            {
                if (element != _entryNode)
                {
                    RemoveElement(element);
                }
            }

            _entryNode.RefreshPorts();
        }
        
        private void AddNodeSearchWindow(EditorWindow editorWindow)
        {
            _searchProvider = ScriptableObject.CreateInstance<CreateNodeSearchWindowProvider>();
            _searchProvider.Initialize(editorWindow, this, _database);
            nodeCreationRequest += context =>
            {
                var searchWindowContext = new SearchWindowContext(context.screenMousePosition);
                UnityEditor.Experimental.GraphView.SearchWindow.Open(searchWindowContext, _searchProvider);
            };
        }
        
        private void SetupUi()
        {
            this.AddStyleSheet("Styles/DialogueGraphView");
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            var grid = new GridBackground();
            grid.StretchToParentSize();
            Insert(0, grid);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = ports.Where(port =>
            {
                var isSameNode = port.node == startPort.node;
                var isSameDirection = port.direction == startPort.direction;
                var areCompatible = PortsUtils.AreCompatible(startPort, port);
                return !isSameNode && !isSameDirection && areCompatible;
            });
            return compatiblePorts.ToList();
        }

        private void CreateEntryNode()
        {
            _entryNode = new EntryNode()
            {
                title = "Start"
            };

            AddNode(_entryNode, Vector2.zero);
            RegisterCallback<GeometryChangedEvent>(RepositionEntryNodeOnGeometryChange);
        }

        private void RepositionEntryNodeOnGeometryChange(GeometryChangedEvent evt)
        {
            FrameNext();
            UnregisterCallback<GeometryChangedEvent>(RepositionEntryNodeOnGeometryChange);
        }
        
        public void AddNode(Node node, Vector2 position)
        {
            node.SetPosition(new Rect(position, Vector2.zero));
            AddElement(node);
        }
        
        public void AddDefaultDialogueNode(string title, Vector2 position)
        {
            var defaultDialogueLine = ScriptableObject.CreateInstance<DialogueLine>();
            var node = new DialogueNode(defaultDialogueLine);
            AddNode(node, position);
        }
        
        public void AddNewCheckNode(string checkName, Vector2 position)
        {
            var checksDatabase = _database.ChecksDatabase;
            var check = checksDatabase.CreateNew(checkName);
            var node = new CheckNode(check, _database);
            AddNode(node, position);
            node.StartRename();
        }
        
        public void AddCheckNode(Check check, Vector2 position)
        {
            var checksDatabase = _database.ChecksDatabase;
            var node = new CheckNode(check, _database);
            AddNode(node, position);
        }
        
        public void AddNewTriggerNode(string triggerName, Vector2 position)
        {
            var triggersDatabase = _database.TriggersDatabase;
            var trigger = triggersDatabase.CreateNew(triggerName);
            var node = new TriggerNode(trigger, _database);
            AddNode(node, position);
            node.StartRename();
        }
        
        public void AddTriggerNode(Trigger trigger, Vector2 position)
        {
            var triggersDatabase = _database.TriggersDatabase;
            var displayName = triggersDatabase.GetDisplayName(trigger);
            var node = new TriggerNode(trigger, _database);
            AddNode(node, position);
        }
        
        public void AddBinaryNode(BinaryOperation operation, Vector2 position)
        {
            var node = new BooleanNode(operation);
            AddNode(node, position);
        }
    }
}