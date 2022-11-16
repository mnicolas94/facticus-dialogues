using System;
using System.Threading;
using UnityEngine;

namespace Dialogues.Samples.Scripts
{
    public class DialogueLauncher : MonoBehaviour
    {
        [SerializeField] private DialoguePopup _popupPrefab;
        [SerializeField] private Dialogue _dialogueToShow;

        private CancellationTokenSource _cts;

        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
        }

        private void OnDisable()
        {
            if (!_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }
            
            _cts.Dispose();
        }

        [ContextMenu("Display dialogue")]
        public void DisplayDialogue()
        {
            var ct = _cts.Token;
            AsyncUtils.Popups.ShowPopup(_popupPrefab, _dialogueToShow, ct);
        }
    }
}