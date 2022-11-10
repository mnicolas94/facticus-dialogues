using TNRD;
using UnityEngine;
using Utils.Serializables;

namespace Dialogues.Checks
{
    public class Check : ScriptableObject, ICheck
    {
        [SerializeField, HideInInspector] private string _displayName;
        [SerializeField] private SerializableInterface<ISerializablePredicate> _predicate;

        public string DisplayName => _displayName;

        public bool IsMet()
        {
            return _predicate.Value.IsMet();
        }
    }
}