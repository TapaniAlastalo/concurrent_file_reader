using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ConcurrentFileReader
{
    public class TaskHandler
    {
        const int RUNNERS = 100; 
        const int WAIT_DELAY = 10;
        const int QUEUE_LIMIT = 1000*1000;

        // FILE
        //const string PATH = "test_files\\test3.txt";
        const string PATH = "test_files\\test4.txt";
        const int LENGTH = 10*1000;

        public void RunTasks()
        {
            // Create blocking queue for communication.
            //using (var queue = new ResultCollection())
            using (var results = new ResultCollection(QUEUE_LIMIT))
            {
                // Create tasks
                var tasks = new List<Task>();
                for (int i = 0; i < RUNNERS; i++)
                {
                    tasks.Add(ReadWriteAsync(i, results));
                }

                // Wait for all tasks to complete
                Task.WaitAll(tasks.ToArray());

                // Results
                CheckResults(results);
            }
        }

        private async Task ReadWriteAsync(int counter, ResultCollection results)
        {
            // Read File
            Console.WriteLine("START TASK-" + counter);

            // just wait
            await WaitABit();
            
            int offset = counter * LENGTH;
            string foundText = FileHandler.ReadFromFile(PATH, offset, LENGTH);
            
            // DEBUG
            //Console.WriteLine("TASK-" + counter + " Read: " + foundText);

            
            // just wait
            await WaitABit();                
            if (!results.TryAdd(foundText))
            {
                Console.WriteLine($"TASK-{counter}: Couldn't add text. Queue length: {results.SQueue.Count}");
            }
            else
            {
                Console.WriteLine($"TASK-{counter}: Added text. Queue length: {results.SQueue.Count}");
            }
        }

        private void CheckResults(ResultCollection results)
        {
            if (!results.TryCountSum(out int value))
            {
                Console.WriteLine($"\t\t\t\t\t\t\tCHECK-OUT: Couldn't take correct value: SUM = {value} , from results. Queue length: {results.SQueue.Count}");
            }
            else
            {
                Console.WriteLine($"\t\t\t\t\t\t\tCHECK-OUT: Success: SUM = {value}. Queue length: {results.SQueue.Count}");
            }

        }

        private async Task WaitABit()
        {
            await Task.Delay(WAIT_DELAY);
        }
    }
}
