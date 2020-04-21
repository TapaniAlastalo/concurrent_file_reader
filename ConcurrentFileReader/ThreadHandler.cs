using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ConcurrentFileReader
{
    public class ThreadHandler
    {
        public const int WAIT_DELAY = 10;

        // FILE        
        private int threadCount;
        private int maxQueue;

        private string path;
        private int readLength;

        public ThreadHandler(int threadCount, int maxQueue, string path, int readLength)
        {
            this.threadCount = threadCount;
            this.maxQueue = maxQueue;
            this.path = path;
            this.readLength = readLength;
        }

        public int RunThreads()
        {
            // Create blocking queue for communication.
            //using (var queue = new ResultCollection())
            using (var results = new ResultCollection(maxQueue))
            {
                // Create tasks
                var threads = new List<Thread>();
                for (int i = 0; i < threadCount; i++)
                {
                    threads.Add(new Thread(() => ReadWriteWork(i, results)));
                }
                
                // Start Threads
                foreach (var thread in threads)
                {
                    thread.Start();
                }

                // Wait for all Threads to complete
                foreach (var thread in threads)
                {
                    thread.Join();
                }

                // Results
                return CheckResults(results);
            }
        }

        private void ReadWriteWork(int counter, ResultCollection results)
        {
            // Read File
            //Console.WriteLine("START THREAD-" + counter);

            int offset = counter * readLength;
            string foundText = FileHandler.ReadFromFile(path, offset, readLength);

            // DEBUG
            //Console.WriteLine("THREAD-" + counter + " Read: " + foundText);


            // just wait
            //await WaitABit();                
            if (!results.TryAdd(foundText))
            {
                //Console.WriteLine($"THREAD-{counter}: Couldn't add text. Queue length: {results.SQueue.Count}");
            }
            else
            {
                //Console.WriteLine($"THREAD-{counter}: Added text. Queue length: {results.SQueue.Count}");
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
    }
}
