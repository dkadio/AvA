using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knoten
{
    class Program
    {
        static void Main(string[] args)
        {
           // if (args.Length != 0) {
            Knoten k = new Knoten(1);//Convert.ToInt32(args[0]));
               k.readNodes("test.txt");
               k.nodeInit();
               Console.WriteLine("Node Info:" + k.id + " " + k.ip+ ":" + k.port);
               Console.WriteLine("Nachbaranzahl: " + k.neighBors.Count);
               k.Listen();
          //  }
        }
    }
}
