using System;
using Dialogues.Core;
using Dialogues.Core.Triggers;
using Dialogues.Editor.DialogueGraph.Ports;
using Dialogues.Editor.DialogueGraph.Utils;
using UnityEditor.Experimental.GraphView;

namespace Dialogues.Editor.DialogueGraph.Nodes
{
    [Serializable]
    public sealed class TriggerNode : PropertyNode<Trigger>
    {
        public TriggerNode(Trigger property, DialoguesDatabase database) : base(property, database)
        {
            this.AddStyleSheet("Styles/Nodes/TriggerNode");
            var displayName = GetDatabaseHandler(database).GetDisplayName(property);
            title = displayName;
        }
        
        protected override Port GetPort()
        {
            var port = PortsUtils.CreateTriggerPort("", Direction.Input, Port.Capacity.Multi);
            return port;
        }

        protected override PropertyDatabaseHandler<Trigger> GetDatabaseHandler(DialoguesDatabase database)
        {
            return database.TriggersDatabase;
        }

        public override Node Deserialize()
        {
            return new TriggerNode(_property, _database);
        }
    }
}