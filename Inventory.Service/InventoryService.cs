using Inventory.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service
{
    public class InventoryService : IInventoryService
    {
        public Location AddItemsToLocation(Location locationToAddItemsTo, params ItemBundle[] itemBundlesToAdd)
        {
            var updatedItemBundles = locationToAddItemsTo.ItemBundles
                .Concat(itemBundlesToAdd)
                .GroupBy(itemBundle => itemBundle.ItemInBundle.Id)
                .Select(itemBundleGroup => new ItemBundle(
                    itemBundleGroup.Sum(itemBundle => itemBundle.Amount),
                    itemBundleGroup.First().ItemInBundle)
                );

            return new Location(locationToAddItemsTo.Id, locationToAddItemsTo.Name, updatedItemBundles);
        }

        public Location AddItemsToLocation(Location locationToAddItemsTo, params Item[] itemsToAdd)
        {
            return AddItemsToLocation(locationToAddItemsTo, itemsToAdd.Select(item => new ItemBundle(1, item)).ToArray());
        }

        public Location RemoveItemFromLocation(Location locationToRemoveItemsFrom, params Item[] itemsToRemove)
        {
            var updatedItemBundles = locationToRemoveItemsFrom.ItemBundles
                .Where(itemBundle => itemsToRemove.Contains(itemBundle.ItemInBundle))
                .Select(itemBundle => new ItemBundle(itemBundle.Amount - 1, itemBundle.ItemInBundle))
                .Concat(locationToRemoveItemsFrom.ItemBundles)
                .GroupBy(itemBundle => itemBundle.ItemInBundle.Id)
                .Select(itemBundleGroup => itemBundleGroup.OrderBy(itemBundle => itemBundle.Amount).First())
                .Where(itemBundle => itemBundle.Amount > 0);

            return new Location(locationToRemoveItemsFrom.Id, locationToRemoveItemsFrom.Name, updatedItemBundles);
        }

        public Location CreateLocation(Location locationToAdd)
        {
            return new Location(Guid.NewGuid(), locationToAdd.Name, locationToAdd.ItemBundles);
        }
        
        public Item CreateItem(Item itemToAdd)
        {
            return new Item(Guid.NewGuid(), itemToAdd.Name, itemToAdd.ExpectedLocationName);
        }
    }
}
