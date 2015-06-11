using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Contract
{
    public interface IInventoryService
    {
        IEnumerable<Location> GetAllLocations();
        IEnumerable<Item> GetAllItems();

        Location CreateLocation(Location locationToAdd);
        Location DeleteLocation(Guid locationId);

        Item CreateItem(Item itemToAdd);
        Item DeleteItem(Guid itemId);

        Location AddItemToLocation(Location locationToAddItemTo, Item itemToAdd);
        Location RemoveItemFromLocation(Location locationToRemoveItemFrom, Item itemToRemove);

        Location GetLocation(Guid locationId);
        Item GetItem(Guid itemId);
    }
}
