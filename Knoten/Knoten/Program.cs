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
            //TODO: read the files and id from args
            if (args.Length != 0) {
               Knoten k = new Knoten(Convert.ToInt32(args[0]));
               k.readNodes("test.txt");
               k.nodeInit();
               k.readGraph("graph1.gv");
               Console.WriteLine("********************************");
               Console.WriteLine("Node Info:" + k.id + " " + k.ip+ ":" + k.port);
               Console.WriteLine("Nachbaranzahl: " + k.neighBors.Count);
               for (int i = 0; i < k.neighBors.Count; i++)
               {
                   Console.WriteLine("Nachbar: " + k.neighBors[i].id);
               }
                   Console.WriteLine("********************************");
               k.Listen();
               for (var i = 0; i < k.rumors.Count; i++)
               {
                   Console.WriteLine("Geruecht: " + k.rumors[i].rumor + ", gehoert: " + k.rumors[i].counter);
                  
               }
            }
        }
    }
}
