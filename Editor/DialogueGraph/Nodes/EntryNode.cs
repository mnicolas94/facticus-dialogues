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
    }
}