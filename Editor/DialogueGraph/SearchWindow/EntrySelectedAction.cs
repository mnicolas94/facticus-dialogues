using System;
using UnityEngine;

namespace Dialogues.Editor.DialogueGraph.SearchWindow
{
    public class EntrySelectedAction
    {
        private Action<DialogueGraphView, Vector2> _action;

        public EntrySelectedAction(Action<DialogueGraphView, Vector2> action)
        {
            _action = action;
        }

        public void OnSelected(DialogueGraphView graphView, Vector2 graphPosition)
        {
            _action?.Invoke(graphView, graphPosition);
        }
    }
}