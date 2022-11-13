using System;
using System.Collections.Generic;
using Dialogues.Checks;
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
        private Port _inPort;
        private Port _outPort;
        private Port _checkPort;
        private Port _triggerPort;

        public DialogueLine DialogueLine => _dialogueLine;

        public ICheck Check => null;
        
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
            
            _inPort = PortsUtils.CreateDialoguePort("In", Direction.Input, Port.Capacity.Multi);
            _outPort = PortsUtils.CreateDialoguePort("Out", Direction.Output, Port.Capacity.Multi);
            _checkPort = PortsUtils.CreateCheckPort("Check", Direction.Input, Port.Capacity.Single);
            _triggerPort = PortsUtils.CreateTriggerPort("Trigger", Direction.Output, Port.Capacity.Multi);
            this.AddPort(_inPort);
            this.AddPort(_outPort);
            this.AddPort(_checkPort);
            this.AddPort(_triggerPort);
            
            RefreshExpandedState();
            RefreshPorts();
        }

        public Node Deserialize()
        {
            return new DialogueNode(_dialogueLine);
        }

        public List<Port> GetPorts()
        {
            return new List<Port>{ _inPort, _outPort, _checkPort, _triggerPort};
        }
    }
}