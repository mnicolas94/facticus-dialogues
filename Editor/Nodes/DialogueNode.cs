using Dialogues.Editor.Ports;
using Dialogues.Editor.Utils;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogues.Editor.Nodes
{
    public class DialogueNode : Node
    {
        private DialogueLine _dialogueLine;
        private SerializedObject _serializedDialogueLine;
        private SerializedProperty _lineDataProperty;
        
        public DialogueNode() : this(ScriptableObject.CreateInstance<DialogueLine>())
        {
        }

        public DialogueNode(DialogueLine dialogueLine)
        {
            this.AddStyleSheet("Styles/Nodes/DialogueNode");
            _dialogueLine = dialogueLine;
            _serializedDialogueLine = new SerializedObject(dialogueLine);
            _lineDataProperty = _serializedDialogueLine.FindProperty(DialogueLine.LineDataFieldName);
            SetupUi();
        }

        private void SetupUi()
        {
            var imguiContainer = new IMGUIContainer(() =>
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(_lineDataProperty);
                var changed = EditorGUI.EndChangeCheck();
                if (changed)
                {
                    RefreshExpandedState();
                }
            });
            Add(imguiContainer);
            
            this.AddPort(PortsUtils.CreateDialoguePort("In", Direction.Input, Port.Capacity.Multi));
            this.AddPort(PortsUtils.CreateDialoguePort("Out", Direction.Output, Port.Capacity.Multi));
            this.AddPort(PortsUtils.CreateCheckPort("Check", Direction.Input, Port.Capacity.Single));
            this.AddPort(PortsUtils.CreateTriggerPort("Trigger", Direction.Output, Port.Capacity.Multi));
            
            RefreshExpandedState();
            RefreshPorts();
        }
    }
}