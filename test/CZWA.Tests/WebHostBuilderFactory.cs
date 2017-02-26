using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CZWA.DB;
using CZWA.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CZWA.Tests
{
    public static class WebHostBuilderFactory
    {
        public static IWebHostBuilder Create()
        {
            return Create(Enumerable.Empty<Action<IServiceCollection>>());
        }

        public static IWebHostBuilder Create(IEnumerable<Action<IServiceCollection>> configureServices)
        {
            var configureApplication = Enumerable.Empty<Action<IApplicationBuilder>>();
            return Create(configureServices, configureApplication);
        }

        public static IWebHostBuilder Create(IEnumerable<Action<IApplicationBuilder>> configureApplication)
        {
            var configureServices = Enumerable.Empty<Action<IServiceCollection>>();
            return Create(configureServices, configureApplication);
        }

        public static IWebHostBuilder Create(IEnumerable<Action<IServiceCollection>> configureServices,
          IEnumerable<Action<IApplicationBuilder>> configureApplication)
        {
            // We can't use ".UseStartup<T>" as we want to be able to affect "MyStartup.Configure(IApplicationBuilder)".
            Startup app = null;
            var contentRoot = GetContentRoot();
            var webHostBuilder = new WebHostBuilder()
              .UseContentRoot(contentRoot.FullName)
              .UseEnvironment(EnvironmentName.Development)
              .ConfigureServices(services =>
              {
                  var hostingEnvironment = GetHostingEnvironment(services);
                  app = new Startup(hostingEnvironment);
                  ConfigureServices(app, services, configureServices);
              })
              .Configure(builder =>
              {
                  ConfigureApplication(app, builder, configureApplication);
              });
            return webHostBuilder;
        }

        private static DirectoryInfo GetContentRoot()
        {
            // Change to match your directory layout.
            const string relativeContentRootPath = @"..\..\src\CZWA.Web";
            var contentRoot = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), relativeContentRootPath));
            if (!contentRoot.Exists)
            {
                throw new DirectoryNotFoundException($"Directory '{contentRoot.FullName}' not found.");
            }
            return contentRoot;
        }

        private static void ConfigureServices(Startup app, IServiceCollection services,
          IEnumerable<Action<IServiceCollection>> configureServices)
        {
            app.ConfigureServices(services);
            foreach (var serviceConfiguration in configureServices)
            {
                serviceConfiguration(services);
            }
        }

        private static IHostingEnvironment GetHostingEnvironment(IServiceCollection services)
        {
            Func<ServiceDescriptor, bool> isHostingEnvironmet = service => service.ImplementationInstance is IHostingEnvironment;
            var hostingEnvironment = (IHostingEnvironment)services.Single(isHostingEnvironmet).ImplementationInstance;
            var assembly = typeof(Startup).GetTypeInfo().Assembly;
            // This can be skipped if you keep your tests and production code in the same assembly.
            hostingEnvironment.ApplicationName = assembly.GetName().Name;
            return hostingEnvironment;
        }

        private static void ConfigureApplication(Startup app, IApplicationBuilder builder,
          IEnumerable<Action<IApplicationBuilder>> configureApplication)
        {
            var optionsbuilder = new DbContextOptionsBuilder<DataContext>();
            optionsbuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Core-Test-V1;Trusted_Connection=True;MultipleActiveResultSets=true");
            var dataContext = new DataContext(optionsbuilder.Options, null);

            foreach (var applicationConfiguration in configureApplication)
            {
                applicationConfiguration(builder);
            }
            app.Configure(builder, dataContext, builder.ApplicationServices);
        }
    }
}
