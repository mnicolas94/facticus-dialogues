using System;
using Dialogues.Editor.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Dialogues.Editor.Ports
{
    public class DialoguePort : Port
    {
        private PortType _portType;

        public PortType PortType => _portType;

        protected DialoguePort(
            Orientation portOrientation,
            Direction portDirection,
            Capacity portCapacity,
            Type type)
            : base(portOrientation, portDirection, portCapacity, type)
        {
            
        }
        
        public static DialoguePort Create(
            Orientation portOrientation,
            Direction portDirection,
            Capacity portCapacity,
            PortType type,
            EdgeConnectorListener edgeConnectorListener)
        {
            var port = new DialoguePort(portOrientation, portDirection, portCapacity, typeof(object));
            if (edgeConnectorListener != null) {
                port.m_EdgeConnector = new EdgeConnector<Edge>(edgeConnectorListener);
            }
            port.AddManipulator(port.m_EdgeConnector);
            port._portType = type;
            
            return port;
        }
    }
}