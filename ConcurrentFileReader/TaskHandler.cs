﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;

namespace ConcurrentFileReader
{
    public class TaskHandler
    {
        public const int WAIT_DELAY = 10;
        
        // FILE        
        private int taskCount;
        private int maxQueue;

        private string path;
        private int readLength;
                
        public TaskHandler(int taskCount, int maxQueue, string path, int readLength)
        {
            this.taskCount = taskCount;
            this.maxQueue = maxQueue;
            this.path = path;
            this.readLength = readLength;
        }

        public int RunTasks()
        {
            // Create blocking queue for communication.
            //using (var queue = new ResultCollection())
            using (var results = new ResultCollection(maxQueue))
            {
                // Create tasks
                var tasks = new List<Task>();
                for (int i = 0; i < taskCount; i++)
                {
                    tasks.Add(ReadWriteAsync(i, results));
                }

                // Wait for all tasks to complete
                Task.WaitAll(tasks.ToArray());

                // Results
                return CheckResults(results);
            }
        }

        private async Task ReadWriteAsync(int counter, ResultCollection results)
        {
            // Read File
            //Console.WriteLine("START TASK-" + counter);

            // just wait
            //await WaitABit();
            
            int offset = counter * readLength;
            string foundText = FileHandler.ReadFromFile(path, offset, readLength);
            
            // DEBUG
            //Console.WriteLine("TASK-" + counter + " Read: " + foundText);

            
            // just wait
            //await WaitABit();                
            if (!results.TryAdd(foundText))
            {
                //Console.WriteLine($"TASK-{counter}: Couldn't add text. Queue length: {results.SQueue.Count}");
            }
            else
            {
                //Console.WriteLine($"TASK-{counter}: Added text. Queue length: {results.SQueue.Count}");
            }
        }

        private int CheckResults(ResultCollection results)
        {
            if (!results.TryCountSum(out int value))
            {
                //Console.WriteLine($"\t\t\t\t\t\t\tCHECK-OUT: Couldn't take correct value: SUM = {value} , from results. Queue length: {results.SQueue.Count}");                
            }
            else
            {
                //Console.WriteLine($"\t\t\t\t\t\t\tCHECK-OUT: Success: SUM = {value}. Queue length: {results.SQueue.Count}");
            }
            return value;
        }

        private async Task WaitABit()
        {
            await Task.Delay(WAIT_DELAY);
        }
    }
}
