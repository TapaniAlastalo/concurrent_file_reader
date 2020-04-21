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

            new TaskHandler().RunTasks();
        }

        

    }
}
