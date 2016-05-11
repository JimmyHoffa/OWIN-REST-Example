using Infrastructure;
using Inventory.Composer;
using Inventory.Contract;
using Inventory.Endpoint;
using Inventory.Persistence.InMemory;
using Inventory.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory
{
    public class InventoryApplication
    {
        public DependencyContainer GetApplicationDependencyGraph()
        {
            var dependencyContainer = new DependencyContainer();

            dependencyContainer.SetSingleDependencyInstance<IItemStore>(new InMemoryItemStore());
            dependencyContainer.SetSingleDependencyInstance<ILocationStore>(new InMemoryLocationStore());
            dependencyContainer.SetDependencyCreator<IInventoryService>(() => new InventoryService());

            dependencyContainer.SetDependencyCreator<IInventoryComposer>(() => new InventoryComposer(
                dependencyContainer.GetDependency<IItemStore>(),
                dependencyContainer.GetDependency<ILocationStore>(),
                dependencyContainer.GetDependency<IInventoryService>()));

            dependencyContainer.SetDependencyCreator<InventoryController>(() => new InventoryController(
                dependencyContainer.GetDependency<Action<string>>(),
                dependencyContainer.GetDependency<IInventoryComposer>()));

            return dependencyContainer;
        }
    }
}
