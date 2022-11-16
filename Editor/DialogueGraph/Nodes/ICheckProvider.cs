using Dialogues.Core.Checks;

namespace Dialogues.Editor.DialogueGraph.Nodes
{
    public interface ICheckProvider
    {
        ICheck GetCheck();
    }
}