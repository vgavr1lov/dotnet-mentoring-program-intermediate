using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
   public class IncDecExpressionVisitor : ExpressionVisitor
   {
      Dictionary<string, int> ParameterValuePairs = new Dictionary<string, int>();
      public Expression Modify(Expression expression)
      {
         return Visit(expression);
      }

      public Expression TranslateToConstant(Expression expression, Dictionary<string, int> parameterValuePairs)
      {
         ParameterValuePairs = parameterValuePairs;

         if (expression is LambdaExpression lambdaExpression)
         {
            var remainingParameters = lambdaExpression.Parameters
               .Where(x => !ParameterValuePairs.ContainsKey(x.Name))
               .ToList();

            var body = Visit(lambdaExpression.Body);

            var parameterTypes = remainingParameters
             .Select(p => p.Type)
             .Append(body.Type)
             .ToArray();

            var delegateType = Expression.GetFuncType(parameterTypes);

            return Expression.Lambda(delegateType, body, remainingParameters);
         }

         return Visit(expression);
      }

      protected override Expression VisitParameter(ParameterExpression node)
      {
         if (ParameterValuePairs.TryGetValue(node.Name, out var value))
            return Expression.Constant(value);

         return base.VisitParameter(node);
      }

      protected override Expression VisitBinary(BinaryExpression node)
      {
         if (node.NodeType == ExpressionType.Add)
         {
            if (IsIncrement(node, out var parameter))
               return Expression.PostIncrementAssign(parameter);
         }

         if (node.NodeType == ExpressionType.Subtract)
         {
            if (IsIncrement(node, out var parameter))
               return Expression.PostDecrementAssign(parameter);
         }

         return base.VisitBinary(node);
      }

      private bool IsIncrement(BinaryExpression node, out Expression parameter)
      {
         parameter = FindBinaryExpressionByType(node, ExpressionType.Parameter);

         if (parameter == null)
            return false;

         var constant = FindBinaryExpressionByType(node, ExpressionType.Constant);

         if (constant is ConstantExpression constantExpression)
         {
            if (constantExpression.Value is int value && value == 1)
               return true;
         }

         return false;
      }

      private Expression FindBinaryExpressionByType(BinaryExpression node, ExpressionType expressionType)
      {
         if (node.Left.NodeType == expressionType)
            return node.Left;

         if (node.Right.NodeType == expressionType)
            return node.Right;

         return null;
      }

   }
}