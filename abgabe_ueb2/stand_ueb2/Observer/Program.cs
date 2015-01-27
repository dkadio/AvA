using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Observer
{
    class Program
    {
        static void Main(string[] args)
        {
            Observer workerObject = new Observer(-2, "localhost", 6000);
            Thread workerThread = new Thread(workerObject.DoWork);
            workerObject.readNodes("test.txt");
            // Start the worker thread.
            workerThread.Start();
            Console.WriteLine("main thread: Starting worker thread...");
            
            // Loop until worker thread activates.
            while (workerThread.IsAlive)
            {
                Thread.Sleep(10000);
                Console.WriteLine("!!!!!!!Pruefe Termninierung 1");
                if (workerObject.Terminated())
                {
               
                    int rs = workerObject.getRS();
                    while (workerThread.IsAlive)
                    {
                        Thread.Sleep(15000);
                        Console.WriteLine("!!!!!!!Pruefe Termninierung 2");
                        if (workerObject.Terminated() && rs == workerObject.getRS())
                        {
                            Console.WriteLine("Terminierung erfolgreich. alle nachrichten angekommen");
                            workerObject.RequestStop();
                        }

                    }
                    
                }
                
            }

            // Put the main thread to sleep for 1 millisecond to
            // allow the worker thread to do some work:
            

            // Request that the worker thread stop itself:
            

            // Use the Join method to block the current thread 
            // until the object's thread terminates.
            workerThread.Join();
            Console.WriteLine("main thread: Worker thread has terminated.");
            Console.ReadLine();
        }
    }
}
