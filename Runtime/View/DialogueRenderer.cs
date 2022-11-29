using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dialogues.Core;
using TNRD;
using UnityEngine;

namespace Dialogues.View
{
    public class DialogueRenderer : MonoBehaviour
    {
        [SerializeField, Tooltip("Whether triggers will be executed or not. If false, line renderers should " +
                                 "take care of executing the lines' triggers")]
        private bool _handleTriggers = true;
        [SerializeField, Tooltip("Whether dialogue lines' triggers should be executed after rendering the line. " +
                                 "Otherwise, they will be triggered after he line is rendered")]
        private bool _triggerAfterRendering;
        [SerializeField] private List<SerializableInterface<IDialogueLineRenderer>> _renderers;
        
        public async Task RenderDialogue(Dialogue dialogue, CancellationToken ct)
        {
            var executeTriggersBefore = _handleTriggers && !_triggerAfterRendering;
            var executeTriggersAfter = _handleTriggers && _triggerAfterRendering;
            
            // get dialogue lines iterator
            var lines = dialogue.Iterate(executeTriggersBefore);
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

                if (executeTriggersAfter)
                {
                    dialogueLine.ExecuteTrigger();
                }
            }
        }
    }
}