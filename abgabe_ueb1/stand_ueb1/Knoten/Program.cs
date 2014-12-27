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
           Console.WriteLine(".exe id believecounter neighbord");
            //TODO: read the files and id from args
            if (args.Length != 0) {
               Knoten k = new Knoten(Convert.ToInt32(args[0]));
                 int believeCounter = Convert.ToInt32(args[1]);
               k.readNodes("test.txt");
               k.nodeInit();
               k.readGraph("graph1.gv");
               Console.WriteLine("*************info*******************");
               Console.WriteLine("id: " + k.id + ", ip:port: " + k.ip+ ":" + k.port);
               Console.WriteLine("Nachbaranzahl: " + k.neighBors.Count);
               Console.WriteLine("*************!info******************");
               //reducing neighbor number 
               if (args.Length > 2)
               {
                   k.neighborLimit(Convert.ToInt32(args[2]));
               }
               k.Listen();
               foreach (var rumor in k.rumors)
               {
                   if (rumor.believe(believeCounter))
                   {
                       Console.WriteLine("believe " + rumor.rumor);
                   }
                   
               }
               Console.WriteLine("All Rumors **** ");
               Console.WriteLine("Node: " + k.id + ", bc: " + believeCounter);
               for (var i = 0; i < k.rumors.Count; i++)
               {
           
                   Console.WriteLine("Rumor: " + k.rumors[i].rumor + ", Count: " + k.rumors[i].counter);
                  
               }
            }
        }
    }
}
