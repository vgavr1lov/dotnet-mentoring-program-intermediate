using EnterpriseChatLibrary;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EnterpriseChatClient;

internal class Program
{
    static void Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
             .ConfigureServices(ConfigureServices)
             .Build();

        using (var scope = host.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<ClientUI>();
            service.Run();
        }
    }

    static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddScoped<IOutputWriter, ConsoleOutputWriter>();
        services.AddScoped<IInputReader, ConsoleInputReader>();
        services.AddSingleton<ClientUI>();
    }
}

