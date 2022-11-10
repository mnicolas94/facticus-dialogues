using Dialogues.Checks;
using UnityEditor.Experimental.GraphView;

namespace Dialogues.Editor.Nodes
{
    public class BooleanNode : Node
    {
        private BinaryOperation _booleanOperation;

        public BooleanNode(BinaryOperation booleanOperation)
        {
            _booleanOperation = booleanOperation;
        }
    }
}