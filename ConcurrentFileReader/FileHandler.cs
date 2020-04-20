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
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        // Read the source file into a byte array.
                        byte[] buffer = new byte[fs.Length];
                        long maxLength = fs.Length;
                        
                        if(offset < maxLength)
                        {
                            // Limit length to avoid out of bounds
                            if (offset + length > maxLength)
                            {
                                length = (int)(maxLength - (long)offset);
                            }

                            // set offset
                            //fs.Position = offset;
                            fs.Seek(offset, SeekOrigin.Begin);
                            fs.Read(buffer, 0, length);

                            // add to return values
                            values += Encoding.UTF8.GetString(buffer, 0, length); // Encoding.UTF8.GetString(buffer);
                        }
                        // close
                        fs.Close();
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
            int offset = 4;
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
