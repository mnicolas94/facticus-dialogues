using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogues.Editor.Nodes
{
    public abstract class PropertyNode<T> : RenamableNode where T : ScriptableObject
    {
        private T _property;
        private PropertyDatabaseHandler<T> _database;

        public PropertyNode(T property, PropertyDatabaseHandler<T> database)
        {
            _property = property;
            _database = database;

            OnRename += newName => _database.Rename(_property, newName);
            
            SetupPort();
            RefreshExpandedState();
        }

        private void SetupPort()
        {
            this.RemoveCollapsibleButton();
            
            // add port
            var port = GetPort();
            titleButtonContainer.Add(port);
        }

        protected abstract Port GetPort();
    }
}