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
        private static Func<ILocationStore> _locationStoreConstructor;
        private static Func<IItemStore> _itemStoreConstructor;

        private ILocationStore _locationStore;
        private IItemStore _itemStore;

        public static void SetILocationStoreConstructor(Func<ILocationStore> locationStoreConstructor)
        {
            _locationStoreConstructor = locationStoreConstructor;
        }

        public static void SetIItemStoreConstructor(Func<IItemStore> itemStoreConstructor)
        {
            _itemStoreConstructor = itemStoreConstructor;
        }

        public InventoryService()
        {
            _locationStore = _locationStoreConstructor();
            _itemStore = _itemStoreConstructor();
        }

        public IEnumerable<Location> GetAllLocations()
        {
            return _locationStore.GetLocations();
        }

        public IEnumerable<Item> GetAllItems()
        {
            return _itemStore.GetItems();
        }

        public Location AddItemToLocation(Location locationToAddItemTo, Item itemToAdd)
        {
            return _locationStore.UpsertLocation(locationToAddItemTo.AddItems(itemToAdd));
        }

        public Location RemoveItemFromLocation(Location locationToRemoveItemFrom, Item itemToRemove)
        {
            return _locationStore.UpsertLocation(locationToRemoveItemFrom.AddItems(itemToRemove));
        }

        public Location GetLocation(Guid locationId)
        {
            return _locationStore.GetLocationById(locationId);
        }

        public Item GetItem(Guid itemId)
        {
            return _itemStore.GetItemById(itemId);
        }

        public Location CreateLocation(Location locationToAdd)
        {
            return _locationStore.UpsertLocation(new Location(Guid.NewGuid(), locationToAdd.Name, locationToAdd.ItemBundles));
        }

        public Location DeleteLocation(Guid locationId)
        {
            return _locationStore.DeleteLocation(locationId);
        }

        public Item CreateItem(Item itemToAdd)
        {
            return _itemStore.UpsertItem(new Item(Guid.NewGuid(), itemToAdd.Name, itemToAdd.ExpectedLocationName));
        }

        public Item DeleteItem(Guid itemId)
        {
            return _itemStore.DeleteItem(itemId);
        }
    }
}
