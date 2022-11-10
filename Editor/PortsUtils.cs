using UnityEditor.Experimental.GraphView;

namespace Dialogues.Editor
{
    public static class PortsUtils
    {
        public static Port CreateEntryPort(string portName)
        {
            var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            port.portName = portName;
            return port;
        }
        
        public static Port CreateInputPort(string portName)
        {
            var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            port.portName = portName;
            return port;
        }
        
        public static Port CreateCheckPort(string portName)
        {
            var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
            port.portName = portName;
            return port;
        }
        
        public static Port CreateOutputPort(string portName)
        {
            var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            port.portName = portName;
            return port;
        }
        
        public static Port CreateTriggerPort(string portName)
        {
            var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            port.portName = portName;
            return port;
        }

        public static bool AreCompatible(Port startPort, Port port)
        {
            // TODO
            return true;
        }
    }
}