using Inventory.Contract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Persistence.InMemory
{
    public class InMemoryItemStore : IItemStore
    {
        private ConcurrentDictionary<Guid, Item> _items = new ConcurrentDictionary<Guid, Item>();

        public Item GetItemById(Guid itemId)
        {
            Item result;
            if (_items.TryGetValue(itemId, out result))
                return result;

            return null;
        }

        public Item UpsertItem(Item latestOrNewItem)
        {
            _items.TryAdd(latestOrNewItem.Id, latestOrNewItem);
            return GetItemById(latestOrNewItem.Id);
        }

        public Item DeleteItem(Guid itemId)
        {
            Item deletedItem = null;
            _items.TryRemove(itemId, out deletedItem);
            return deletedItem;
        }

        IEnumerable<Item> IItemStore.GetItems()
        {
            return _items.Values;
        }
    }
}
