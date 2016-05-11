using Inventory.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Composer
{
    public class InventoryComposer : IInventoryComposer
    {
        private IItemStore _itemStore;
        private ILocationStore _locationStore;
        private IInventoryService _inventoryService;

        public InventoryComposer(IItemStore itemStore, ILocationStore locationStore, IInventoryService inventoryService)
        {
            _itemStore = itemStore;
            _locationStore = locationStore;
            _inventoryService = inventoryService;
        }

        public Location GetLocationById(Guid locationId)
        {
            return _locationStore.GetLocationById(locationId);
        }

        public Location CreateLocation(Location locationToCreate)
        {
            return StoreLocation(_inventoryService.CreateLocation(locationToCreate));
        }
        
        public Location StoreLocation(Location locationToStore)
        {
            foreach (var possiblyNewItem in locationToStore.ItemBundles.Select(bundle => bundle.ItemInBundle))
                _itemStore.UpsertItem(possiblyNewItem);

            return _locationStore.UpsertLocation(locationToStore);
        }
        
        public ItemBundle GetItemBundleFromLocationById(Guid locationId, Guid itemId)
        {
            var location = _locationStore.GetLocationById(locationId);
            if (location == null)
                return null;

            return location.ItemBundles.FirstOrDefault(itemBundle => itemBundle.ItemInBundle.Id == itemId);
        }
        
        public Location AddItemsToLocation(Guid locationId, params ItemBundle[] itemsToAdd)
        {
            var locationToAddItemsTo = _locationStore.GetLocationById(locationId);
            var locationWithAdditions = _inventoryService.AddItemsToLocation(locationToAddItemsTo, itemsToAdd);
            return StoreLocation(locationWithAdditions);
        }
        public Item CreateItem(Item itemToCreate)
        {
            return StoreItem(_inventoryService.CreateItem(itemToCreate));
        }


        public Item StoreItem(Item itemToStore)
        {
            return _itemStore.UpsertItem(itemToStore);
        }

        public Item GetItemById(Guid itemId)
        {
            return _itemStore.GetItemById(itemId);
        }
    }
}
