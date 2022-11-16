using System;
using Dialogues.Core.Checks;
using Dialogues.Core.Triggers;
using TNRD;
using UnityEngine;

namespace Dialogues.Core
{
    [Serializable]
    public class DialogueLine : ScriptableObject
    {
        [SerializeField] private SerializableInterface<IDialogueLineData> _lineData;
        [SerializeField] private SerializableInterface<ICheck> _check;
        [SerializeField] private SerializableInterface<ITrigger> _trigger;

        public IDialogueLineData LineData => _lineData.Value;

        public bool EvaluateCheck()
        {
            return _check?.Value == null || _check.Value.IsMet();
        }

        public void ExecuteTrigger()
        {
            _trigger.Value?.Invoke();
        }

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