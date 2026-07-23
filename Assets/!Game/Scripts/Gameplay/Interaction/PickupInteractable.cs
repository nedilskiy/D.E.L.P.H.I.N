using UnityEngine;
using UnityEngine.Events;

namespace Delphin.Gameplay
{
    public class PickupInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string itemName = "Item";
        [SerializeField] private UnityEvent<GameObject> onPickedUp;

        public string InteractionPrompt => $"Поднять: {itemName}";

        public void Interact(GameObject interactor)
        {
            onPickedUp?.Invoke(interactor);
            gameObject.SetActive(false);
        }
    }
}
