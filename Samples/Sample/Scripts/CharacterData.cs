using UnityEngine;

namespace Dialogues.Samples.Scripts
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Facticus/Dialogues/Samples/CharacterData", order = 0)]
    public class CharacterData : ScriptableObject
    {
        [SerializeField] private string _characterName;
        [SerializeField] private Color _nameColor;

        public string CharacterName => _characterName;

        public Color NameColor => _nameColor;
    }
}