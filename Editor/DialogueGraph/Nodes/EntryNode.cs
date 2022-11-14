using System.Collections.Generic;
using Dialogues.Editor.DialogueGraph.Ports;
using Dialogues.Editor.DialogueGraph.Utils;
using UnityEditor.Experimental.GraphView;

namespace Dialogues.Editor.DialogueGraph.Nodes
{
    public class EntryNode : Node
    {
        private Port _port;

        public Port Port => _port;

        public EntryNode()
        {
            this.AddStyleSheet("Styles/Nodes/EntryNode");
            
            capabilities &= ~Capabilities.Deletable;
            capabilities &= ~Capabilities.Movable;
            capabilities &= ~Capabilities.Collapsible;
            capabilities &= ~Capabilities.Copiable;
            
            this.RemoveCollapsibleButton();
            _port = PortsUtils.CreateEntryPort("");
            titleButtonContainer.Add(_port);
            RefreshExpandedState();
        }

        public List<DialogueNode> GetConnectedDialogueNodes()
        {
            var connectedDialogues = new List<DialogueNode>();
            if (_port.connected)
            {
                foreach (var connection in _port.connections)
                {
                    var dialogueNode = (DialogueNode) connection.input.node;
                    connectedDialogues.Add(dialogueNode);
                }
            }

            return connectedDialogues;
        }
    }
}