using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues.Core.Triggers
{
    [Serializable]
    public class TriggerList : ITrigger
    {
        [SerializeField] private List<Trigger> _triggers;

        public TriggerList(List<Trigger> triggers)
        {
            _triggers = triggers;
        }

        public TriggerList() : this(new List<Trigger>())
        {
        }

        public void Invoke()
        {
            foreach (var trigger in _triggers)
            {
                trigger.Invoke();
            }
        }
    }
}