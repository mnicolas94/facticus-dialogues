﻿using System;
using TNRD;
using UnityEngine;

namespace Dialogues.Checks
{
    [Serializable]
    public class NegationCheck : ICheck
    {
        [SerializeField] private SerializableInterface<ICheck> _check;
        
        public bool IsMet()
        {
            return !_check.Value.IsMet();
        }
    }
}