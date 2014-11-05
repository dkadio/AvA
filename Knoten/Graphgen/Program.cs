using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Graphgen
{
    class Program
    {
        /**
         * Generates a random graphviz file
         */
        static void Main(string[] args)
        {
            if (args[0] == "?" || args.Length == 0)
            {
                Console.WriteLine("graphgen.exe nodeanzahl anzahl kanten");
            }
            else
            {
                Console.WriteLine("Generate graphiz");
                int init = Convert.ToInt32(args[0]);
                int edges = Convert.ToInt32(args[1]);
                if (init < edges)
                {
                    edges -= init-1;
                    Console.WriteLine(edges);

                }
                else
                {
                    Console.WriteLine("Kanten Anzahl kleiner als Knotenanzahl!!");
                }
                String filename = "graph1.gv";
                StreamWriter file = new StreamWriter(filename);
                Random r = new Random();
                //wirte first line
                file.WriteLine("graph G {");
                //generate the edges now
                for (var i = 1; i <= init; i++)
                {
                    if (i > 1)
                    {
                        var j = r.Next(1, i);
                        inserEdge(file, i, j);
                    }
                    
                }
                for (var i = 0; i < edges; i++)
                {
                    var j = r.Next(1, init);
                    var k = r.Next(1, init);
                    if (j == k)
                    {
                        i--;
                    }
                    else
                    {
                        inserEdge(file, j, k);
                    }
                }
                    file.WriteLine("}");
                file.Close();
            }
            
        }

        private static void inserEdge(StreamWriter file, int i, int j)
        {
            file.WriteLine(i + " -- " + j + ";");
            Console.WriteLine("inserted edge: " + i + " -- " + j);
        }
    }
}
