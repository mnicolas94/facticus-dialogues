using System;
using System.Collections.Generic;
using TNRD;
using UnityEngine;

namespace Dialogues.Runtime
{
    [Serializable]
    public class DialogueLine : ScriptableObject
    {
        [SerializeField] private List<SerializableInterface<IDialogueLineData>> _lineData;
    }
    
    
}