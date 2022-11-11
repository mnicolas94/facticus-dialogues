using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Dialogues.Editor
{
    public static class NodeExtensions
    {
        public static void AddPort(this Node node, Port port, bool refresh = false)
        {
            var container = port.direction == Direction.Input ? node.inputContainer : node.outputContainer;
            container.Add(port);
            if (refresh)
            {
                node.RefreshExpandedState();
                node.RefreshPorts();
            }
        }

        public static void RemoveCollapsibleButton(this Node node)
        {
            // remove collapsible button
            var button = node.Q("collapse-button");
            button.RemoveFromHierarchy();
            node.capabilities &= ~Capabilities.Collapsible;
        }
    }
}