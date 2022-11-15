using System.Threading;
using System.Threading.Tasks;
using AsyncUtils;
using Dialogues.View;
using UnityEngine;

namespace Dialogues.Samples.Scripts
{
    public class DialoguePopup : AsyncPopupInitializable<Dialogue>
    {
        [SerializeField] private DialogueRenderer _dialogueRenderer;
        
        private Dialogue _dialogue;
        
        public override async Task Show(CancellationToken ct)
        {
            await _dialogueRenderer.RenderDialogue(_dialogue, ct);
        }

        public override void Initialize(Dialogue dialogue)
        {
            _dialogue = dialogue;
        }
    }
}