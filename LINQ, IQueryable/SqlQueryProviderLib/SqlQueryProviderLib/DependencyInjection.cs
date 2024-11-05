using Microsoft.Extensions.DependencyInjection;

namespace SqlQueryProviderLib;
public static class DependencyInjection
{
   public static IServiceCollection AddSqlQueryService(this IServiceCollection services, string connectionString)
   {
      services.Configure<SqlQueryProviderOptions>(options =>
      {
         options.ConnectionString = connectionString;
      });
      services.AddScoped(typeof(IQueryable<>), typeof(SqlQuery<>));
      services.AddScoped<IQueryHandler, SqlQueryHandler>();
      services.AddScoped<IQueryProvider, SqlLinqProvider>();
      return services;
   }
}
