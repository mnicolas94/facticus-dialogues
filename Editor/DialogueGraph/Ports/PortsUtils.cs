using Dialogues.Editor.DialogueGraph.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogues.Editor.DialogueGraph.Ports
{
    public static class PortsUtils
    {
        private static Port CreatePort(
            string portName,
            Direction direction,
            Port.Capacity capacity,
            PortType portType,
            string stylePath
            )
        {
            var port = DialoguePort.Create(
                Orientation.Horizontal,
                direction,
                capacity,
                portType,
                new EdgeConnectorListener());
            port.portName = portName;
            port.portColor = GetPortTypeColor(portType);
            port.AddStyleSheet(stylePath);
            return port;
        }

        public static Port CreateEntryPort(string portName)
        {
            return CreatePort(
                portName,
                Direction.Output,
                Port.Capacity.Multi,
                PortType.Dialogue,
                "Styles/Ports/EntryPort");
        }
        
        public static Port CreateDialoguePort(string portName, Direction direction, Port.Capacity capacity)
        {
            return CreatePort(
                portName,
                direction,
                capacity,
                PortType.Dialogue,
                "Styles/Ports/DialoguePort");
        }
        
        public static Port CreateCheckPort(string portName, Direction direction, Port.Capacity capacity)
        {
            return CreatePort(
                portName,
                direction,
                capacity,
                PortType.Check,
                "Styles/Ports/CheckPort");
        }
        
        public static Port CreateTriggerPort(string portName, Direction direction, Port.Capacity capacity)
        {
            return CreatePort(
                portName,
                direction,
                capacity,
                PortType.Trigger,
                "Styles/Ports/TriggerPort");
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

        private static Color GetPortTypeColor(PortType type)
        {
            var stringColor = "";
            switch (type)
            {
                case PortType.Check:
                    stringColor = "#06D6A0";
                    break;
                case PortType.Dialogue:
                    stringColor = "#118AB2";
                    break;
                case PortType.Trigger:
                    stringColor = "#EF476F";
                    break;
                default:
                    stringColor = "#FFFFFF";
                    break;
            }

            ColorUtility.TryParseHtmlString(stringColor, out var color);
            return color;
        }
    }
}