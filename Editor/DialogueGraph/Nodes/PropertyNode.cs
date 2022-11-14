using System.Collections.Generic;
using Dialogues.Editor.DialogueGraph.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dialogues.Editor.DialogueGraph.Nodes
{
    public abstract class PropertyNode<T> : RenamableNode, ISerializableNode where T : ScriptableObject
    {
        [SerializeField] protected T _property;
        [SerializeField] protected DialoguesDatabase _database;
        private Port _port;

        public T Property => _property;

        public PropertyNode(T property, DialoguesDatabase database)
        {
            _property = property;
            _database = database;

            OnRename += newName => GetDatabaseHandler(_database).Rename(_property, newName);
            
            SetupPort();
            RefreshExpandedState();
        }

        private void SetupPort()
        {
            this.RemoveCollapsibleButton();
            
            // add port
            _port = GetPort();
            titleButtonContainer.Add(_port);
        }

        protected abstract Port GetPort();

        protected abstract PropertyDatabaseHandler<T> GetDatabaseHandler(DialoguesDatabase database);

        public abstract Node Deserialize();
        public List<Port> GetPorts()
        {
            return new List<Port>{ _port };
        }
    }
}