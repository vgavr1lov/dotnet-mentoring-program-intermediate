using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
   [TestClass]
   public class ExpressionMappingTests
   {
      // todo: add as many test methods as you wish, but they should be enough to cover basic scenarios of the mapping generator

      [TestMethod]
      public void Map_DifferentNamedIntProperties_ValueMappedCorrectly()
      {
         var foo = GetSampleData();

         var mapGenerator = new MappingGenerator();
         var mappings = PropertyMapping.CreateMappings<Foo, Bar>(
             (foo => foo.Id, bar => bar.BarId)
         );

         var mapper = mapGenerator.Generate<Foo, Bar>(mappings);

         var res = mapper.Map(foo);

         Assert.AreEqual(foo.Id, res.BarId);
      }

      [TestMethod]
      public void Map_DifferentNamedAndTypedProperties_ValueMappedCorrectly()
      {
         var foo = GetSampleData();

         var mapGenerator = new MappingGenerator();
         var mappings = PropertyMapping.CreateMappings<Foo, Bar>(
             (foo => foo.Age, bar => bar.BarAge)
         );

         var mapper = mapGenerator.Generate<Foo, Bar>(mappings);

         var res = mapper.Map(foo);

         Assert.AreEqual(foo.Age.ToString(), res.BarAge);
      }

      [TestMethod]
      public void Map_MultipleDifferentNamedAndTypedProperties_ValueMappedCorrectly()
      {
         var foo = GetSampleData();

         var mapGenerator = new MappingGenerator();
         var mappings = PropertyMapping.CreateMappings<Foo, Bar>(
             (foo => foo.Id, bar => bar.BarId),
             (foo => foo.Name, bar => bar.BarName),
             (foo => foo.Description, bar => bar.BarDescription),
             (foo => foo.Age, bar => bar.BarAge)
         );

         var mapper = mapGenerator.Generate<Foo, Bar>(mappings);

         var res = mapper.Map(foo);

         Assert.AreEqual(foo.Id, res.BarId);
         Assert.AreEqual(foo.Name, res.BarName);
         Assert.AreEqual(foo.Description, res.BarDescription);
         Assert.AreEqual(foo.Age.ToString(), res.BarAge);
      }

      private Foo GetSampleData()
      {
         return new Foo()
         {
            Id = 1,
            Name = "Foo",
            Description = "Foo Bar",
            Age = 40
         };
      }

   }
}

