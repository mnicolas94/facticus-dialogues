using Dialogues.Checks;

namespace Dialogues.Editor.Nodes
{
    public sealed class CheckNode : RenamableNode
    {
        private Check _check;
        private PropertyDatabaseHandler<Check> _database;

        public CheckNode(Check check, PropertyDatabaseHandler<Check> database)
        {
            _check = check;
            _database = database;

            OnRename += newName => _database.Rename(_check, newName);
        }
    }
}