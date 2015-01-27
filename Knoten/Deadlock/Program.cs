using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace Deadlock
{
    class Program
    {
        static void Main(string[] args)
        {
            int pid = Process.GetCurrentProcess().Id % 3;
            if (pid == 0)
            {
                //starte mit File B
                Console.WriteLine("B");
                ClientProzess cp = new ClientProzess("b", "a");
            }
            else
            {
                //starte mit File A
                Console.WriteLine("A");
                ClientProzess cp = new ClientProzess("a", "b");
            }
            Console.ReadLine();
            
        }
    }
}
