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

            /*Console.WriteLine(".exe id believecounter neighbord");
            CustomerNode cnode = new CustomerNode(1, "localhost", 5000);
            cnode.readNodes("test.txt");
            cnode.nodeInit();
            cnode.readGraph("graph1.gv");
            
            Console.WriteLine("*************info*******************");
            Console.WriteLine("id: " + cnode.id + ", ip:port: " + cnode.ip + ":" + cnode.port);
            Console.WriteLine("Nachbaranzahl: " + cnode.neighBors.Count);
            Console.WriteLine("*************!info******************");
            cnode.Listen();
             */
            //TODO: read the files and id from args
             if (args.Length != 0) {
                 int id = Convert.ToInt32(args[0]);
                 String nodetype = args[1];
                 int buyCounter = Convert.ToInt32(args[2]);
                 if (args.Length > 3)
                 {
                     int believeCounter = Convert.ToInt32(args[4]);
                     int neigborlimit = Convert.ToInt32(args[3]);
                 }
                 Node k = new Node();
                 if (nodetype == "BID")
                 {
                      k = new BusinessNode(id, nodetype);
                 }
                 else if (nodetype == "CID")
                 {
                      k = new CustomerNode(id, nodetype, buyCounter);
                 }
                 k.printid();
                 k.readNodes("test.txt");
                 k.nodeInit();
                 k.readGraph("graph1.gv");
                 k.getinformation();
                 k.Listen();
                 k.getProductAndNeihborInfo();
                 /* k.readNodes("test.txt");
                  k.nodeInit();
                  k.readGraph("graph1.gv");
                  Console.WriteLine("*************info*******************");
                  Console.WriteLine("id: " + k.id + ", ip:port: " + k.ip+ ":" + k.port);
                  Console.WriteLine("Nachbaranzahl: " + k.neighBors.Count);
                  Console.WriteLine("*************!info******************");
                  //reducing neighbor number 
                  if (args.Length > 2)
                  {
                      k.neighborLimit(neigborlimit));
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
                  
                  }*/
                 Console.ReadLine();
             }
             else
             {
                 Console.WriteLine("id nodetype buylimit neighborlimit believecounter");
             }
        }
    }
}
