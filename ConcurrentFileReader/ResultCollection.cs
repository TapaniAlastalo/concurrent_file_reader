using System;
using System.Collections.Generic;
using System.Linq;

namespace ConcurrentFileReader
{
    public class ResultCollection : IDisposable
    {
        public List<string> SQueue { get; set; }
        public int Count { get; set; }
        public long Sum { get; set; }
        private readonly bool isLimited;
        private int limit;

        public ResultCollection()
        {
            SQueue = new List<string>();
            isLimited = false;
            int Count = 0;
            long Sum = 0;
        }
        public ResultCollection(int maxCount)
        {
            SQueue = new List<string>();
            isLimited = true;
            limit = maxCount;
            int Count = 0;
            long Sum = 0;
        }

        public bool TryAdd(string text)
        {
            // check whether queue is not limited or there is still room
            if (!isLimited || SQueue.Count < limit)
            {
                lock (SQueue)
                {
                    string[] values = text.Split();
                    foreach(string value in values)
                    {
                        // Debug
                        Console.WriteLine("TryAdd: " + value);
                        SQueue.Add(value);
                    }                    
                    return true;
                }
            }
            return false;
        }


        public bool TryTake(out string value)
        {
            lock (SQueue)
            {
                if (SQueue.Count > 0)
                {
                    value = SQueue[0];
                    SQueue.RemoveAt(0);
                    return true;
                }
                else
                {
                    value = "";
                    return false;
                }
            }
        }

        public bool TryCountSum(out int value)
        {
            lock (SQueue)
            {
                value = 0;
                bool success = true;
                while (SQueue.Count > 0)
                {
                    try
                    {
                        value += int.Parse(SQueue[0]);
                    }
                    catch (Exception e)
                    {
                        value += 0;
                        success = true;
                    }
                    finally
                    {
                        SQueue.RemoveAt(0);                        
                    }
                }
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
