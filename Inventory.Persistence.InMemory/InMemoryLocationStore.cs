using Inventory.Contract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Persistence.InMemory
{
    public class InMemoryLocationStore : ILocationStore
    {
        private ConcurrentDictionary<Guid, Location> _locations = new ConcurrentDictionary<Guid, Location>();

        public Location GetLocationById(Guid locationId)
        {
            Location result;
            if (_locations.TryGetValue(locationId, out result))
                return result;

            return null;
        }

        public Location UpsertLocation(Location latestOrNewLocation)
        {
            _locations.TryAdd(latestOrNewLocation.Id, new Location(latestOrNewLocation.Id, latestOrNewLocation.Name, latestOrNewLocation.ItemBundles.ToArray())); // Force enumerable iteration here at the bounds of the process to avoid passing out a deferred execution object
            return GetLocationById(latestOrNewLocation.Id); // return the latest in case modification or deletion happened between add and here
        }

        public Location DeleteLocation(Guid locationId)
        {
            Location deletedLocation = null;
            _locations.TryRemove(locationId, out deletedLocation);
            return deletedLocation;
        }

        IEnumerable<Location> ILocationStore.GetLocations()
        {
            return _locations.Values;
        }
    }
}
