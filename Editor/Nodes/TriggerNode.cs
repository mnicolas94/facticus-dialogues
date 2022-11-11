using Dialogues.Editor.Ports;
using Dialogues.Editor.Utils;
using Dialogues.Triggers;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogues.Editor.Nodes
{
    public sealed class TriggerNode : PropertyNode<Trigger>
    {
        public TriggerNode(Trigger property, PropertyDatabaseHandler<Trigger> database) : base(property, database)
        {
            this.AddStyleSheet("Styles/Nodes/TriggerNode");
        }
        
        protected override Port GetPort()
        {
            var port = PortsUtils.CreateTriggerPort("", Direction.Input, Port.Capacity.Multi);
            return port;
        }
    }
}