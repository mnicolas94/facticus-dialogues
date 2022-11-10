using System;
using UnityEditor.Experimental.GraphView;

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
        
//        public static DialoguePort Create(
//            string name,
//            Orientation portOrientation,
//            Direction portDirection,
//            Capacity portCapacity,
//            PortType type,
//            EdgeConnectorListener edgeConnectorListener)
//        {
//            var port = new DialoguePort(portOrientation, portDirection, portCapacity, typeof(object));
//            if (edgeConnectorListener != null) {
//                port.m_EdgeConnector = new EdgeConnector<Edge>(edgeConnectorListener);
//                port.AddManipulator(port.m_EdgeConnector);
//            }
//            
//            port.AddStyleSheet("Styles/Node/Port");
//            if (!required) {
//                port.AddToClassList("optional");
//            }
//            
//            port._portType = type;
//            port.portColor = PortHelper.PortColor(port);
//            port.viewDataKey = Guid.NewGuid().ToString();
//            port.portName = name;
//            
//            
//            return port;
//        }
    }
}