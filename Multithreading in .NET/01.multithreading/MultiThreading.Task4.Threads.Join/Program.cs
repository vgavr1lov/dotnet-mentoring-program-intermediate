/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;
using System.Xml.Linq;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private const int NumberOfThreads = 10;
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(NumberOfThreads);
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            Console.WriteLine("Result of using Thread class and Join method:");
            DecrementIntegerWithJoin(NumberOfThreads);

            Console.WriteLine("Result of using ThreadPool class and Semaphore:");
            ThreadPool.SetMaxThreads(NumberOfThreads, NumberOfThreads);
            DecrementIntegerWithSemaphore(NumberOfThreads);

            Console.ReadLine();
        }

        private static void DecrementIntegerWithSemaphore(object input)
        {
            var integer = (int)input;
            integer--;
            Console.WriteLine(integer);
            if (integer > 0)
            {
                semaphoreSlim.Wait();
                ThreadPool.QueueUserWorkItem(new WaitCallback(DecrementIntegerWithSemaphore), integer);
            }
        }

        private static void DecrementIntegerWithJoin(object input)
        {
            var integer = (int)input;
            integer--;
            Console.WriteLine(integer);

            if (integer > 0)
            {
                var thread = new Thread(DecrementIntegerWithJoin);
                thread.Start(integer);
                thread.Join();
            }
        }

    }
}
