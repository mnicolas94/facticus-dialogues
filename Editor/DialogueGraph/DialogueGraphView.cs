using System;
using System.Collections.Generic;
using System.Linq;
using Dialogues.Core;
using Dialogues.Core.Checks;
using Dialogues.Core.Triggers;
using Dialogues.Editor.DialogueGraph.Nodes;
using Dialogues.Editor.DialogueGraph.Ports;
using Dialogues.Editor.DialogueGraph.SearchWindow;
using Dialogues.Editor.DialogueGraph.Utils;
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

        public EntryNode EntryNode => _entryNode;

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

        public void ClearGraph()
        {
            IEnumerable<GraphElement> elements = nodes.Cast<GraphElement>().Concat(edges);
            
            foreach (var element in elements)
            {
                if (element != _entryNode)
                {
                    RemoveElement(element);
                }
            }

            _entryNode.Port.DisconnectAll();
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
        
        public void AddDefaultDialogueNode(Vector2 position)
        {
            var defaultDialogueLine = ScriptableObject.CreateInstance<DialogueLine>();
            
            // get default line data type
            var lineDataTypes = TypeCache.GetTypesDerivedFrom<IDialogueLineData>().ToList();
            if (lineDataTypes.Count > 0)
            {
                var lineDataType = lineDataTypes[0];
                var lineData = (IDialogueLineData) Activator.CreateInstance(lineDataType);
                defaultDialogueLine.EditorSetLineData(lineData);
            }
            
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