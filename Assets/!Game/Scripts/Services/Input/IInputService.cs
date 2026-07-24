using System;
using Delphin.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Delphin.Services
{
    public interface IInputService : IGameService
    {
        InputActionAsset Actions { get; }
        Vector2 MoveInput { get; }
        Vector2 LookInput { get; }
        bool SprintHeld { get; }
        bool CrouchHeld { get; }

        event Action JumpPerformed;
        event Action InteractPerformed;
        event Action InventoryTogglePerformed;
        event Action AttackPerformed;
        event Action CancelPerformed;

        void SwitchActionMap(string mapName);
        void Enable();
        void Disable();
    }
}
