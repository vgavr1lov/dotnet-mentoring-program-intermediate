/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
   class Program
   {
      static void Main(string[] args)
      {
         Console.WriteLine("Expression Visitor for increment/decrement.");
         Console.WriteLine();

         var visitor = new IncDecExpressionVisitor();

         Console.WriteLine("Task 1: converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.");
         Expression<Func<int, int>> incrementExpression = (x) => x + 1;
         var incrementExpressionModified = visitor.Modify(incrementExpression);
         Console.WriteLine($"Before: {incrementExpression}");
         Console.WriteLine($"After: {incrementExpressionModified}");

         Expression<Func<int, int>> decrementExpression = (x) => x - 1;
         var decrementExpressionModified = visitor.Modify(decrementExpression);
         Console.WriteLine($"Before: {decrementExpression}");
         Console.WriteLine($"After: {decrementExpressionModified}");
         Console.WriteLine();

         Console.WriteLine("Task 2: changes parameter values in a lambda expression to constants");
         var parameterValuePairs = new Dictionary<string, int>
         {
           {"x", 1},
           {"y", 2}
         };

         Expression<Func<int, int>> lambdaExpressionWithOneParameter = (x) => x;
         var lambdaExpressionWithOneParameterModified = visitor.TranslateToConstant(lambdaExpressionWithOneParameter, parameterValuePairs);
         Console.WriteLine($"Before: {lambdaExpressionWithOneParameter}");
         Console.WriteLine($"After: {lambdaExpressionWithOneParameterModified}");

         Expression<Func<int, int, int>> lambdaExpressionWithTwoParameter = (x, y) => x + y;
         var lambdaExpressionWithTwoParameterModified = visitor.TranslateToConstant(lambdaExpressionWithTwoParameter, parameterValuePairs);
         Console.WriteLine($"Before: {lambdaExpressionWithTwoParameter}");
         Console.WriteLine($"After: {lambdaExpressionWithTwoParameterModified}");

         Expression<Func<int, int, int, int>> lambdaExpressionWithThreeParameter = (x, y, z) => x + y + z;
         var lambdaExpressionWithThreeParameterModified = visitor.TranslateToConstant(lambdaExpressionWithThreeParameter, parameterValuePairs);
         Console.WriteLine($"Before: {lambdaExpressionWithThreeParameter}");
         Console.WriteLine($"After: {lambdaExpressionWithThreeParameterModified}");

         Console.ReadLine();
      }
   }
}
