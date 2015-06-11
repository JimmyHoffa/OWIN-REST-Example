using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Contract
{
    public class Item : IEqualityComparer<Item>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string ExpectedLocationName { get; private set; }

        public Item(Guid id, string name, string expectedLocationName)
        {
            Id = id;
            Name = name;
            ExpectedLocationName = expectedLocationName;
        }

        public bool Equals(Item x, Item y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Item obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
