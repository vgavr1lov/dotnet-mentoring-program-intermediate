using System.Linq.Expressions;
using System.Text;

namespace SqlQueryProviderLib;

public class ExpressionToSqlTranslator : ExpressionVisitor
{
   readonly StringBuilder _queryBuilder;
   public ExpressionToSqlTranslator()
   {
      _queryBuilder = new StringBuilder();
   }

   public string Translate(Expression exp)
   {
      Visit(exp);

      return _queryBuilder.ToString();
   }

   protected override Expression VisitMethodCall(MethodCallExpression node)
   {
      if (node.Method.DeclaringType == typeof(Queryable)
          && node.Method.Name == "Where")
      {

         if (_queryBuilder.Length == 0)
            _queryBuilder.Append("SELECT * FROM ");
         var entity = Visit(node.Arguments[0]);
         _queryBuilder.Append(" WHERE ");
         var predicate = node.Arguments[1];
         Visit(predicate);

         return node;
      }

      return base.VisitMethodCall(node);
   }

   protected override Expression VisitBinary(BinaryExpression node)
   {
      switch (node.NodeType)
      {
         case ExpressionType.Equal:
            ProcessBinaryExpression(node, " = ");
            break;
         case ExpressionType.GreaterThan:
            ProcessBinaryExpression(node, " > ");
            break;
         case ExpressionType.LessThan:
            ProcessBinaryExpression(node, " < ");
            break;
         default:
         case ExpressionType.AndAlso:
            var leftExpression = node.Left;
            Visit(leftExpression);
            _queryBuilder.Append(" AND ");
            var rightExpression = node.Right;
            Visit(rightExpression);
            break;

            throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
      };

      return node;
   }
   protected override Expression VisitMember(MemberExpression node)
   {
      _queryBuilder.Append(node.Member.Name.ToUpper());
      return node;
   }

   protected override Expression VisitConstant(ConstantExpression node)
   {
      if (node.Type.IsGenericType && node.Type.GetGenericTypeDefinition() == typeof(SqlQuery<>))
      {
         var tableName = node.Type.GetGenericArguments()[0].Name.ToUpper();
         _queryBuilder.Append($"[{tableName}]");
      }
      else
      {
         var value = GetConstantValue(node);
         _queryBuilder.Append(value);
      }

      return base.VisitConstant(node);
   }

   private void ProcessBinaryExpression(BinaryExpression node, string expressionOperator)
   {
      var memberAccessNode = FindBinaryExpression(node, ExpressionType.MemberAccess);
      var constantNode = FindBinaryExpression(node, ExpressionType.Constant);

      if (memberAccessNode == null || constantNode == null)
         throw new NotSupportedException("Expression doesn't contain property/field or constant");

      BuildExpressionOutput(memberAccessNode, constantNode, expressionOperator);
   }

   private void BuildExpressionOutput(Expression memberAccessNode, Expression constantNode, string expressionOperator, string prefixWildcard = "", string suffixWildcard = "")
   {
      Visit(memberAccessNode);
      _queryBuilder.Append(expressionOperator);
      _queryBuilder.Append(prefixWildcard);
      Visit(constantNode);
      _queryBuilder.Append(suffixWildcard);
   }

   private Expression? FindBinaryExpression(BinaryExpression node, ExpressionType expressionType)
   {
      if (node.Left.NodeType == expressionType)
         return node.Left;

      if (node.Right.NodeType == expressionType)
         return node.Right;

      return null;
   }

   private string? GetConstantValue(ConstantExpression constant)
   {
      if (constant.Value == null)
         return "NULL";

      if (constant.Type == typeof(string))
         return $"'{constant.Value}'";

      if (constant.Type == typeof(bool))
         return (bool)constant.Value ? "1" : "0";

      return constant.Value.ToString();
   }
}