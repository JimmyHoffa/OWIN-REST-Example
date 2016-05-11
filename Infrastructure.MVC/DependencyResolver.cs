using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace Infrastructure.MVC
{
    public class DependencyResolver : IDependencyResolver, IDependencyScope
    {
        private List<IDisposable> _disposableDependencies = new List<IDisposable>();
        private DependencyContainer _containerForDependenciesToResolve;

        public DependencyResolver(DependencyContainer containerForDependenciesToResolve)
        {
            _containerForDependenciesToResolve = containerForDependenciesToResolve;
        }

        public IDependencyScope BeginScope()
        {
            return new DependencyResolver(_containerForDependenciesToResolve);
        }

        public object GetService(Type serviceType)
        {
            object dependency;
            try
            {
                dependency = _containerForDependenciesToResolve.GetDependency(serviceType);
            }
            catch
            {
                // MVC uses this method as a *try* for it's framework components, and has defaults outside of this it uses if we haven't registered them here
                // Treat these dependencies in a special manner - return null and the calling framework will resolve them for us.
                if (serviceType.FullName.StartsWith("System")) // If this turns out to be too inclusive, the dependencies are all System.Web.Http or System.Net.Http, but "System" will be more performant
                    return null;

                throw;
            }

            if (serviceType.GetInterfaces().Contains(typeof(IDisposable)))
                _disposableDependencies.Add((IDisposable)dependency);

            return dependency;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new[] { GetService(serviceType) };
        }

        public void Dispose()
        {
            foreach (var disposable in _disposableDependencies)
                try { disposable.Dispose(); }
                catch { } // TODO: Log disposal failures
        }
    }
}
