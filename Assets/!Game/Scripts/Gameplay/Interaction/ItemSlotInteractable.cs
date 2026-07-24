using System;
using Delphin.Core;
using Delphin.Services;
using UnityEngine;

namespace Delphin.Gameplay
{
    public class ItemSlotInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemDefinition requiredItem;
        [SerializeField] private Transform slotPoint;
        [SerializeField] private float ejectForce = 3f;

        private GameObject installedModel;

        public bool IsFilled { get; private set; }
        public event Action<ItemDefinition> ItemInserted;

        public string InteractionPrompt => string.Empty;

        public void PrimaryInteract(GameObject interactor)
        {
            if (IsFilled)
                return;

            var equipment = ServiceLocator.Get<IEquipmentService>();
            if (equipment.EquippedItem != requiredItem)
                return;

            ServiceLocator.Get<IInventoryService>().RemoveItem(requiredItem);
            equipment.Unequip();

            IsFilled = true;

            if (requiredItem.WorldModel != null && slotPoint != null)
                installedModel = Instantiate(requiredItem.WorldModel, slotPoint, false);

            ItemInserted?.Invoke(requiredItem);
        }

        public void SecondaryInteract(GameObject interactor)
        {
        }

        public void BurnAndEject()
        {
            if (!IsFilled)
                return;

            IsFilled = false;

            if (installedModel == null)
                return;

            installedModel.transform.SetParent(null, true);

            var rb = installedModel.GetComponent<Rigidbody>();
            if (rb == null)
                rb = installedModel.AddComponent<Rigidbody>();

            rb.AddForce((Vector3.up + UnityEngine.Random.insideUnitSphere * 0.5f) * ejectForce, ForceMode.Impulse);

            installedModel = null;
        }
    }
}
