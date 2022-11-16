using TNRD;
using UnityEngine;
using Utils.Serializables;

namespace Dialogues.Core.Checks
{
    public class Check : ScriptableObject, ICheck
    {
        [SerializeField, HideInInspector] private string _displayName;
        [SerializeField] private SerializableInterface<ISerializablePredicate> _predicate;

        public bool IsMet()
        {
            var predicate = _predicate.Value;
            return predicate != null && predicate.IsMet();
        }
    }
}