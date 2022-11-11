using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogues.Editor.Nodes
{
    /// <summary>
    /// source: https://forum.unity.com/threads/graphview-state-machine-graph.960057/#post-7115239
    /// </summary>
    public class RenamableNode : Node
    {
        public Action<string> OnRename;
        
        protected Label _titleLabel;
        protected TextField _renameField;
 
        public string ID {
            get => title;
            set {
                if (string.IsNullOrEmpty(value) || value.Equals("(Unnamed)"))
                {
                    title = "(Unnamed)";
                    _titleLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
                }
                else
                {
                    title = value;
                    _titleLabel.style.unityFontStyleAndWeight = FontStyle.Normal;
                }
            }
        }
 
        public RenamableNode()
        {
            capabilities |= Capabilities.Renamable;
 
            titleContainer.RegisterCallback<MouseDownEvent>(MouseRename, TrickleDown.TrickleDown);
 
            focusable = true;
            pickingMode = PickingMode.Position;
            RegisterCallback<KeyDownEvent>(KeyboardShortcuts, TrickleDown.TrickleDown);
 
            _titleLabel = titleContainer.Q<Label>();
 
            _renameField = new TextField { name = "textField", isDelayed = true };
            _renameField.style.display = DisplayStyle.None;
            _renameField.ElementAt(0).style.fontSize = _titleLabel.style.fontSize;
            _renameField.ElementAt(0).style.height = 18f; // still need to figure out how to make these values depend on the label's font size
            _renameField.style.paddingTop = 8.5f;
            _renameField.style.paddingLeft = 4f;
            _renameField.style.paddingRight = 4f;
            _renameField.style.paddingBottom = 7.5f;
            titleContainer.Insert(1, _renameField);
 
            VisualElement textInput = _renameField.Q(TextField.textInputUssName);
            textInput.RegisterCallback<FocusOutEvent>(EndRename);
        }
 
        private void MouseRename(MouseDownEvent evt)
        {
            if(evt.clickCount == 2 && evt.button == (int)MouseButton.LeftMouse && IsRenamable())
                StartRename();
        }
 
        private void KeyboardShortcuts(KeyDownEvent evt)
        {
            if(evt.keyCode == KeyCode.F2 && IsRenamable())
                StartRename();
        }
 
        public void StartRename()
        {
            _titleLabel.style.display = DisplayStyle.None;
            _renameField.SetValueWithoutNotify(ID);
            _renameField.style.display = DisplayStyle.Flex;
            _renameField.Q(TextField.textInputUssName).Focus();
            _renameField.SelectAll();
        }
 
        private void EndRename(FocusOutEvent evt)
        {
            _titleLabel.style.display = DisplayStyle.Flex;
            _renameField.style.display = DisplayStyle.None;
 
            if (ID != _renameField.text)
            {
                ID = _renameField.text;
                OnRename?.Invoke(ID);
            }
        }
    }
}