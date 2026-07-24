using UnityEngine;
using UnityEngine.Events;

namespace Delphin.Gameplay
{
    public class PressInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string label = "Кнопка";
        [SerializeField] private bool oneShot;
        [SerializeField] private UnityEvent onPressed;

        private bool consumed;

        public string InteractionPrompt => label;

        public void Interact(GameObject interactor)
        {
            if (oneShot && consumed)
                return;

            consumed = true;
            onPressed?.Invoke();
        }

        public void SecondaryInteract(GameObject interactor)
        {
        }
    }
}
