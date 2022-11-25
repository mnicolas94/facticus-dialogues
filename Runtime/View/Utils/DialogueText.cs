using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Dialogues.View.Utils
{
    public class DialogueText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textUi;
        [SerializeField] private float _letterAppearCooldown;

        public async Task ShowText(string text, CancellationToken ct)
        {
            _textUi.text = text;
            await ShowCharacterByCharacter(ct);
        }

        public void ShowAll()
        {
            var textColor = _textUi.color;
            textColor.a = 1;
            _textUi.color = textColor;
        }
        
        private async Task ShowCharacterByCharacter(CancellationToken ct)
        {
            DisappearText();
            await Task.Yield();  // wait one frame for TMPro to render text
            
            int characterCount = _textUi.textInfo.characterCount;
            int i = 0;
            while (i < characterCount && !ct.IsCancellationRequested)
            {
                await AsyncUtils.Utils.Delay(_letterAppearCooldown, ct);
                
                SetCharacterAlpha(i, 255);

                i++;
            }
        }

        private void SetCharacterAlpha(int characterIndex, byte alpha) {
            var charInfo = _textUi.textInfo.characterInfo[characterIndex];
            int meshIndex = charInfo.materialReferenceIndex; 
            int vertexIndex = charInfo.vertexIndex;

            Color32[] vertexColors = _textUi.textInfo.meshInfo[meshIndex].colors32;

            if(charInfo.isVisible)
            {
                var color = vertexColors[vertexIndex + 0];
                color.a = alpha;
                vertexColors[vertexIndex + 0] = color;
                vertexColors[vertexIndex + 1] = color;
                vertexColors[vertexIndex + 2] = color;
                vertexColors[vertexIndex + 3] = color;
            }

            _textUi.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }

        private void DisappearText()
        {
            var textColor = _textUi.color;
            textColor.a = 0;
            _textUi.color = textColor;
        }
    }
}
