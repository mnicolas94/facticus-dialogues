using Dialogues.Editor.Utils;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;
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
        private DialoguesDatabase _database;

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
            _database = GetDefaultOrCreateDatabase();
            CreateToolbar();
            CreateGraph();
            CreateMiniMap();
        }

        private void CreateGraph()
        {
            _graphView = new DialogueGraphView(this, _database)
            {
                name = "Dialogue graph view"
            };

            _graphView.style.flexGrow = 1;
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
            _miniMap.AddStyleSheet("Styles/MiniMap");
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

        private DialoguesDatabase GetDefaultOrCreateDatabase()
        {
            DialoguesDatabase database;
            
            var databasesGuids = AssetDatabase.FindAssets($"t:{typeof(DialoguesDatabase).Name}");
            if (databasesGuids.Length > 0)
            {
                var guid = databasesGuids[0];  // get the first one
                var path = AssetDatabase.GUIDToAssetPath(guid);
                database = AssetDatabase.LoadAssetAtPath<DialoguesDatabase>(path);
            }
            else
            {
                // Create a database
                database = CreateInstance<DialoguesDatabase>();
                var path = "Assets/DialoguesDatabase.asset";
                AssetDatabase.CreateAsset(database, path);
                AssetDatabase.SaveAssets();
            }
            
            return database;
        }
    }
}