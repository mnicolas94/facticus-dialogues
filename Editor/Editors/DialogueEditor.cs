using Dialogues.Core;
using Dialogues.Editor.DialogueGraph;
using UnityEditor;
using UnityEngine;

namespace Dialogues.Editor.Editors
{
    [CustomEditor(typeof(Dialogue))]
    public class DialogueEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Open in editor"))
            {
                var dialogue = (Dialogue) target;
                DialogueGraphEditor.EditDialogue(dialogue);
            }
        }
    }
}