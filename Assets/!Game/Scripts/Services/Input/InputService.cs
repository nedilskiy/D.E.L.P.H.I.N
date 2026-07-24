using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Delphin.Services
{
    public sealed class InputService : IInputService
    {
        private readonly InputActionAsset actions;
        private InputActionMap playerMap;
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction sprintAction;
        private InputAction crouchAction;
        private InputAction jumpAction;
        private InputAction interactAction;
        private InputAction inventoryAction;
        private InputAction attackAction;
        private InputAction cancelAction;

        public InputActionAsset Actions => actions;
        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool SprintHeld { get; private set; }
        public bool CrouchHeld { get; private set; }

        public event Action JumpPerformed;
        public event Action InteractPerformed;
        public event Action InventoryTogglePerformed;
        public event Action AttackPerformed;
        public event Action CancelPerformed;

        public InputService(InputActionAsset actions)
        {
            this.actions = actions != null ? actions : throw new ArgumentNullException(nameof(actions));
        }

        public void Initialize()
        {
            playerMap = actions.FindActionMap("Player", throwIfNotFound: true);

            moveAction = playerMap.FindAction("Move", throwIfNotFound: true);
            lookAction = playerMap.FindAction("Look", throwIfNotFound: true);
            sprintAction = playerMap.FindAction("Sprint", throwIfNotFound: true);
            crouchAction = playerMap.FindAction("Crouch", throwIfNotFound: true);
            jumpAction = playerMap.FindAction("Jump", throwIfNotFound: true);
            interactAction = playerMap.FindAction("Interact", throwIfNotFound: true);
            inventoryAction = playerMap.FindAction("Inventory", throwIfNotFound: true);
            attackAction = playerMap.FindAction("Attack", throwIfNotFound: true);
            cancelAction = playerMap.FindAction("Cancel", throwIfNotFound: true);

            moveAction.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            moveAction.canceled += _ => MoveInput = Vector2.zero;

            lookAction.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
            lookAction.canceled += _ => LookInput = Vector2.zero;

            sprintAction.performed += _ => SprintHeld = true;
            sprintAction.canceled += _ => SprintHeld = false;

            crouchAction.performed += _ => CrouchHeld = true;
            crouchAction.canceled += _ => CrouchHeld = false;

            jumpAction.performed += _ => JumpPerformed?.Invoke();
            interactAction.performed += _ => InteractPerformed?.Invoke();
            inventoryAction.performed += _ => InventoryTogglePerformed?.Invoke();
            attackAction.performed += _ => AttackPerformed?.Invoke();
            cancelAction.performed += _ => CancelPerformed?.Invoke();

            Enable();
        }

        public void SwitchActionMap(string mapName)
        {
            var map = actions.FindActionMap(mapName, throwIfNotFound: true);
            actions.Disable();
            map.Enable();
        }

        public void Enable()
        {
            playerMap?.Enable();
        }

        public void Disable()
        {
            playerMap?.Disable();
        }

        public void Shutdown()
        {
            actions.Disable();
            JumpPerformed = null;
            InteractPerformed = null;
            InventoryTogglePerformed = null;
            AttackPerformed = null;
            CancelPerformed = null;
        }
    }
}
