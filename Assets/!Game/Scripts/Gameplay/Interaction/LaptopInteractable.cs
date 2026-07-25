using Delphin.Core;
using Delphin.Services;
using TMPro;
using UnityEngine;

namespace Delphin.Gameplay
{
    public class LaptopInteractable : MonoBehaviour, IInteractable
    {
        private enum LaptopState
        {
            Closed,
            Open,
            Focused
        }

        [SerializeField] private string closedPrompt = "Открыть ноутбук";
        [SerializeField] private string openPrompt = "Осмотреть";
        [SerializeField] private Animation animation;
        [SerializeField] private AnimationClip openClip;
        [SerializeField] private TMP_Text screen;
        [SerializeField, TextArea(2, 4)] private string bootMessage = "ЗАГРУЗКА ОПЕРАЦИОННОЙ СИСТЕМЫ D.E.L.P.H.I.N...";
        [SerializeField] private Transform focusPoint;

        private LaptopState state = LaptopState.Closed;
        private CameraFocusController cameraFocus;
        private IInputService input;
        private IGameStateService gameState;

        public string InteractionPrompt => state switch
        {
            LaptopState.Closed => closedPrompt,
            LaptopState.Open => openPrompt,
            _ => string.Empty
        };

        private void Start()
        {
            SetScreen(string.Empty);
            input = ServiceLocator.Get<IInputService>();
            gameState = ServiceLocator.Get<IGameStateService>();
        }

        private void OnDestroy()
        {
            if (input != null && state == LaptopState.Focused)
                input.CancelPerformed -= OnCancelPerformed;
        }

        public void PrimaryInteract(GameObject interactor)
        {
            switch (state)
            {
                case LaptopState.Closed:
                    Open();
                    break;

                case LaptopState.Open:
                    BeginFocus(interactor);
                    break;
            }
        }

        public void SecondaryInteract(GameObject interactor)
        {
        }

        private void Open()
        {
            state = LaptopState.Open;
            animation.Play(openClip.name);
            SetScreen(bootMessage);
        }

        private void BeginFocus(GameObject interactor)
        {
            if (focusPoint == null)
                return;

            cameraFocus = interactor.GetComponent<CameraFocusController>();
            if (cameraFocus == null)
                return;

            state = LaptopState.Focused;
            gameState.SetState(GameState.Paused);
            cameraFocus.FocusOn(focusPoint);

            input.CancelPerformed += OnCancelPerformed;
        }

        private void OnCancelPerformed()
        {
            if (state != LaptopState.Focused)
                return;

            state = LaptopState.Open;
            input.CancelPerformed -= OnCancelPerformed;
            cameraFocus.ReturnToPlayer(() => gameState.SetState(GameState.Playing));
        }

        private void SetScreen(string text)
        {
            if (screen != null)
                screen.text = text;
        }
    }
}
