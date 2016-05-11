using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Contract
{
    public interface IInventoryService
    {
        Location AddItemsToLocation(Location locationToAddItemsTo, params ItemBundle[] itemBundlesToAdd);

        Location AddItemsToLocation(Location locationToAddItemsTo, params Item[] itemsToAdd);

        Location RemoveItemFromLocation(Location locationToRemoveItemsFrom, params Item[] itemsToRemove);

        Location CreateLocation(Location locationToAdd);

        Item CreateItem(Item itemToAdd);
    }
}
