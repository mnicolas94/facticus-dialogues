using System;
using Dialogues.Checks;
using Dialogues.Editor.DialogueGraph.Ports;
using Dialogues.Editor.DialogueGraph.Utils;
using UnityEditor.Experimental.GraphView;

namespace Dialogues.Editor.DialogueGraph.Nodes
{
    [Serializable]
    public sealed class CheckNode : PropertyNode<Check>
    {
        public CheckNode(Check property, DialoguesDatabase database) : base(property, database)
        {
            this.AddStyleSheet("Styles/Nodes/CheckNode");
            var displayName = GetDatabaseHandler(database).GetDisplayName(property);
            title = displayName;
        }

        protected override Port GetPort()
        {
            var port = PortsUtils.CreateCheckPort("", Direction.Output, Port.Capacity.Multi);
            return port;
        }

        protected override PropertyDatabaseHandler<Check> GetDatabaseHandler(DialoguesDatabase database)
        {
            return database.ChecksDatabase;
        }

        public override Node Deserialize()
        {
            return new CheckNode(_property, _database);
        }
    }
}