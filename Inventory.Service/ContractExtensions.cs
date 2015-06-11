using Inventory.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service
{
    public static class ContractExtensions
    {
        public static Location AddItems(this Location locationToAddItemsTo, params Item[] items)
        {
            var updatedItemBundles = locationToAddItemsTo.ItemBundles
                .Concat(items.Select(item => new ItemBundle(1, item))
                .GroupBy(itemBundle => itemBundle.ItemInBundle.Id)
                .Select(itemBundleGroup => new ItemBundle(
                    itemBundleGroup.Sum(itemBundle => itemBundle.Amount),
                    itemBundleGroup.First().ItemInBundle)
                ));

            return new Location(locationToAddItemsTo.Id, locationToAddItemsTo.Name, updatedItemBundles);
        }

        public static Location RemoveItems(this Location locationToRemoveItemsFrom, params Item[] items)
        {
            var updatedItemBundles = locationToRemoveItemsFrom.ItemBundles
                .Where(itemBundle => items.Contains(itemBundle.ItemInBundle))
                .Select(itemBundle => new ItemBundle(itemBundle.Amount - 1, itemBundle.ItemInBundle))
                .Concat(locationToRemoveItemsFrom.ItemBundles)
                .GroupBy(itemBundle => itemBundle.ItemInBundle.Id)
                .Select(itemBundleGroup => itemBundleGroup.OrderBy(itemBundle => itemBundle.Amount).First())
                .Where(itemBundle => itemBundle.Amount > 0);

            return new Location(locationToRemoveItemsFrom.Id, locationToRemoveItemsFrom.Name, updatedItemBundles);
        }
    }
}
