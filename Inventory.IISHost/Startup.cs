using Inventory.Endpoint;
using Inventory.Persistence.InMemory;
using Inventory.Service;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Swashbuckle.Application;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;

//[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

[assembly: OwinStartup(typeof(Inventory.IISHost.Startup))]
namespace Inventory.IISHost
{
    public class Startup
    {
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

            app.UseWebApi(httpConfiguration);
            app.UseStageMarker(PipelineStage.MapHandler);
        }

        private static void ConfigureSwagger(HttpConfiguration config)
        {
            config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Inventory Service")
                        .Description("An API for managing the inventory of items in locations.")
                        .Contact(cc => cc
                        .Name("Jimmy Hoffa")
                        .Email("Jimmy@Hoffa.isaRiddle"));
                })
                .EnableSwaggerUi(c =>
                {
                });
        }
    }
}
