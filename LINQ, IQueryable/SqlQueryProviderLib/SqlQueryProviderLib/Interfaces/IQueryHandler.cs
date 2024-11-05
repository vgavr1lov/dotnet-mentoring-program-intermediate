namespace SqlQueryProviderLib;

public interface IQueryHandler
{
   object? ExecuteQuery(Type itemType, string queryString);
}