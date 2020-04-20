using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace ConcurrentFileReader
{
    class Program
    {
        static void Main(string[] args)
        {
            RunTests();
        }

        private static void RunTests()
        {
            // Run example
            // Example.Run(); // PASS

            // Run FileHandler Test
            //FileHandlerTest.Run(); // PASS

            RunTasks();
        }

        const int QUEUE_LIMIT = 100;
        const int WAIT_DELAY = 10;
        const int RUNNERS = 10;
        const int LENGTH = 10;

        private static void RunTasks()
        {
            // Create blocking queue for communication.
            using (var queue = new BlockingCollection<string>(QUEUE_LIMIT))
            {
                // Create tasks
                var tasks = new List<Task>
                {
                    ReadAsync(0),
                    ReadAsync(1),
                    ReadAsync(2),
                    ReadAsync(3),
                    ReadAsync(4),
                    ReadAsync(5),
                    ReadAsync(6),
                    ReadAsync(7),
                    ReadAsync(8),
                    ReadAsync(9),
                };

                // Wait for all tasks to complete
                Task.WaitAll(tasks.ToArray());
            }
        }

        private static async Task ReadAsync(int counter)
        {
            //while (true)
            //{
                
                // Read File
                Console.WriteLine("START TASK-" + counter);
                
                // just wait
                await WaitABit();

                string path = "test_files\\test1.txt";
                int offset = counter * LENGTH;
                int length = LENGTH;
                string txt = FileHandler.ReadFromFile(path, offset, length);
                Console.WriteLine("TASK-" + counter + " Read: " + txt);
                Console.WriteLine(txt);
                
                // Console.WriteLine($"\t\t\t\t\t\t\tCouldn't take value from queue. Queue length: {queue.Count}");                
            //}
        }

        static async Task WaitABit()
        {
            await Task.Delay(WAIT_DELAY);
        }

    }
}
