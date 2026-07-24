using System;
using System.Collections.Generic;
using Delphin.Core;

namespace Delphin.Services
{
    public interface IInventoryService : IGameService
    {
        IReadOnlyList<ItemDefinition> Items { get; }
        event Action ItemsChanged;
        void AddItem(ItemDefinition item);
    }
}
