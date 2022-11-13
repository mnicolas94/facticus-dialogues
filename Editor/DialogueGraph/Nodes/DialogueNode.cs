using System;
using Dialogues.Editor.DialogueGraph.Ports;
using Dialogues.Editor.DialogueGraph.Utils;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogues.Editor.DialogueGraph.Nodes
{
    [Serializable]
    public sealed class DialogueNode : Node, ISerializableNode
    {
        [SerializeField] private DialogueLine _dialogueLine;
        private SerializedObject _serializedDialogueLine;
        private SerializedProperty _lineDataProperty;

        public DialogueNode(DialogueLine dialogueLine)
        {
            _dialogueLine = dialogueLine;
            _serializedDialogueLine = new SerializedObject(dialogueLine);
            _lineDataProperty = _serializedDialogueLine.FindProperty(DialogueLine.EditorLineDataFieldName);
            SetupUi();
        }

        private void SetupUi()
        {
            title = "Dialogue";
            this.AddStyleSheet("Styles/Nodes/DialogueNode");
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

        public Node Deserialize()
        {
            return new DialogueNode(_dialogueLine);
        }
    }
}