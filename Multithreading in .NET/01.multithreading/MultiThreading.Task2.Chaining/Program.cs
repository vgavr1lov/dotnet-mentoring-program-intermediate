/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        private const int NumberOfIntegers = 10;
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var firstTask = Task.Run(() =>
            {
                return CollectRandomIntegers(NumberOfIntegers);
            });

            var secondTask = firstTask.ContinueWith(t =>
            {
                return MultiplyByRandomInteger(firstTask.Result);
            });

            var thirdTask = secondTask.ContinueWith(t =>
            {
                return SortIntegersByAscending(secondTask.Result);
            });

            var fourthTask = thirdTask.ContinueWith(t =>
            {
                return CalculateAverage(thirdTask.Result);
            });

            Console.ReadLine();
        }

        private static BigInteger[] CollectRandomIntegers(int numberOfIntegers)
        {
            var integers = new BigInteger[numberOfIntegers];

            for (int i = 0; i < numberOfIntegers; i++)
            {
                integers[i] = GetRandomInteger();
            }

            Print(integers);

            return integers;
        }

        private static BigInteger[] MultiplyByRandomInteger(BigInteger[] integers)
        {
            for (int i = 0; i < integers.Length; i++)
            {
                integers[i] *= GetRandomInteger();
            }

            Print(integers);

            return integers;
        }

        private static BigInteger[] SortIntegersByAscending(BigInteger[] integers)
        {
            var sortedIntegers = integers.OrderBy(i => i).ToArray();

            Print(sortedIntegers);

            return sortedIntegers;
        }

        private static BigInteger CalculateAverage(BigInteger[] integers)
        {
            var average = CalculateSum(integers) / integers.Length;

            Print(average);

            return average;
        }

        private static BigInteger CalculateSum(BigInteger[] integers)
        {
            BigInteger sum = 0;
            foreach (var integer in integers)
            {
                sum += integer;
            }

            return sum;
        }

        private static void Print(BigInteger[] integers)
        {
            foreach (var integer in integers)
            {
                Console.WriteLine(integer.ToString());
            }
            Console.WriteLine();
        }

        private static void Print(BigInteger integer)
        {
            Console.WriteLine(integer.ToString());
            Console.WriteLine();
        }

        private static int GetRandomInteger()
        {
            var random = new Random();

            return random.Next(int.MinValue, int.MaxValue);
        }
    }
}
