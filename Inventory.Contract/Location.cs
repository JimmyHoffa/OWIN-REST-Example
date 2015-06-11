using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Contract
{
    public class Location
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<ItemBundle> ItemBundles { get; private set; }

        public Location(Guid id, string name, IEnumerable<ItemBundle> itemBundles)
        {
            Id = id;
            Name = name;
            ItemBundles = itemBundles;
        }
    }
}
