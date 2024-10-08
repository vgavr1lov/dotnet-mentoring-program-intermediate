/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            ExecuteContinuationRegardless();
            ExecuteContinuationOnUnsuccess();
            ExecuteContinuationOnFailure();
            ExecuteContinuationOnCancelation();

            Console.ReadLine();
        }

        private static void ExecuteContinuationRegardless()
        {
            var parentTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent task A is started");
                Thread.Sleep(100);
                Console.WriteLine("Parent task A is finished");
            });
            parentTask.ContinueWith(childTask =>
            {
                Console.WriteLine("Continuation task A is started");
                Thread.Sleep(100);
                Console.WriteLine("Continuation task A is finished");
            }, TaskContinuationOptions.None);
        }

        private static void ExecuteContinuationOnUnsuccess()
        {
            var parentTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent task B is started");
                Thread.Sleep(200);
                throw null;
            });
            parentTask.ContinueWith(childTask =>
            {
                Console.WriteLine("Continuation task B is started");
                Thread.Sleep(100);
                Console.WriteLine("Continuation task B is finished");
            }, TaskContinuationOptions.NotOnRanToCompletion);
        }

        private static void ExecuteContinuationOnFailure()
        {
            var parentTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent task C is started");
                Thread.Sleep(200);
                throw null;
            });
            parentTask.ContinueWith(childTask =>
            {
                Console.WriteLine("Continuation task C is started");
                Thread.Sleep(100);
                Console.WriteLine("Continuation task C is finished");
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private static void ExecuteContinuationOnCancelation()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var parentTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent task D is started");
                Thread.Sleep(200);
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                Console.WriteLine("Parent task D is finished");
            }, cancellationTokenSource.Token);
            parentTask.ContinueWith(childTask =>
            {
                Console.WriteLine("Continuation task D is started");
                Thread.Sleep(100);
                Console.WriteLine("Continuation task D is finished");
            }, TaskContinuationOptions.OnlyOnCanceled);
        }
    }
}
