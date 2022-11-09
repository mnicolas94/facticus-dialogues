using System.Collections.Generic;
using System.Linq;
using Dialogues.Editor.SearchWindow;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogues.Editor
{
    public class DialogueGraphView : GraphView
    {
        private readonly Vector2 _defaultNodeSize = new Vector2(700, 300);
        private CreateNodeSearchWindowProvider _searchProvider;

        public DialogueGraphView(EditorWindow editorWindow)
        {
            SetupUi();
            AddNodeSearchWindow(editorWindow);
            CreateEntryNode();
        }

        private void AddNodeSearchWindow(EditorWindow editorWindow)
        {
            _searchProvider = ScriptableObject.CreateInstance<CreateNodeSearchWindowProvider>();
            _searchProvider.Initialize(editorWindow, this);
            nodeCreationRequest += context =>
            {
                var searchWindowContext = new SearchWindowContext(context.screenMousePosition);
                UnityEditor.Experimental.GraphView.SearchWindow.Open(searchWindowContext, _searchProvider);
            };
        }

        private void SetupUi()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraphView"));
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
            var compatiblePorts = ports.Where(port => port.node != startPort.node);
            return compatiblePorts.ToList();
        }

        private void CreateEntryNode()
        {
            var node = new Node()
            {
                title = "Entry"
            };

            node.CreatePort("Next", Direction.Output);

            node.capabilities &= ~Capabilities.Deletable;
            node.capabilities &= ~Capabilities.Movable;
            node.capabilities &= ~Capabilities.Collapsible;
            node.capabilities &= ~Capabilities.Copiable;
            
            AddNode(node, Vector2.zero);
        }

        public void AddNode(Node node, Vector2 position)
        {
            node.SetPosition(new Rect(position, _defaultNodeSize));
            AddElement(node);
        }
        
        public void AddEmptyNode(string title, Vector2 position)
        {
            var node = CreateEmptyDialogueNode(title);
            AddNode(node, position);
        }
        
        private Node CreateEmptyDialogueNode(string title)
        {
            var node = new DialogueNode()
            {
                title = title
            };

            node.CreatePort("In", Direction.Input, Port.Capacity.Multi);
            node.CreatePort("Out", Direction.Output, Port.Capacity.Multi);
            return node;
        }
    }
}