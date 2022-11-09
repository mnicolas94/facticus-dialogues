using System.Collections.Generic;
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
        
        public void Initialize(EditorWindow editorWindow, DialogueGraphView graphView)
        {
            _editorWindow = editorWindow;
            _graphView = graphView;
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>();
            var title = new SearchTreeGroupEntry(new GUIContent("Nodes"), 0);
            tree.Add(title);
            
            var defaultEntry = new SearchTreeEntry(new GUIContent("Empty dialogue"))
            {
                level = 1
            };
            
            tree.Add(defaultEntry);

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var screenPosition = context.screenMousePosition;
            var editorPosition = screenPosition - _editorWindow.position.position;
            var mouseGraphPosition = _graphView.contentViewContainer.WorldToLocal(editorPosition);
            
            _graphView.AddEmptyNode("Dialogue", mouseGraphPosition);
            return true;
        }
    }
}