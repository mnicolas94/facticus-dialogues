using Dialogues.Editor.DialogueGraph.Utils;
using UnityEditor.Experimental.GraphView;

namespace Dialogues.Editor.DialogueGraph.Ports
{
    public static class PortsUtils
    {
        public static Port CreateEntryPort(string portName)
        {
            var port = DialoguePort.Create(
                Orientation.Horizontal,
                Direction.Output,
                Port.Capacity.Multi,
                PortType.Dialogue,
                new EdgeConnectorListener());
            port.portName = portName;
            port.AddStyleSheet("Styles/Ports/EntryPort");
            return port;
        }
        
        public static Port CreateDialoguePort(string portName, Direction direction, Port.Capacity capacity)
        {
            var port = DialoguePort.Create(
                Orientation.Horizontal,
                direction,
                capacity,
                PortType.Dialogue,
                new EdgeConnectorListener());
            port.portName = portName;
            port.AddStyleSheet("Styles/Ports/DialoguePort");
            return port;
        }
        
        public static Port CreateCheckPort(string portName, Direction direction, Port.Capacity capacity)
        {
            var port = DialoguePort.Create(
                Orientation.Horizontal,
                direction,
                capacity,
                PortType.Check,
                new EdgeConnectorListener());
            port.portName = portName;
            port.AddStyleSheet("Styles/Ports/CheckPort");
            return port;
        }
        
        public static Port CreateTriggerPort(string portName, Direction direction, Port.Capacity capacity)
        {
            var port = DialoguePort.Create(
                Orientation.Horizontal,
                direction,
                capacity,
                PortType.Trigger,
                new EdgeConnectorListener());
            port.portName = portName;
            port.AddStyleSheet("Styles/Ports/TriggerPort");
            return port;
        }

        public static bool AreCompatible(Port startPort, Port endPort)
        {
            var startType = PortType.Dialogue;
            var endType = PortType.Dialogue;
            
            if (startPort is DialoguePort startDialoguePort)
            {
                startType = startDialoguePort.PortType;
            }
            
            if (endPort is DialoguePort endDialoguePort)
            {
                endType = endDialoguePort.PortType;
            }
            
            return startType == endType;
        }
    }
}