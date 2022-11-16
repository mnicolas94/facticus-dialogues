using System.Collections.Generic;
using System.Linq;
using Dialogues.Core;
using Dialogues.Editor.DialogueGraph.Nodes;
using TNRD;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace Dialogues.Editor.DialogueGraph
{
    public static class DialogueGraphSerialization
    {
        public static void SaveGraph(this DialogueGraphView graphView, Dialogue dialogue)
        {
            // --- serialize dialogue data
            // clear dialogue data
            dialogue.EditorClearLines();
            dialogue.EditorClearConnections();

            // add start dialogue lines
            var startDialogueNodes = graphView.EntryNode.GetConnectedDialogueNodes();
            foreach (var startDialogueNode in startDialogueNodes)
            {
                var dialogueLine = startDialogueNode.DialogueLine;
                dialogue.EditorAddStartLine(dialogueLine);
            }
            
            // add dialogue lines
            var dialogueNodes = graphView.nodes.Where(node => node is DialogueNode).Cast<DialogueNode>();
            foreach (var dialogueNode in dialogueNodes)
            {
                // add line
                var dialogueLine = dialogueNode.DialogueLine;
                dialogueLine.EditorSetCheck(dialogueNode.Check);
                dialogueLine.EditorSetTrigger(dialogueNode.Trigger);
                EditorUtility.SetDirty(dialogueLine);
                dialogue.EditorAddLine(dialogueLine);

                // add connections
                var connectedDialogueNodes = dialogueNode.GetConnectedDialogueNodes();
                var connectedLines = connectedDialogueNodes.ConvertAll(connectedDialogueLine => connectedDialogueLine.DialogueLine);
                dialogue.EditorAddLineConnections(dialogueLine, connectedLines);
            }
            
            // --- serialize graph data
            // serialize nodes
            var serializableNodes = graphView.nodes
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
            var nodesIndexPorts = new Dictionary<ISerializableNode, (int, List<Port>)>();
            for (int i = 0; i < serializableNodes.Count; i++)
            {
                var serializableNode = (ISerializableNode) serializableNodes[i];
                var ports = serializableNode.GetPorts();
                nodesIndexPorts.Add(serializableNode, (i, ports));
            }

            var serializableEdges = new List<SerializableEdge>();
            foreach (var edge in graphView.edges)
            {
                var (inputNodeIndex, inputPortIndex) = GetNodePortIndices(edge.input.node, edge.input, nodesIndexPorts);
                var (outputNodeIndex, outputPortIndex) = GetNodePortIndices(edge.output.node, edge.output, nodesIndexPorts);

                var serializableEdge = new SerializableEdge
                {
                    inputNodeIndex = inputNodeIndex,
                    inputPortIndex = inputPortIndex,
                    outputNodeIndex = outputNodeIndex,
                    outputPortIndex = outputPortIndex,
                };
                
                serializableEdges.Add(serializableEdge);
            }
            
            // create editor data and set it to the dialogue
            var editorData = new DialogueEditorData(serializableNodesPositions, serializableEdges);
            dialogue.EditorData = editorData;
            
            EditorUtility.SetDirty(dialogue);
            AssetDatabase.SaveAssets();
        }

        public static void LoadGraph(this DialogueGraphView graphView, Dialogue dialogue)
        {
            graphView.ClearGraph();
            
            if (dialogue == null)
            {
                return;
            }
            
            var editorData = dialogue.EditorData;
            var tempNodes = new List<Node>();
            foreach (var (serializedNode, position) in editorData.Nodes)
            {
                var node = serializedNode.Value.Deserialize();
                graphView.AddNode(node, position);
                tempNodes.Add(node);
            }
            
            // deserialize edges
            foreach (var serializedEdge in editorData.Edges)
            {
                var inputPort = GetPortFromSerializedEdgeIndices(
                    tempNodes, serializedEdge.inputNodeIndex, serializedEdge.inputPortIndex);
                
                var outputPort = GetPortFromSerializedEdgeIndices(
                    tempNodes, serializedEdge.outputNodeIndex, serializedEdge.outputPortIndex);
                inputPort = inputPort ?? graphView.EntryNode.Port;
                outputPort = outputPort ?? graphView.EntryNode.Port;
                
                var edge = new Edge
                {
                    input = inputPort,
                    output = outputPort
                };
                graphView.AddElement(edge);
                inputPort.Connect(edge);
                outputPort.Connect(edge);
            }
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

        private static Port GetPortFromSerializedEdgeIndices(List<Node> tempNodes, int nodeIndex, int portIndex)
        {
            if (nodeIndex >= 0)
            {
                var inputNode = tempNodes[nodeIndex];
                var inputPorts = ((ISerializableNode) inputNode).GetPorts();
                var inputPort = inputPorts[portIndex];
                return inputPort;
            }

            return null;
        }
    }
}