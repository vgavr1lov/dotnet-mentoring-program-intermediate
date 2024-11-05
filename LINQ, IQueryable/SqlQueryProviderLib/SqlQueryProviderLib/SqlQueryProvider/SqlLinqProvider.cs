using System.Linq.Expressions;

namespace SqlQueryProviderLib;

public class SqlLinqProvider : IQueryProvider
{
   private readonly IQueryHandler _sqlQueryHandler;
   public SqlLinqProvider(IQueryHandler queryHandler)
    {
      _sqlQueryHandler = queryHandler;
    }
    public IQueryable CreateQuery(Expression expression)
   {
      throw new NotImplementedException();
   }

   public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
   {
      return new SqlQuery<TElement>(expression, this);
   }

   public object? Execute(Expression expression)
   {
      throw new NotImplementedException();
   }

   public TResult Execute<TResult>(Expression expression)
   {
      Type itemType = TypeHelper.GetElementType(expression.Type);
      var translator = new ExpressionToSqlTranslator();
      var queryStringList = translator.Translate(expression);

      return (TResult)_sqlQueryHandler.ExecuteQuery(itemType, queryStringList); 
   }
}
