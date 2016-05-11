using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Swashbuckle.Application;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;
using Infrastructure.MVC;

//[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

[assembly: OwinStartup(typeof(Inventory.IISHost.Startup))]
namespace Inventory.IISHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();
            var dependencyContainer = new InventoryApplication().GetApplicationDependencyGraph();
            dependencyContainer.SetSingleDependencyInstance<Action<string>>(Console.WriteLine);
            httpConfiguration.DependencyResolver = new DependencyResolver(dependencyContainer);
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
