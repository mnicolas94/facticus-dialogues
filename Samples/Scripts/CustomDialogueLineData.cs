using System;
using UnityEngine;

namespace Dialogues.Samples.Scripts
{
    [Serializable]
    public class CustomDialogueLineData : IDialogueLineData
    {
        [SerializeField] private string _characterName;
        [SerializeField] private Sprite _characterSprite;

        [Space(12)]
        [SerializeField] private string _dialogueText;

        public override string ToString()
        {
            return $"{_characterName}: {_dialogueText}";
        }
    }
}