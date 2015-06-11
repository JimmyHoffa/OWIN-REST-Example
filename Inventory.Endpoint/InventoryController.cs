using Inventory.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Inventory.Endpoint
{
    public class InventoryController : ApiController
    {
        private static Action<string> _simpleMessagePublisher = x => { }; // Sensible default

        private static Func<IInventoryService> _inventoryServiceConstructor;
        private IInventoryService _inventoryService;

        public static void SetIInventoryServiceConstructor(Func<IInventoryService> inventoryServiceConstructor)
        {
            _inventoryServiceConstructor = inventoryServiceConstructor;
        }

        public static void SetSimpleMessagePublisher(Action<string> simpleMessagePublisher)
        {
            _simpleMessagePublisher = simpleMessagePublisher;
        }

        public InventoryController()
        {
            _inventoryService = _inventoryServiceConstructor();
        }

        [HttpGet]
        [Route("locations/{locationIdString}")]
        public IHttpActionResult GetLocationById(string locationIdString)
        {
            try
            {
                _simpleMessagePublisher(string.Format("Request for location {0}", locationIdString));

                var locationId = Guid.Parse(locationIdString);
                var location = _inventoryService.GetLocation(locationId);

                if (location == null)
                    return NotFound();

                return Ok(new Location(location.Id, location.Name, location.ItemBundles.ToArray())); // Force enumerable iteration here at the bounds of the process to avoid passing out a deferred execution object
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("locations")]
        public IHttpActionResult CreateLocation(Location locationToCreate)
        {
            try
            {
                _simpleMessagePublisher(string.Format("Creating location {0}", locationToCreate.Name));
                return Ok(_inventoryService.CreateLocation(locationToCreate));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("locations/{locationIdString}/items/{itemIdString}")]
        public IHttpActionResult GetItemFromLocationById(string locationIdString, string itemIdString)
        {
            try
            {
                _simpleMessagePublisher(string.Format("Request for item {0} in location {1}", itemIdString, locationIdString));

                var locationId = Guid.Parse(locationIdString);
                var location = _inventoryService.GetLocation(locationId);

                if (location == null)
                    return NotFound();

                var itemId = Guid.Parse(itemIdString);
                var itemBundleInQueriedLocation = location.ItemBundles.FirstOrDefault(itemBundle => itemBundle.ItemInBundle.Id == itemId);

                if (itemBundleInQueriedLocation == null)
                    return NotFound();

                return Ok(itemBundleInQueriedLocation);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("locations/{locationIdString}/items")]
        public IHttpActionResult AddItemsToLocation(string locationIdString, ItemBundle[] itemsToAdd)
        {
            try
            {
                _simpleMessagePublisher(string.Format("Adding items [{0}] to location {1}", string.Join(",", itemsToAdd.Select(itemBundle => itemBundle.Amount.ToString() + " " + itemBundle.ItemInBundle.Name)), locationIdString));

                var locationId = Guid.Parse(locationIdString);
                var location = _inventoryService.GetLocation(locationId);

                if (location == null)
                    return NotFound();

                throw new NotImplementedException();

                return Ok(location);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("items")]
        public IHttpActionResult CreateItem(Item itemToCreate)
        {
            try
            {
                _simpleMessagePublisher(string.Format("Creating item {0}", itemToCreate.Name));
                return Ok(_inventoryService.CreateItem(itemToCreate));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("items/{itemIdString}")]
        public IHttpActionResult GetItemById(string itemIdString)
        {
            try
            {
                _simpleMessagePublisher(string.Format("Request for item {0}", itemIdString));

                var itemId = Guid.Parse(itemIdString);
                var item = _inventoryService.GetItem(itemId);
                if (item == null)
                    return NotFound();

                return Ok(item);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
