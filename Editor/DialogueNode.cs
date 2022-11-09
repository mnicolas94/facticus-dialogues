using Dialogues.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.Editor.EditorGUIUtils;

namespace Dialogues.Editor
{
    public class DialogueNode : Node
    {
        private DialogueLine _dialogueLine;
        private SerializedObject _serializedDialogueLine;
        
        public DialogueNode() : this(ScriptableObject.CreateInstance<DialogueLine>())
        {
        }

        public DialogueNode(DialogueLine dialogueLine)
        {
            _dialogueLine = dialogueLine;
            _serializedDialogueLine = new SerializedObject(dialogueLine);
            SetupUi();
        }

        private void SetupUi()
        {
            var imguiContainer = new IMGUIContainer(() =>
            {
                GUIUtils.DrawSerializedObject(_serializedDialogueLine);
            });
            Add(imguiContainer);
        }
    }
}