using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;

namespace ExpressionTrees.Task2.ExpressionMapping
{
   public class MappingGenerator
   {
      public Mapper<TSource, TDestination> Generate<TSource, TDestination>(List<PropertyMapping> mappings)
      {
         var sourceParam = Expression.Parameter(typeof(TSource));
         var destinationParam = Expression.Parameter(typeof(TDestination));

         var bindings = new List<MemberBinding>();

         foreach (var propertyMapping in mappings)
         {
            var sourceProperty = propertyMapping.SourceProperty;
            var targetProperty = propertyMapping.TargetProperty;

            var sourceAccessor = Expression.PropertyOrField(sourceParam, sourceProperty.Name);

            var binding = CreateMemberBinding(sourceProperty.PropertyType, targetProperty.PropertyType, targetProperty, sourceAccessor);

            bindings.Add(binding);
         }

         var newExpression = Expression.New(typeof(TDestination));
         var body = Expression.MemberInit(newExpression, bindings);

         var mapFunction =
                Expression.Lambda<Func<TSource, TDestination>>(
                    body,
                    sourceParam
                );

         return new Mapper<TSource, TDestination>(mapFunction.Compile());
      }

      private MemberBinding CreateMemberBinding(Type sourcePropertyType, Type targetPropertyType, MemberInfo targetProperty, MemberExpression sourceAccessor)
      {
         if (sourcePropertyType == targetPropertyType)
            return Expression.Bind(targetProperty, sourceAccessor);

         if (targetPropertyType == typeof(string))
         {
            var toStringCall = Expression.Call(sourceAccessor, "ToString", null);
            return Expression.Bind(targetProperty, toStringCall);
         }

         return null;
      }
   }
}
