using UnityEngine;

namespace Delphin.Gameplay
{
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        void Interact(GameObject interactor);
        void SecondaryInteract(GameObject interactor);
    }
}
