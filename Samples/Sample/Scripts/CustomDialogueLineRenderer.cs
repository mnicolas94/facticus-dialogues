using System.Threading;
using System.Threading.Tasks;
using Dialogues.View;
using Dialogues.View.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogues.Samples.Scripts
{
    public class CustomDialogueLineRenderer : DialogueLineRendererBase<CustomDialogueLineData>
    {
        [SerializeField] private DialogueText _dialogueTextRenderer;
        [SerializeField] private TextMeshProUGUI _nameText;

        [SerializeField] private Button _interactionButton;
        
        public override async Task RenderLine(CustomDialogueLineData line, CancellationToken ct)
        {
            _nameText.text = line.Character.CharacterName;
            _nameText.color = line.Character.NameColor;
            
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var linkedCt = linkedCts.Token;

            try
            {
                var showTextTask = _dialogueTextRenderer.ShowText(line.DialogueText, linkedCt);
                var interactionTask = AsyncUtils.Utils.WaitPressButtonAsync(_interactionButton, linkedCt);

                var finishedTask = await Task.WhenAny(showTextTask, interactionTask);
                await finishedTask;  // hack to propagate exceptions

                linkedCts.Cancel();
                
                // ensure entire text is displayed even if async task was cancelled
                _dialogueTextRenderer.ShowAll();
            }
            finally
            {
                linkedCts.Dispose();
            }
            
            await AsyncUtils.Utils.WaitPressButtonAsync(_interactionButton, ct);
        }
    }
}