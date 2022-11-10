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
        public static readonly string LineDataFieldName = nameof(_lineData);
#endif
    }
}