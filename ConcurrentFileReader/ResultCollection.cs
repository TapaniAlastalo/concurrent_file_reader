using System;
using System.Collections.Generic;
using System.Linq;

namespace ConcurrentFileReader
{
    public class ResultCollection : IDisposable
    {
        public List<string> SQueue { get; set; }
        public int Sum { get; set; }
        private readonly bool isLimited;
        private int limit;

        public ResultCollection()
        {
            SQueue = new List<string>();
            isLimited = false;
            Sum = 0;
        }
        public ResultCollection(int maxCount)
        {
            SQueue = new List<string>();
            isLimited = true;
            limit = maxCount;
            Sum = 0;
        }

        public bool TryAdd(string text)
        {
            // check whether queue is not limited or there is still room
            if (!isLimited || SQueue.Count < limit)
            {
                lock (SQueue)
                {
                    char[] delimiters = new char[] { '\r', '\n' };
                    string[] values = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                                        
                    // Add first and last to SQueue for latter use. Because there may be incorrectness
                    int count = values.Length;
                    if (count > 0)
                    {
                        SQueue.Add(values[0]);
                        if (count > 1)
                        {
                            SQueue.Add(values[count -1]);
                            if (count > 2)
                            {
                                // Sum up others from second until one before last
                                for (int i = 1; i < count - 1; i++)
                                {
                                    // Debug
                                    //Console.WriteLine("TryAdd: " + value);
                                    try
                                    {
                                        Sum += int.Parse(values[i]);
                                    }
                                    catch (Exception e)
                                    {
                                        //success = false;
                                    }
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public bool TryCountSum(out int value)
        {
            lock (SQueue)
            {
                bool success = true;
                while (SQueue.Count > 0)
                {
                    try
                    {
                        Sum += int.Parse(SQueue[0]);
                    }
                    catch (Exception e)
                    {
                        success = false;
                    }
                    finally
                    {
                        SQueue.RemoveAt(0);                        
                    }
                }
                value = Sum;
                return success;
            }
        }

        public int QueueCount()
        {
            return SQueue.Count();
        }


        public void Dispose()
        {
            //throw new NotImplementedException();
        }

    }
}
