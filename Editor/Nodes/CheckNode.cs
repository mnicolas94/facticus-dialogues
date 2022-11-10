using Dialogues.Checks;
using UnityEditor.Experimental.GraphView;

namespace Dialogues.Editor.Nodes
{
    public sealed class CheckNode : Node
    {
        private Check _check;

        public CheckNode(Check check)
        {
            _check = check;
            title = DialoguesDatabase.GetCheckDisplayName(check);
        }
    }
}