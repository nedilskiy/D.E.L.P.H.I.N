using Delphin.Core;
using Delphin.Services;
using UnityEngine;

namespace Delphin.Gameplay
{
    public class EquippedItemView : MonoBehaviour
    {
        [SerializeField] private Transform handSocket;

        private IEquipmentService equipment;
        private GameObject currentInstance;

        private void Start()
        {
            equipment = ServiceLocator.Get<IEquipmentService>();
            equipment.EquippedChanged += OnEquippedChanged;

            if (equipment.EquippedItem != null)
                OnEquippedChanged(equipment.EquippedItem);
        }

        private void OnDestroy()
        {
            if (equipment != null)
                equipment.EquippedChanged -= OnEquippedChanged;
        }

        private void OnEquippedChanged(ItemDefinition item)
        {
            if (currentInstance != null)
                Destroy(currentInstance);

            if (item == null || item.WorldModel == null)
                return;

            currentInstance = Instantiate(item.WorldModel, handSocket, false);
        }
    }
}
