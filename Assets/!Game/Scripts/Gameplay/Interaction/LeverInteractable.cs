using System;
using Delphin.Core;
using Delphin.Services;
using UnityEngine;

namespace Delphin.Gameplay
{
    public class LeverInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform handle;
        [SerializeField] private float restAngle;
        [SerializeField] private float pulledAngle = -90f;
        [SerializeField] private float dragSpeed = 0.003f;

        private IInputService input;
        private IGameStateService gameState;
        private float pull;
        private bool isDragging;
        private bool firedThisPull;

        public event Action Pulled;

        public string InteractionPrompt => string.Empty;

        private void Start()
        {
            input = ServiceLocator.Get<IInputService>();
            gameState = ServiceLocator.Get<IGameStateService>();
        }

        private void OnDestroy()
        {
            if (input != null && isDragging)
                input.AttackCanceled -= OnReleased;
        }

        public void PrimaryInteract(GameObject interactor)
        {
            if (isDragging)
                return;

            isDragging = true;
            firedThisPull = false;
            gameState.SetState(GameState.Paused);
            input.AttackCanceled += OnReleased;
        }

        public void SecondaryInteract(GameObject interactor)
        {
        }

        private void Update()
        {
            if (!isDragging)
                return;

            pull = Mathf.Clamp01(pull - input.LookInput.y * dragSpeed);

            if (handle != null)
                handle.localRotation = Quaternion.Euler(Mathf.Lerp(restAngle, pulledAngle, pull), 0f, 0f);

            if (pull >= 1f && !firedThisPull)
            {
                firedThisPull = true;
                Pulled?.Invoke();
            }
        }

        private void OnReleased()
        {
            isDragging = false;
            input.AttackCanceled -= OnReleased;
            gameState.SetState(GameState.Playing);

            pull = 0f;
            if (handle != null)
                handle.localRotation = Quaternion.Euler(restAngle, 0f, 0f);
        }
    }
}
