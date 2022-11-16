using UnityEngine;
using UnityEngine.Events;

namespace Dialogues.Core.Triggers
{
    public class Trigger : ScriptableObject, ITrigger
    {
        [SerializeField] private UnityEvent _event;

        public void Invoke()
        {
            _event.Invoke();
        }
    }
}