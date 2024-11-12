using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTrees.Task2.ExpressionMapping
{
   public class PropertyMapping
   {
      public PropertyInfo SourceProperty { get; set; }
      public PropertyInfo TargetProperty { get; set; }

      private static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> expression)
      {
         if (expression.Body is MemberExpression memberExpression)
            return memberExpression.Member as PropertyInfo;


         if (expression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression operand)
            return operand.Member as PropertyInfo;

         return null;
      }

      public static List<PropertyMapping> CreateMappings<TSource, TTarget>(params (Expression<Func<TSource, object>> source, Expression<Func<TTarget, object>> target)[] mappings)
      {
         return mappings
            .Select(mapping => new PropertyMapping
            {
               SourceProperty = GetPropertyInfo(mapping.source),
               TargetProperty = GetPropertyInfo(mapping.target)
            })
            .ToList();
      }
   }
}
