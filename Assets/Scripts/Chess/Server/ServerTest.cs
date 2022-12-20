using System;
using System.Collections.Generic;
using System.Threading;

namespace Chess.Server
{
    public class ServerTest
    {
        static Model.Server server = new Model.Server();
        
        public static void Main()
        {
            MultiThreadTest();
        }

        public static void SimpleTest()
        {
            for (int i = 0; i < 10; i++)
            {
                server.Join();
            }
            Console.WriteLine(server.GameCount == 5);
        }
        
        public static void MultiThreadTest()
        {
            var t = DateTime.Now;
            var threads = new List<Thread>();
            for (var i = 0; i < 10; i++)
            {
                var thread = new Thread(TestJoin);
                thread.Start();
                thread.Name = "" + i;
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.Out.WriteLine(DateTime.Now - t + ", gameCount=" + server.GameCount);
        }

        private static void TestJoin()
        {
            for (int i = 0; i < 10; i++)
            {
                server.Join();
            }
        }
    }
}