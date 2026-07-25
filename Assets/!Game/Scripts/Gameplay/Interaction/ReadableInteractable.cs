using Delphin.Core;
using Delphin.Services;
using Delphin.UI;
using UnityEngine;

namespace Delphin.Gameplay
{
    public class ReadableInteractable : MonoBehaviour, IInteractable
    {
        private enum ReadState
        {
            Idle,
            Zoomed,
            Reading
        }

        [SerializeField] private string title = "Записка";
        [SerializeField, TextArea(4, 12)] private string body;
        [SerializeField] private Transform focusPoint;

        private ReadState state = ReadState.Idle;
        private CameraFocusController cameraFocus;
        private DocumentReaderView documentReader;
        private IInputService input;
        private IGameStateService gameState;

        public string InteractionPrompt => "Осмотреть";

        private void Start()
        {
            input = ServiceLocator.Get<IInputService>();
            gameState = ServiceLocator.Get<IGameStateService>();
            documentReader = FindFirstObjectByType<DocumentReaderView>();
        }

        private void OnDestroy()
        {
            if (input != null && state != ReadState.Idle)
                input.CancelPerformed -= OnCancelPerformed;
        }

        public void PrimaryInteract(GameObject interactor)
        {
            switch (state)
            {
                case ReadState.Idle:
                    BeginFocus(interactor);
                    break;

                case ReadState.Zoomed:
                    OpenDocument();
                    break;
            }
        }

        public void SecondaryInteract(GameObject interactor)
        {
        }

        private void BeginFocus(GameObject interactor)
        {
            if (focusPoint == null)
                return;

            cameraFocus = interactor.GetComponent<CameraFocusController>();
            if (cameraFocus == null)
                return;

            state = ReadState.Zoomed;
            gameState.SetState(GameState.Paused);
            cameraFocus.FocusOn(focusPoint);

            input.CancelPerformed += OnCancelPerformed;
        }

        private void OpenDocument()
        {
            state = ReadState.Reading;
            documentReader?.Show(title, body);
        }

        private void OnCancelPerformed()
        {
            switch (state)
            {
                case ReadState.Reading:
                    documentReader?.Hide();
                    state = ReadState.Zoomed;
                    break;

                case ReadState.Zoomed:
                    state = ReadState.Idle;
                    input.CancelPerformed -= OnCancelPerformed;
                    cameraFocus.ReturnToPlayer(() => gameState.SetState(GameState.Playing));
                    break;
            }
        }
    }
}
