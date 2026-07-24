using System;
using Delphin.Core;

namespace Delphin.Services
{
    public interface IEquipmentService : IGameService
    {
        ItemDefinition EquippedItem { get; }
        event Action<ItemDefinition> EquippedChanged;
        void Equip(ItemDefinition item);
        void Unequip();
    }
}
