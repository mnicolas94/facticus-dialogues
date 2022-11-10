using UnityEditor.Experimental.GraphView;

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
    }
}