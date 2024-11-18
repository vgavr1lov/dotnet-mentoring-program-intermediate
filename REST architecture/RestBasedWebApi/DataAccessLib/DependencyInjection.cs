using DataAccessLib.Interfaces;
using DataAccessLib.Models;
using DataAccessLib.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLib;

public static class DependencyInjection
{
   public static IServiceCollection AddDataAccessLib(
       this IServiceCollection services,
       string? connectionString)
   {
      services.AddDbContext<DatabaseContext>(options =>
          options.UseSqlServer(connectionString)
          .EnableSensitiveDataLogging(false));
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      return services;
   }
}
