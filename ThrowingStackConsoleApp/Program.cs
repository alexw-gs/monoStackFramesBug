using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ThrowingStackConsoleApp
{
    class Program
    {
        static void Main()
        {
            MainAsync().Wait();
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        static async Task MainAsync()
        {
            Console.WriteLine("Starting program");
            Console.WriteLine();
            try
            {
                await Method1Task('x');
            }
            catch (Exception exception)
            {
                Console.WriteLine($"exception.ToString(): {exception.ToString()}");

                Console.WriteLine("=================================================================================");
                var stackTrace = new StackTrace(exception, false);
                Console.WriteLine($"stackTrace.FrameCount: {stackTrace.FrameCount}");
                Console.WriteLine("=================================================================================");

                var stackFramesIndividual = new StackFrame[stackTrace.FrameCount];
                for (int i = 0; i < stackTrace.FrameCount; i++)
                {
                    stackFramesIndividual[i] = stackTrace.GetFrame(i);
                }
                Console.WriteLine($"stackFramesIndividual.Length: {stackFramesIndividual.Length}");
                Console.WriteLine("=================================================================================");
                var stackFrameIndividualString = stackFramesIndividual.Select(f => $"{GetClassName(f)}.{GetMethodName(f)}{Environment.NewLine}").Aggregate((a, b) => $"{a}{b}");
                Console.WriteLine($"individual stack frames: {stackFrameIndividualString}");
                Console.WriteLine("=================================================================================");


                var stackFrames = stackTrace.GetFrames();
                Console.WriteLine($"stackFrames.Length: {stackFrames.Length}");
                Console.WriteLine("=================================================================================");

                var stackFrameString = stackFrames.Select(f => $"{GetClassName(f)}.{GetMethodName(f)}{Environment.NewLine}").Aggregate((a, b) => $"{a}{b}");
                Console.WriteLine($"stack frames: {stackFrameString}");
                Console.WriteLine("=================================================================================");
                Console.WriteLine("=================================================================================");


                
                Console.WriteLine($"stackTrace.ToString: {stackTrace.ToString()}");
                Console.WriteLine("=================================================================================");

            }
        }

        static async Task<int> Method1Task(char key)
        {
            return await Method2Task(key);
        }
        static async Task<int> Method2Task(char key)
        {
            return await Method3Task(key);
        }
        static async Task<int> Method3Task(char key)
        {
            return await Method4Task(key);
        }
        static async Task<int> Method4Task(char key)
        {
            await Task.Delay(10);
            if (key == 'x')
            {
                throw new Exception("Typed x exception");
            }
            Console.WriteLine("you didn't type x so no exception");
            return 0;
        }

        private static string GetMethodName(StackFrame frame)
        {
            return frame.GetMethod()?.Name ?? "Unknown_Method";
        }

        private static string GetClassName(StackFrame frame)
        {
            return frame.GetMethod()?.DeclaringType?.FullName ?? "Unknown_Class";
        }
    }
}
