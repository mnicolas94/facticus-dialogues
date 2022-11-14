using System;
using TNRD;
using UnityEngine;
using Utils.Serializables;

namespace Dialogues.Checks
{
    [Serializable]
    public class BinaryCheck : ICheck
    {
        [SerializeField] private BinaryOperation _operation;
        [SerializeField] private SerializableInterface<ICheck> _checkA;
        [SerializeField] private SerializableInterface<ICheck> _checkB;

        public BinaryCheck(BinaryOperation operation, ICheck checkA, ICheck checkB)
        {
            _operation = operation;
            _checkA = new SerializableInterface<ICheck>{Value = checkA};
            _checkB = new SerializableInterface<ICheck>{Value = checkB};
        }

        public BinaryCheck() : this(BinaryOperation.AND, null, null)
        {
        }

        public bool IsMet()
        {
            var valueA = _checkA.Value.IsMet();
            var valueB = _checkB.Value.IsMet();

            switch (_operation)
            {
                case BinaryOperation.AND: return valueA && valueB;
                case BinaryOperation.OR: return valueA || valueB;
                case BinaryOperation.XOR: return valueA ^ valueB;
                case BinaryOperation.NAND: return !(valueA && valueB);
                case BinaryOperation.NOR: return !(valueA || valueB);
                case BinaryOperation.XNOR: return !(valueA ^ valueB);
                default: return valueA;  // unreachable
            }
        }
    }
}