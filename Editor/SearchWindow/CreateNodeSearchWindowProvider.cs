using System;
using System.Collections.Generic;
using System.Linq;
using Dialogues.Checks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogues.Editor.SearchWindow
{
    public class CreateNodeSearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private EditorWindow _editorWindow;
        private DialogueGraphView _graphView;
        private DialoguesDatabase _database;

        private Texture2D _indentationIcon;

        public DialoguesDatabase Database
        {
            get => _database;
            set => _database = value;
        }

        public void Initialize(EditorWindow editorWindow, DialogueGraphView graphView, DialoguesDatabase database)
        {
            _editorWindow = editorWindow;
            _graphView = graphView;
            _database = database;
            
            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(0, 0, Color.clear);
            _indentationIcon.Apply();
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>();
            var title = new SearchTreeGroupEntry(new GUIContent("Nodes"), 0);
            tree.Add(title);
            
            AddDialogueNodeEntry(tree);
            AddChecks(tree);
            AddTriggers(tree);
            AddBooleanOperators(tree);
            
            return tree;
        }

        private void AddDialogueNodeEntry(List<SearchTreeEntry> tree)
        {
            var defaultEntry = GetIndentedEntry("New dialogue");
            defaultEntry.level = 1;
            defaultEntry.userData = new EntrySelectedAction((graph, position) =>
            {
                graph.AddDefaultDialogueNode("Dialogue", position);
            });

            tree.Add(defaultEntry);
        }

        private void AddChecks(List<SearchTreeEntry> tree)
        {
            var group = new SearchTreeGroupEntry(new GUIContent("Checks"), 1);
            tree.Add(group);
            
            // create check entry
            var createCheckEntry = GetIndentedEntry("<Create new check>");
            createCheckEntry.level = 2;
            createCheckEntry.userData = new EntrySelectedAction((graph, position) =>
            {
                graph.AddNewCheckNode("new check", position);
            });
            tree.Add(createCheckEntry);
            
            // checks-in-database entries
            
            var checksDatabase = _database.ChecksDatabase;
            foreach (var check in checksDatabase.List)
            {
                var checkName = checksDatabase.GetDisplayName(check);
                var entry = GetIndentedEntry(checkName);
                entry.level = 2;
                entry.userData = new EntrySelectedAction((graph, position) =>
                {
                    graph.AddCheckNode(check, position);
                });
                tree.Add(entry);
            }
        }
        
        private void AddTriggers(List<SearchTreeEntry> tree)
        {
            var group = new SearchTreeGroupEntry(new GUIContent("Triggers"), 1);
            tree.Add(group);
            
            // create trigger entry
            var createTriggerEntry = GetIndentedEntry("<Create new trigger>");
            createTriggerEntry.level = 2;
            createTriggerEntry.userData = new EntrySelectedAction((graph, position) =>
            {
                graph.AddNewTriggerNode("new trigger", position);
            });
            tree.Add(createTriggerEntry);

            // triggers-in-database entries
            var triggersDatabase = _database.TriggersDatabase;
            foreach (var trigger in triggersDatabase.List)
            {
                var triggerName = triggersDatabase.GetDisplayName(trigger);
                var entry = GetIndentedEntry(triggerName);
                entry.level = 2;
                entry.userData = new EntrySelectedAction((graph, position) =>
                {
                    graph.AddTriggerNode(trigger, position);
                });
                tree.Add(entry);
            }
        }
        
        private void AddBooleanOperators(List<SearchTreeEntry> tree)
        {
            var group = new SearchTreeGroupEntry(new GUIContent("Booleans"), 1);
            tree.Add(group);
            
            var binaryOperations = Enum.GetValues(typeof(BinaryOperation)).Cast<BinaryOperation>();
            foreach (var operation in binaryOperations)
            {
                var entry = GetIndentedEntry(operation.ToString());
                entry.level = 2;
                entry.userData = new EntrySelectedAction((graph, position) =>
                {
                    graph.AddBinaryNode(operation.ToString(), position, operation);
                });
                tree.Add(entry);
            }
        }

        private SearchTreeEntry GetIndentedEntry(string entryText)
        {
            var entry = new SearchTreeEntry(new GUIContent(entryText, _indentationIcon));
            return entry;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var screenPosition = context.screenMousePosition;
            var editorPosition = screenPosition - _editorWindow.position.position;
            var mouseGraphPosition = _graphView.contentViewContainer.WorldToLocal(editorPosition);

            if (searchTreeEntry.userData is EntrySelectedAction action)
            {
                action.OnSelected(_graphView, mouseGraphPosition);
                return true;
            }
            
            return false;
        }
    }
}