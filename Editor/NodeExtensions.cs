using UnityEditor.Experimental.GraphView;

namespace Dialogues.Editor
{
    public static class NodeExtensions
    {
        public static void CreatePort(this Node node, string portName, Direction direction, Port.Capacity capacity = Port.Capacity.Single)
        {
            var port = node.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(float));
            port.portName = portName;
            var container = direction == Direction.Input ? node.inputContainer : node.outputContainer;
            container.Add(port);
            
            node.RefreshExpandedState();
            node.RefreshPorts();
        }
    }
}