using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogues.Editor
{
    public class DialogueGraphEditor : EditorWindow
    {
        private DialogueGraphView _graphView;
        private MiniMap _miniMap;
        private Toolbar _toolBar;
        private VisualElement _toolBarLeftLayout;
        private VisualElement _toolBarRightLayout;

        private bool ShowMiniMap
        {
            get => _miniMap.style.display == DisplayStyle.Flex;
            set => _miniMap.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }

        [MenuItem("Graph/Open")]
        public static void OpenWindow()
        {
            var window = GetWindow<DialogueGraphEditor>();
            window.titleContent = new GUIContent("Dialogue editor");
        }

        private void OnEnable()
        {
            CreateGraph();
            CreateToolbar();
            CreateMiniMap();
        }

        private void CreateGraph()
        {
            _graphView = new DialogueGraphView(this)
            {
                name = "Dialogue graph view"
            };

            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void CreateToolbar()
        {
            _toolBar = new Toolbar();
            _toolBarLeftLayout = new VisualElement();
            _toolBarRightLayout = new VisualElement();
            
            _toolBarLeftLayout.style.flexGrow = 1;
            _toolBarRightLayout.style.flexGrow = 1;
            
            _toolBarLeftLayout.style.flexShrink = 0;
            _toolBarRightLayout.style.flexShrink = 0;
            
            _toolBarRightLayout.style.flexDirection = FlexDirection.RowReverse;
            _toolBarLeftLayout.style.flexDirection = FlexDirection.Row;

            _toolBar.Add(_toolBarLeftLayout);
            _toolBar.Add(_toolBarRightLayout);
            
            rootVisualElement.Add(_toolBar);
        }

        private void CreateMiniMap()
        {
            _miniMap = new MiniMap();
            _miniMap.anchored = true;
            _miniMap.style.display = DisplayStyle.None;  // start hidden
            _miniMap.styleSheets.Add(Resources.Load<StyleSheet>("MiniMap"));
            _graphView.Add(_miniMap);
            
            var miniMapButton = new ToolbarToggle();
            miniMapButton.text = "MiniMap";
            miniMapButton.RegisterCallback<ChangeEvent<bool>>((@event) => ShowMiniMap = @event.newValue);
            _toolBarRightLayout.Add(miniMapButton);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }
    }
}