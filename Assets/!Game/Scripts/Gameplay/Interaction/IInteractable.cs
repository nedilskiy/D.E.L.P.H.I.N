using UnityEngine;

namespace Delphin.Gameplay
{
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        void PrimaryInteract(GameObject interactor);
        void SecondaryInteract(GameObject interactor);
    }
}
