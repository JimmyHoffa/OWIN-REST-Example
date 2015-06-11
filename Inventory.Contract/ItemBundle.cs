using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Contract
{
    public class ItemBundle
    {
        public int Amount { get; private set; }
        public Item ItemInBundle { get; private set; }

        public ItemBundle(int amount, Item itemInBundle)
        {
            Amount = amount;
            ItemInBundle = itemInBundle;
        }
    }
}
