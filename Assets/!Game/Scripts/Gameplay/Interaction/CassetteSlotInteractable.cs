using System;
using Delphin.Core;
using Delphin.Services;
using UnityEngine;

namespace Delphin.Gameplay
{
    public class CassetteSlotInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private CassetteDefinition requiredCassette;
        [SerializeField] private Transform slotPoint;

        private GameObject installedModel;

        public bool IsFilled { get; private set; }
        public CassetteDefinition InsertedCassette { get; private set; }

        public event Action<ItemDefinition> ItemInserted;
        public event Action<ItemDefinition> ItemEjected;
        public event Action IncompatibleCassetteInserted;

        public string InteractionPrompt => string.Empty;

        public void PrimaryInteract(GameObject interactor)
        {
            if (IsFilled)
                return;

            var equipment = ServiceLocator.Get<IEquipmentService>();
            var equippedItem = equipment.EquippedItem;
            if (equippedItem == null)
                return;

            var equippedCassette = equippedItem as CassetteDefinition;
            if (equippedCassette == null || equippedCassette != requiredCassette)
            {
                IncompatibleCassetteInserted?.Invoke();
                return;
            }

            ServiceLocator.Get<IInventoryService>().RemoveItem(equippedCassette);
            equipment.Unequip();

            InsertedCassette = equippedCassette;
            IsFilled = true;

            if (equippedCassette.WorldModel != null && slotPoint != null)
                installedModel = Instantiate(equippedCassette.WorldModel, slotPoint, false);

            ItemInserted?.Invoke(equippedCassette);
        }

        public void SecondaryInteract(GameObject interactor)
        {
            if (!IsFilled)
                return;

            var ejected = InsertedCassette;
            ServiceLocator.Get<IInventoryService>().AddItem(ejected);

            if (installedModel != null)
                Destroy(installedModel);

            installedModel = null;
            InsertedCassette = null;
            IsFilled = false;

            ItemEjected?.Invoke(ejected);
        }

        public void RecordData(Vector3 coordinates)
        {
            InsertedCassette?.Record(coordinates);
        }
    }
}
