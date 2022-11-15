using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Dialogues.View
{
    public abstract class DialogueLineRendererBase<T> : MonoBehaviour, IDialogueLineRenderer where T : IDialogueLineData
    {
        public bool CanRenderLine(IDialogueLineData line)
        {
            return line is T;
        }

        public async Task RenderLine(IDialogueLineData line, CancellationToken ct)
        {
            await RenderLine((T) line, ct);
        }
        
        public abstract Task RenderLine(T line, CancellationToken ct);
    }
}