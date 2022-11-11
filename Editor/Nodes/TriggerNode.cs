using Dialogues.Triggers;

namespace Dialogues.Editor.Nodes
{
    public sealed class TriggerNode : RenamableNode
    {
        private Trigger _trigger;
        private PropertyDatabaseHandler<Trigger> _database;

        public TriggerNode(Trigger trigger, PropertyDatabaseHandler<Trigger> database)
        {
            _trigger = trigger;
            _database = database;

            OnRename += newName => _database.Rename(_trigger, newName);
        }
    }
}