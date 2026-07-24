using System;

namespace Delphin.Services
{
    public sealed class EquipmentService : IEquipmentService
    {
        public ItemDefinition EquippedItem { get; private set; }
        public event Action<ItemDefinition> EquippedChanged;

        public void Initialize()
        {
        }

        public void Equip(ItemDefinition item)
        {
            if (item == null || item == EquippedItem)
                return;

            EquippedItem = item;
            EquippedChanged?.Invoke(item);
        }

        public void Unequip()
        {
            if (EquippedItem == null)
                return;

            EquippedItem = null;
            EquippedChanged?.Invoke(null);
        }

        public void Shutdown()
        {
            EquippedItem = null;
            EquippedChanged = null;
        }
    }
}
