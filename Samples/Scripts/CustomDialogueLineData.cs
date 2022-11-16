using System;
using Dialogues.Core;
using UnityEngine;

namespace Dialogues.Samples.Scripts
{
    [Serializable]
    public class CustomDialogueLineData : IDialogueLineData
    {
        [SerializeField] private CharacterData _character;

        [Space(12)]
        [SerializeField] private string _dialogueText;

        public CharacterData Character => _character;

        public string DialogueText => _dialogueText;

        public override string ToString()
        {
            return $"{_character.CharacterName}: {_dialogueText}";
        }
    }
}