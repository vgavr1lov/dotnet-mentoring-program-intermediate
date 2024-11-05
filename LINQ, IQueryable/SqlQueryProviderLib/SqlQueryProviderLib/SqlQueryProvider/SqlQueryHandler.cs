using Microsoft.Extensions.Options;
using System.Collections;
using System.Data.SqlClient;

namespace SqlQueryProviderLib;

public class SqlQueryHandler : IQueryHandler
{
   readonly string _connectionString;
   public SqlQueryHandler(IOptions<SqlQueryProviderOptions> options)
   {
      _connectionString = options.Value.ConnectionString;
   }

   public object? ExecuteQuery(Type itemType, string queryString)
   {
      var results = Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType)) as IList;

      using (var connection = new SqlConnection(_connectionString))
      {
         connection.Open();
         using (var command = connection.CreateCommand())
         {
            command.CommandText = queryString;

            using (var reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                  var instance = Activator.CreateInstance(itemType);

                  foreach (var property in itemType.GetProperties())
                  {
                     var convertedValue = TypeConverter.ConvertValueToTargetType(reader[property.Name], property.PropertyType);
                     property.SetValue(instance, convertedValue);
                  }

                  results?.Add(instance);
               }
            }
         }
      }

      return results;
   }
}
