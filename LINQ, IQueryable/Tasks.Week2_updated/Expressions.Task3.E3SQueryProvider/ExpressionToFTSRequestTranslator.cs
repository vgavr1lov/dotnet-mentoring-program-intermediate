using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
   public class ExpressionToFtsRequestTranslator : ExpressionVisitor
   {
      readonly StringBuilder _resultStringBuilder;
      readonly List<string> _resultStringList = new List<string>();

      public ExpressionToFtsRequestTranslator()
      {
         _resultStringBuilder = new StringBuilder();
      }

      //public string Translate(Expression exp)
      //{
      //   Visit(exp);

      //   return _resultStringBuilder.ToString();
      //}

      public List<string> Translate(Expression exp)
      {
         Visit(exp);

         return _resultStringList;
      }

      #region protected methods

      protected override Expression VisitMethodCall(MethodCallExpression node)
      {
         if (node.Method.DeclaringType == typeof(Queryable)
             && node.Method.Name == "Where")
         {
            var predicate = node.Arguments[1];
            Visit(predicate);

            return node;
         }

         if (node.Method.DeclaringType == typeof(string))
         {
            switch (node.Method.Name)
            {
               case "Equals":
                  return ProcessStringMethod(node);
               case "StartsWith":
                  return ProcessStringMethod(node, "", "*");
               case "EndsWith":
                  return ProcessStringMethod(node, "*");
               case "Contains":
                  return ProcessStringMethod(node, "*", "*");
            }
         }

         return base.VisitMethodCall(node);
      }

      protected override Expression VisitBinary(BinaryExpression node)
      {
         switch (node.NodeType)
         {
            case ExpressionType.Equal:
               //if (node.Left.NodeType != ExpressionType.MemberAccess)
               //   throw new NotSupportedException($"Left operand should be property or field: {node.NodeType}");

               //if (node.Right.NodeType != ExpressionType.Constant)
               //   throw new NotSupportedException($"Right operand should be constant: {node.NodeType}");

               //Visit(node.Left);
               //_resultStringBuilder.Append("(");
               //Visit(node.Right);
               //_resultStringBuilder.Append(")");

               var memberAccessNode = FindBinaryExpression(node, ExpressionType.MemberAccess);
               var constantNode = FindBinaryExpression(node, ExpressionType.Constant);

               if (memberAccessNode == null || constantNode == null)
                  throw new NotSupportedException("Expression doesn't contain property/field or constant");

               BuildEqualPartialExpressionOutput(memberAccessNode, constantNode);

               break;
            case ExpressionType.AndAlso:
               var leftExpression = node.Left;
               Visit(leftExpression);
               var rightExpression = node.Right;
               Visit(rightExpression);

               break;
            default:
               throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
         };

         return node;
      }

      protected override Expression VisitMember(MemberExpression node)
      {
         _resultStringBuilder.Append(node.Member.Name).Append(":");

         return base.VisitMember(node);
      }

      protected override Expression VisitConstant(ConstantExpression node)
      {
         _resultStringBuilder.Append(node.Value);

         return node;
      }

      #endregion

      #region private methods
      private Expression ProcessStringMethod(MethodCallExpression node, string prefixWildcard = "", string suffixWildcard = "")
      {
         if (node.Arguments.Count > 2)
            throw new NotSupportedException("In current implementation Equal expression can contain only one parameter");

         if ((!TryGetExpression(node.Object, ExpressionType.MemberAccess, out var memberAccessNode))
            || (!TryGetExpression(node.Arguments[0], ExpressionType.Constant, out var constantNode)))
         {
            throw new NotSupportedException("Expression doesn't contain a property/field or constant.");
         }

         BuildEqualPartialExpressionOutput(memberAccessNode, constantNode, prefixWildcard, suffixWildcard);

         return node;
      }

      private bool TryGetExpression(Expression node, ExpressionType expressionType, out Expression foundNode)
      {
         foundNode = null;
         if (node.NodeType == expressionType)
         {
            foundNode = node;
            return true;
         }

         return false;
      }

      private Expression FindBinaryExpression(BinaryExpression node, ExpressionType expressionType)
      {
         if (node.Left.NodeType == expressionType)
            return node.Left;

         if (node.Right.NodeType == expressionType)
            return node.Right;

         return null;
      }

      private void BuildEqualPartialExpressionOutput(Expression memberAccessNode, Expression constantNode, string prefixWildcard = "", string suffixWildcard = "")
      {
         Visit(memberAccessNode);
         _resultStringBuilder.Append("(");
         _resultStringBuilder.Append(prefixWildcard);
         Visit(constantNode);
         _resultStringBuilder.Append(suffixWildcard);
         _resultStringBuilder.Append(")");

         _resultStringList.Add(_resultStringBuilder.ToString());
         _resultStringBuilder.Clear();
      }

      #endregion
   }
}
