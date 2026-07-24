using Delphin.Core;
using Delphin.Services;
using UnityEngine;
using UnityEngine.Events;

namespace Delphin.Gameplay
{
    public class PickupInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemDefinition item;
        [SerializeField] private UnityEvent<GameObject> onPickedUp;

        public string InteractionPrompt => $"Поднять: {item.ItemName}";

        public void PrimaryInteract(GameObject interactor)
        {
            ServiceLocator.Get<IInventoryService>().AddItem(item);
            onPickedUp?.Invoke(interactor);
            gameObject.SetActive(false);
        }

        public void SecondaryInteract(GameObject interactor)
        {
        }
    }
}
