using EnterpriseChatLibrary;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EnterpriseChatServer;

internal class Program
{
    static void Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
             .ConfigureServices(ConfigureServices)
             .Build();

        using (var scope = host.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<ServerUI>();
            service.Run();
        }
    }

    static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddScoped<IOutputWriter, ConsoleOutputWriter>();
        services.AddSingleton<ServerUI>();
    }

}

