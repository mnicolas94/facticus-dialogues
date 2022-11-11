using Dialogues.Checks;
using Dialogues.Editor.Ports;
using Dialogues.Editor.Utils;
using UnityEditor.Experimental.GraphView;

namespace Dialogues.Editor.Nodes
{
    public sealed class CheckNode : PropertyNode<Check>
    {
        public CheckNode(Check property, PropertyDatabaseHandler<Check> database) : base(property, database)
        {
            this.AddStyleSheet("Styles/Nodes/CheckNode");
        }

        protected override Port GetPort()
        {
            var port = PortsUtils.CreateCheckPort("", Direction.Output, Port.Capacity.Multi);
            return port;
        }
    }
}