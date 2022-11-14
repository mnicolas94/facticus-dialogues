using System;
using Dialogues.Checks;
using Dialogues.Triggers;
using TNRD;
using UnityEngine;

namespace Dialogues
{
    [Serializable]
    public class DialogueLine : ScriptableObject
    {
        [SerializeField] private SerializableInterface<IDialogueLineData> _lineData;
        [SerializeField] private SerializableInterface<ICheck> _check;
        [SerializeField] private SerializableInterface<ITrigger> _trigger;

#if UNITY_EDITOR
        
        public static readonly string EditorLineDataFieldName = nameof(_lineData);

        public void EditorSetLineData(IDialogueLineData linData)
        {
            if (_lineData == null)
            {
                _lineData = new SerializableInterface<IDialogueLineData>();
            }
            
            _lineData.Value = linData;
        }
        
        public void EditorSetCheck(ICheck check)
        {
            _check.Value = check;
        }
        
        public void EditorSetTrigger(ITrigger trigger)
        {
            _trigger.Value = trigger;
        }
        
#endif
    }
}