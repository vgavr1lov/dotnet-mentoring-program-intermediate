using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlQueryProviderLib;

namespace SqlQueryProviderConsoleApp;

public class Program
{
   static void Main(string[] args)
   {
      using IHost host = Host.CreateDefaultBuilder(args)
           .ConfigureAppConfiguration((context, config) =>
           {
              config.AddJsonFile("appsettings.json", false, true);
           })
           .ConfigureServices(ConfigureServices)
           .Build();

      using (var scope = host.Services.CreateScope())
      {
         var service = scope.ServiceProvider.GetRequiredService<Application>();
         service.Run();
      }
   }

   static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
   {
      var configuration = context.Configuration;
      var connectionString = configuration.GetConnectionString("Default");

      services.AddSqlQueryService(connectionString);
      services.AddSingleton<Application>();
   }
}
