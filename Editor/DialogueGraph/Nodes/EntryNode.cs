using Dialogues.Editor.DialogueGraph.Ports;
using Dialogues.Editor.DialogueGraph.Utils;
using UnityEditor.Experimental.GraphView;

namespace Dialogues.Editor.DialogueGraph.Nodes
{
    public class EntryNode : Node
    {
        public EntryNode()
        {
            this.AddStyleSheet("Styles/Nodes/EntryNode");
            
            capabilities &= ~Capabilities.Deletable;
            capabilities &= ~Capabilities.Movable;
            capabilities &= ~Capabilities.Collapsible;
            capabilities &= ~Capabilities.Copiable;
            
            this.RemoveCollapsibleButton();
            var port = PortsUtils.CreateEntryPort("");
            titleButtonContainer.Add(port);
            RefreshExpandedState();
        }
    }
}