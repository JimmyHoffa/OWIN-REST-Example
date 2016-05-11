using Infrastructure.MVC;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;

[assembly: OwinStartup(typeof(Inventory.ConsoleHost.Program))]
namespace Inventory.ConsoleHost
{
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

            app.UseStaticFiles();
            app.UseWebApi(httpConfiguration);
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
