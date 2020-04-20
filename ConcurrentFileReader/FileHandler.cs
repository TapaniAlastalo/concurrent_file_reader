using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ConcurrentFileReader
{
    abstract class FileHandler
    {
        public static void WriteToFile(string text, string path)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(text);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public static string ReadFromFile(string path)
        {
            string values = "";
            if (File.Exists(path))
            {
                try
                {
                    // Open the file to read from.
                    using (StreamReader sr = File.OpenText(path))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            values += s + "\n";
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            return values;
        }

        public static string ReadFromFile(string path, int offset, int length)
        {
            string values = "";
            if (File.Exists(path))
            {
                try
                {
                    // Open the file to read from.
                    using (FileStream fsSource = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        // Read the source file into a byte array.
                        byte[] bytes = new byte[fsSource.Length];
                        long maxLength = fsSource.Length;
                        // Debug
                        Console.WriteLine("Max length: " + maxLength);


                        if (offset + length <= maxLength)
                        {
                            fsSource.Read(bytes, offset, length);
                        }
                        /*while (length > 0)
                        {
                            // Read may return anything from 0 to length.
                            int n = fsSource.Read(bytes, offset, length);

                            // Debug
                            Console.WriteLine("Found: " + n);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            offset += n;
                            length -= n;
                        }
                        length = bytes.Length;
                        */

                        // add to return values
                        values += Encoding.UTF8.GetString(bytes);

                    }
                }
                catch (FileNotFoundException fnfe)
                {
                    Console.WriteLine(fnfe.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            return values;
        }
    }

    public static class FileHandlerTest
    {
        private static void TestFileReading()
        {
            Console.WriteLine("TEST Read From File");
            string path = "test_files\\test1.txt";
            string txt = FileHandler.ReadFromFile(path);
            Console.WriteLine("File Read:");
            Console.WriteLine(txt);
        }
        private static void TestOffsetFileReading()
        {
            Console.WriteLine("TEST Read From File with offset");
            string path = "test_files\\test1.txt";
            int offset = 40;
            int length = 20;
            string txt = FileHandler.ReadFromFile(path, offset, length);
            Console.WriteLine("File Read:");
            Console.WriteLine(txt);
        }

        private static void TestFileWriting()
        {
            Console.WriteLine("TEST Write To File");
            //string path = @"c:\temp\T1TextLines.txt";
            string path = "test_files\\testWrite.txt";
            string s = "Test Text";
            // Write
            FileHandler.WriteToFile(s, path);
            // Read
            string textFromFile = FileHandler.ReadFromFile(path);
            Console.Write("\n" + textFromFile);
        }

        public static void Run()
        {
            //TestFileWriting();
            TestFileReading();
            TestOffsetFileReading();
        }
    }
}
