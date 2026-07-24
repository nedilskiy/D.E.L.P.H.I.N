using System;
using System.Collections.Generic;

namespace Delphin.Services
{
    public sealed class InventoryService : IInventoryService
    {
        private readonly List<ItemDefinition> items = new();

        public IReadOnlyList<ItemDefinition> Items => items;
        public event Action ItemsChanged;

        public void Initialize()
        {
        }

        public void AddItem(ItemDefinition item)
        {
            if (item == null)
                return;

            items.Add(item);
            ItemsChanged?.Invoke();
        }

        public void RemoveItem(ItemDefinition item)
        {
            if (item == null || !items.Remove(item))
                return;

            ItemsChanged?.Invoke();
        }

        public void Shutdown()
        {
            items.Clear();
            ItemsChanged = null;
        }
    }
}
