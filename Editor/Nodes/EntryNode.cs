using Dialogues.Editor.Ports;
using Dialogues.Editor.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Dialogues.Editor.Nodes
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