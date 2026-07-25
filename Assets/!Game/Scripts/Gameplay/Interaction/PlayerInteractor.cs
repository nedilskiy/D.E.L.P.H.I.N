using System;
using Delphin.Core;
using Delphin.Services;
using UnityEngine;

namespace Delphin.Gameplay
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private Transform rayOrigin;
        [SerializeField] private float interactRange = 2.5f;
        [SerializeField] private LayerMask interactableMask = ~0;

        private IInputService input;
        private IInteractable focused;

        public IInteractable Focused => focused;
        public event Action<IInteractable> FocusChanged;

        private void Reset()
        {
            rayOrigin = transform;
        }

        private void Start()
        {
            input = ServiceLocator.Get<IInputService>();
            input.AttackPerformed += OnPrimaryPerformed;
            input.CancelPerformed += OnSecondaryPerformed;
        }

        private void OnDestroy()
        {
            if (input != null)
            {
                input.AttackPerformed -= OnPrimaryPerformed;
                input.CancelPerformed -= OnSecondaryPerformed;
            }
        }

        private void Update()
        {
            UpdateFocus();
        }

        private void UpdateFocus()
        {
            IInteractable hitInteractable = null;

            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out var hit, interactRange, interactableMask, QueryTriggerInteraction.Ignore))
                hitInteractable = hit.collider.GetComponentInParent<IInteractable>();

            if (hitInteractable == focused)
                return;

            focused = hitInteractable;
            FocusChanged?.Invoke(focused);
        }

        private void OnPrimaryPerformed()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
                return;

            focused?.PrimaryInteract(gameObject);
            FocusChanged?.Invoke(focused);
        }

        private void OnSecondaryPerformed()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
                return;

            focused?.SecondaryInteract(gameObject);
            FocusChanged?.Invoke(focused);
        }
    }
}
