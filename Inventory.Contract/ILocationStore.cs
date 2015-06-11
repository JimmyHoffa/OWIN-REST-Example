using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Contract
{
    public interface ILocationStore
    {
        IEnumerable<Location> GetLocations();
        Location GetLocationById(Guid locationId);
        Location UpsertLocation(Location latestOrNewLocation);
        Location DeleteLocation(Guid locationId);
    }
}
