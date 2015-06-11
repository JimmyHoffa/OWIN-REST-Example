using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Contract
{
    public interface IItemStore
    {
        IEnumerable<Item> GetItems();
        Item GetItemById(Guid itemId);
        Item UpsertItem(Item latestOrNewItem);
        Item DeleteItem(Guid itemId);
    }
}
