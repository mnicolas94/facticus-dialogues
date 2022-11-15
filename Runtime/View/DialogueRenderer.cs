using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TNRD;
using UnityEngine;

namespace Dialogues.View
{
    public class DialogueRenderer : MonoBehaviour
    {
        [SerializeField] private List<SerializableInterface<IDialogueLineRenderer>> _renderers;
        
        public async Task RenderDialogue(Dialogue dialogue, CancellationToken ct)
        {
            // get dialogue lines iterator
            var lines = dialogue.Iterate();
            foreach (var dialogueLine in lines)
            {
                var lineData = dialogueLine.LineData;
                // get valid renderers
                var renderers = _renderers.Where(lineRenderer => lineRenderer.Value.CanRenderLine(lineData));
                
                // get rendering async tasks
                var renderingTasks = renderers.Select(lineRenderer => lineRenderer.Value.RenderLine(lineData, ct))
                    .ToList();
                
                // render
                await Task.WhenAll(renderingTasks);
            }
        }
    }
}