using Inventory.Endpoint;
using Inventory.Persistence.InMemory;
using Inventory.Service;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;

namespace Inventory.ConsoleHost
{
    [assembly: OwinStartup(typeof(Program))]
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                string baseAddress = ConfigurationManager.AppSettings["BaseAddress"] ?? "http://*:333"; // for test purposes, a reasonable default.

                using (WebApp.Start<Program>(url: baseAddress))
                {
                    Console.WriteLine(string.Format("Server running at: {0}", baseAddress));
                    Console.WriteLine("----------------------");
                    Console.WriteLine("Press enter to quit.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred:");
                Console.WriteLine(ex);
                Console.WriteLine("-------Press enter to quit-------");
                Console.ReadLine();
            }
        }

        public void Configuration(IAppBuilder app)
        {
            // Typically I'd use constructor injection for this, but in a rush and not interested in hacking any containers into the MVC DependencyResolver
            // This still pulls the dependencies up to the top layer effectively committing IoC, it's just controllers are constructed by the guts of the MVC and I'm not interested in hooking into the dep resolve right now
            var itemStore = new InMemoryItemStore();
            var locationStore = new InMemoryLocationStore();
            InventoryService.SetIItemStoreConstructor(() => itemStore); // enclose this single instance because it needs to be maintained consistently since it's an in memory store
            InventoryService.SetILocationStoreConstructor(() => locationStore);

            InventoryController.SetIInventoryServiceConstructor(() => new InventoryService()); // it's dependencies are static already, it does not need to be static
            InventoryController.SetSimpleMessagePublisher(Console.WriteLine);
            
            var httpConfiguration = new HttpConfiguration();

            // Configure Web API Routes:
            // - Enable Attribute Mapping
            httpConfiguration.MapHttpAttributeRoutes();

            // - Enable Default routes at /api.
            //httpConfiguration.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "sdl/{controller}/{resource}/{action}",
            //    defaults: new { resource = "sessions", action = "get" }
            //);

            app.UseStaticFiles();
            app.UseWebApi(httpConfiguration);
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
