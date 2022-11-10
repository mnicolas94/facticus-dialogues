using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues.Triggers
{
    [Serializable]
    public class TriggerList : ITrigger
    {
        [SerializeField] private List<Trigger> _triggers;
        
        public void Invoke()
        {
            foreach (var trigger in _triggers)
            {
                trigger.Invoke();
            }
        }
    }
}