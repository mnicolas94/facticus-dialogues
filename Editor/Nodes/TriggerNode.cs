using Dialogues.Triggers;
using UnityEditor.Experimental.GraphView;

namespace Dialogues.Editor.Nodes
{
    public sealed class TriggerNode : Node
    {
        private Trigger _trigger;

        public TriggerNode(Trigger trigger)
        {
            _trigger = trigger;
        }
    }
}