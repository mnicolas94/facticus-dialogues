using System;
using UnityEngine;

namespace Dialogues.Runtime
{
    public interface IDialogueLineData
    {
    }

    [Serializable]
    public class TextLine : IDialogueLineData
    {
        [SerializeField] private string _line;
    }
}