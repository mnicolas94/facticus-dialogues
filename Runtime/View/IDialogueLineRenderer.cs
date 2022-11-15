using System.Threading;
using System.Threading.Tasks;

namespace Dialogues.View
{
    public interface IDialogueLineRenderer
    {
        bool CanRenderLine(IDialogueLineData line);
        Task RenderLine(IDialogueLineData line, CancellationToken ct);
    }
}