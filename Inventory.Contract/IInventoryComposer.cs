using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Contract
{
    public interface IInventoryComposer
    {
        Location AddItemsToLocation(Guid locationId, params ItemBundle[] itemsToAdd);
        ItemBundle GetItemBundleFromLocationById(Guid locationId, Guid itemId);
        Item GetItemById(Guid itemId);
        Location GetLocationById(Guid locationId);
        Item CreateItem(Item itemToCreate);
        Item StoreItem(Item itemToStore);
        Location StoreLocation(Location locationToStore);
        Location CreateLocation(Location locationToCreate);
    }
}
