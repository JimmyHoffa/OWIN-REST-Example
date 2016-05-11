using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{    public class DependencyContainer
    {
        private static class Dependency<T>
        {
            public static ConcurrentDictionary<Guid, Func<T>> DependencyContainerIdToConstructor { get; set; }

            static Dependency()
            {
                DependencyContainerIdToConstructor = new ConcurrentDictionary<Guid, Func<T>>();
            }
        }

        private Guid _dependencyContainerId = Guid.NewGuid();
        private ConcurrentDictionary<Type, Func<object>> _fallBackDictionaryWhenTypeIsntCompileTimeKnown = new ConcurrentDictionary<Type, Func<object>>();

        public DependencyContainer SetDependencyCreator(Type typeOfDependencyThisCreates, Func<object> dependencyCreator)
        {
            _fallBackDictionaryWhenTypeIsntCompileTimeKnown[typeOfDependencyThisCreates] = dependencyCreator;
            return this;
        }

        public DependencyContainer SetDependencyCreator<T>(Func<T> dependencyCreator)
        {
            Dependency<T>.DependencyContainerIdToConstructor[_dependencyContainerId] = dependencyCreator;
            return SetDependencyCreator(typeof(T), () => dependencyCreator()); // needed to wrap the closure to push the type to return 'object' instead of T for the dictionary
        }

        public DependencyContainer SetSingleDependencyInstance<T>(T dependencyInstance)
        {
            return SetDependencyCreator(() => dependencyInstance);
        }

        public T GetDependency<T>()
        {
            Func<T> dependencyCreator;
            if (Dependency<T>.DependencyContainerIdToConstructor.TryGetValue(_dependencyContainerId, out dependencyCreator))
                return dependencyCreator();

            // This probably wasn't registered using generic <T> because we only knew the type at runtime so compile time didn't have a T
            return (T)GetDependency(typeof(T));
        }

        public object GetDependency(Type typeOfDependencyToGet)
        {
            if (!_fallBackDictionaryWhenTypeIsntCompileTimeKnown.ContainsKey(typeOfDependencyToGet)) // Ack we have no registration for this type by T or Type
                throw new KeyNotFoundException(string.Format("Could not find a registration in the DependencyContainer {0} for type {1}", _dependencyContainerId, typeOfDependencyToGet.FullName));

            return _fallBackDictionaryWhenTypeIsntCompileTimeKnown[typeOfDependencyToGet]();
        }

        public void SetDependenciesFromContainer(DependencyContainer containerToAdd)
        {
            foreach (var ctor in containerToAdd._fallBackDictionaryWhenTypeIsntCompileTimeKnown)
                _fallBackDictionaryWhenTypeIsntCompileTimeKnown[ctor.Key] = ctor.Value;
        }
    }
}
