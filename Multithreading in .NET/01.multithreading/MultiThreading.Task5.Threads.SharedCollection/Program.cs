/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private const int NumberOfElements = 10;

        private static AutoResetEvent readResetEvent = new AutoResetEvent(false);
        private static AutoResetEvent writeResetEvent = new AutoResetEvent(true);
        private static Object valueLock = new Object();

        private static List<int> integers = new List<int>();
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var writerTask = Task.Run(PopulateSharedCollection);
            var readerTask = Task.Run(ReadSharedCollection);

            Console.ReadLine();
        }

        static void PopulateSharedCollection()
        {
            for (int i = 0; i < NumberOfElements; i++)
            {
                writeResetEvent.WaitOne();
                var random = new Random();
                lock (valueLock)
                {
                    integers.Add(random.Next(0, NumberOfElements));
                }
                readResetEvent.Set();
            }
        }

        static void ReadSharedCollection()
        {
            for (int i = 0; i < NumberOfElements; i++)
            {
                readResetEvent.WaitOne();
                lock (valueLock)
                {
                    Console.Write("[ ");
                    foreach (var integer in integers)
                    {
                        Console.Write($"{integer} ");
                    }
                    Console.Write("]");
                    Console.WriteLine();
                }
                writeResetEvent.Set();
            }
        }
    }
}
