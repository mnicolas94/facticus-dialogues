﻿using System.Collections.Generic;
using System.Linq;
using Dialogues.Checks;
using Dialogues.Editor.Nodes;
using Dialogues.Editor.Ports;
using Dialogues.Editor.SearchWindow;
using Dialogues.Editor.Utils;
using Dialogues.Triggers;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogues.Editor
{
    public class DialogueGraphView : GraphView
    {
        private DialoguesDatabase _database;
        
        private readonly Vector2 _defaultNodeSize = new Vector2(700, 300);
        private CreateNodeSearchWindowProvider _searchProvider;

        public DialoguesDatabase Database
        {
            get => _database;
            set
            {
                _database = value;
                _searchProvider.Database = value;
            }
        }

        public DialogueGraphView(EditorWindow editorWindow, DialoguesDatabase database)
        {
            _database = database;
            SetupUi();
            AddNodeSearchWindow(editorWindow);
            CreateEntryNode();
        }

        private void AddNodeSearchWindow(EditorWindow editorWindow)
        {
            _searchProvider = ScriptableObject.CreateInstance<CreateNodeSearchWindowProvider>();
            _searchProvider.Initialize(editorWindow, this, _database);
            nodeCreationRequest += context =>
            {
                var searchWindowContext = new SearchWindowContext(context.screenMousePosition);
                UnityEditor.Experimental.GraphView.SearchWindow.Open(searchWindowContext, _searchProvider);
            };
        }

        private void SetupUi()
        {
            this.AddStyleSheet("Styles/DialogueGraphView");
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            grid.StretchToParentSize();
            Insert(0, grid);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = ports.Where(port =>
            {
                var isSameNode = port.node == startPort.node;
                var isSameDirection = port.direction == startPort.direction;
                var areCompatible = PortsUtils.AreCompatible(startPort, port);
                return !isSameNode && !isSameDirection && areCompatible;
            });
            return compatiblePorts.ToList();
        }

        private void CreateEntryNode()
        {
            var node = new EntryNode()
            {
                title = "Start"
            };

            AddNode(node, Vector2.zero);
        }

        public void AddNode(Node node, Vector2 position)
        {
            node.SetPosition(new Rect(position, _defaultNodeSize));
            AddElement(node);
        }
        
        public void AddDefaultDialogueNode(string title, Vector2 position)
        {
            var node = new DialogueNode()
            {
                title = title
            };
            AddNode(node, position);
        }
        
        public void AddNewCheckNode(string checkName, Vector2 position)
        {
            var checksDatabase = _database.ChecksDatabase;
            var check = checksDatabase.CreateNew(checkName);
            var displayName = checksDatabase.GetDisplayName(check);
            var node = new CheckNode(check, checksDatabase)
            {
                title = displayName
            };
            AddNode(node, position);
            node.StartRename();
        }
        
        public void AddCheckNode(Check check, Vector2 position)
        {
            var checksDatabase = _database.ChecksDatabase;
            var displayName = checksDatabase.GetDisplayName(check);
            var node = new CheckNode(check, checksDatabase)
            {
                title = displayName
            };
            AddNode(node, position);
        }
        
        public void AddNewTriggerNode(string triggerName, Vector2 position)
        {
            var triggersDatabase = _database.TriggersDatabase;
            var trigger = triggersDatabase.CreateNew(triggerName);
            var displayName = triggersDatabase.GetDisplayName(trigger);
            var node = new TriggerNode(trigger, triggersDatabase)
            {
                title = displayName
            };
            AddNode(node, position);
            node.StartRename();
        }
        
        public void AddTriggerNode(Trigger trigger, Vector2 position)
        {
            var triggersDatabase = _database.TriggersDatabase;
            var displayName = triggersDatabase.GetDisplayName(trigger);
            var node = new TriggerNode(trigger, triggersDatabase)
            {
                title = displayName
            };
            AddNode(node, position);
        }
        
        public void AddBinaryNode(string title, Vector2 position, BinaryOperation operation)
        {
            var node = new BooleanNode(operation)
            {
                title = title
            };
            AddNode(node, position);
        }
    }
}