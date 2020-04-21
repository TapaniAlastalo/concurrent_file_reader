using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConcurrentFileReader
{
    class Tester
    {
        const int RUNNERS = 100;        
        const int QUEUE_LIMIT = 1000 * 1000;

        // FILE 1- 4
        const string PATH_1 = "test_files\\test1.txt";
        const string PATH_2 = "test_files\\test2.txt";
        const string PATH_3 = "test_files\\test3.txt";
        const string PATH_4 = "test_files\\test4.txt";
        const string PATH_5 = "test_files\\test5.txt";

        // Read lengths
        const int TINY_READ_LENGTH = 10;
        const int MINI_READ_LENGTH = 1000;
        const int SHORT_READ_LENGTH = 1000;
        const int MEDIUM_READ_LENGTH = 100 * 1000;
        const int LONG_READ_LENGTH = 1000 * 1000;
        const int FULL_READ_LENGTH = 1000 * 1000 * 1000;

        private Stopwatch timer;

        public Tester()
        {
            timer = new Stopwatch();
        }

        public void RunTests()
        {
            // TINY
            Console.WriteLine("TINY");
            TestTinyFile();
            // SMALL
            Console.WriteLine("SMALL");
            TestSmallFile();
            // MEDIUM
            Console.WriteLine("MEDIUM");
            TestMediumFile();
            // BIG
            Console.WriteLine("BIG");
            TestBigFile();
            // HUGE
            Console.WriteLine("HUGE");
            TestHugeFile();
        }

        private void TestTinyFile()
        {
            // SETUP
            string usedPath = PATH_1;
            long fileLength = FileHandler.GetFileLength(usedPath);
            Console.WriteLine($"FILE {usedPath} has length: {fileLength}.\n");

            // TEST DEFAULTS
            TestDefaultReading(usedPath, (int)fileLength, false);
            // TEST TASKS
            TestTaskReading(usedPath, (int)fileLength);
            // TEST THREADS
            //TestThreadReading(usedPath, (int)fileLength);
        }

        private void TestSmallFile()
        {
            // SETUP
            string usedPath = PATH_2;
            long fileLength = FileHandler.GetFileLength(usedPath);
            Console.WriteLine($"FILE {usedPath} has length: {fileLength}.\n");

            // TEST DEFAULTS
            TestDefaultReading(usedPath, (int)fileLength, false);
            // TEST TASKS
            TestTaskReading(usedPath, (int)fileLength);
            // TEST THREADS
            //TestThreadReading(usedPath, (int)fileLength);
        }

        private void TestMediumFile()
        {
            // SETUP
            string usedPath = PATH_3;
            long fileLength = FileHandler.GetFileLength(usedPath);
            Console.WriteLine($"FILE {usedPath} has length: {fileLength}.\n");

            // TEST DEFAULTS
            TestDefaultReading(usedPath, (int)fileLength, true);
            // TEST TASKS
            TestTaskReading(usedPath, (int)fileLength);
            // TEST THREADS
            //TestThreadReading(usedPath, (int)fileLength);
        }

        private void TestBigFile()
        {
            // SETUP
            string usedPath = PATH_4;
            long fileLength = FileHandler.GetFileLength(usedPath);
            Console.WriteLine($"FILE {usedPath} has length: {fileLength}.\n");

            // TEST DEFAULTS
            TestDefaultReading(usedPath, (int)fileLength, true);
            // TEST TASKS
            TestTaskReading(usedPath, (int)fileLength);
            // TEST THREADS
            //TestThreadReading(usedPath, (int)fileLength);
        }

        private void TestHugeFile()
        {
            // SETUP
            string usedPath = PATH_5;
            long fileLength = FileHandler.GetFileLength(usedPath);
            Console.WriteLine($"FILE {usedPath} has length: {fileLength}.\n");

            // TEST DEFAULTS
            TestDefaultReading(usedPath, (int)fileLength, true);
            // TEST TASKS
            TestTaskReading(usedPath, (int)fileLength);
            // TEST THREADS
            //TestThreadReading(usedPath, (int)fileLength);
        }

        private void BasicTests()
        {
            // Run example
            // Example.Run(); // PASS

            // Run FileHandler Test
            //FileHandlerTest.Run(); // PASS
        }

        // THREADS TESTING
        private void TestThreadReading(string path, int readLength)
        {
            // RUN THREAD TESTS
            Console.WriteLine("\nTEST THREADS");            
            int results = 0;
            ThreadHandler thread;

            int[] threadCounts = new int[]
            {
                1,
                2,
                4,
                8,
                16,
                //32,
                //64,
                //128,
                //256,
                //512,
                //1024,
            };

            foreach (int threadCount in threadCounts)
            {
                Console.WriteLine($"TEST THREADS-{threadCount}: START.");
                thread = new ThreadHandler(threadCount, QUEUE_LIMIT, path, CountReadLength(readLength, threadCount));
                timer.Reset();
                timer.Start();
                results = thread.RunThreads();
                timer.Stop();
                Console.WriteLine($"TEST THREADS-{threadCount}: END. SUM = {results}. Timer = {timer.ElapsedMilliseconds}\n");
            }
        }

        // TASKS TESTS
        private void TestTaskReading(string path, int readLength)
        {
            // RUN TASK TESTS
            Console.WriteLine("\nTEST TASKS");
            int results = 0;
            TaskHandler task;

            int[] taskCounts = new int[]
            {
                1,
                2,
                4,
                8,
                16,
                32,
                64,
                128,
                256,
                //512,
                //1024,
            };

            foreach(int taskCount in taskCounts)
            {
                Console.WriteLine($"TEST TASKS-{taskCount}: START.");
                task = new TaskHandler(taskCount, QUEUE_LIMIT, path, CountReadLength(readLength, taskCount));
                timer.Reset();
                timer.Start();
                results = task.RunTasks();
                timer.Stop();
                Console.WriteLine($"TEST TASKS-{taskCount}: END. SUM = {results}. Timer = {timer.ElapsedMilliseconds}\n");
            }
        }

        // DEFAULT TESTS
        private void TestDefaultReading(string path, int readLength, bool skip)
        {
            int results = 0;
            // Skip totally inefficient method
            if (!skip)
            {
                // JUST READ 
                Console.WriteLine("TEST DEFAULT READING: FILE READ START.");
                timer.Reset();
                timer.Start();
                results = CountResults(FileHandler.ReadFromFile(path));
                timer.Stop();
                Console.WriteLine($"TEST DEFAULT READING: FILE READ END. SUM = {results}. Timer = {timer.ElapsedMilliseconds}");
            }

            // JUST DO OFFSET READ FROM 0
            Console.WriteLine("TEST OFFSET 0 READING: FILE READ START.");
            timer.Reset();
            timer.Start();
            results = CountResults(FileHandler.ReadFromFile(path, 0, readLength));
            timer.Stop();
            Console.WriteLine($"TEST OFFSET 0 READING: FILE READ END. SUM = {results}. Timer = {timer.ElapsedMilliseconds}");
        }

        private int CountResults(string text)
        {
            char[] delimiters = new char[] { '\r', '\n' };
            string[] values = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            int sum = 0;
            for(int i = 0; i < values.Length; i++)
            {
                try
                {
                    sum += int.Parse(values[i]);
                }
                catch (Exception e)
                {
                    // parsing error
                }
            }
            return sum;
        }

        // Counts optimized reading length divided equally between no of runners
        private int CountReadLength(int fileLength, int divider)
        {
            return (fileLength / divider) + 1;
        }
    }
}
