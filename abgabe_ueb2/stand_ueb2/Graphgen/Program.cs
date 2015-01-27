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
            List<String> insertededges = new List<string>();
            if (args[0] == "?" || args.Length < 1)
            {
                Console.WriteLine("graphgen.exe nodeanzahl kantenanzahl");
            }
            else
            {
                Console.WriteLine("Generate graphiz");
                int init = Convert.ToInt32(args[0]); //anzahl knoten
                int edges = Convert.ToInt32(args[1]); //anzahl kanten
                if (init < edges)
                {
                    edges -= init-1;
                    Console.WriteLine(edges);

                }
                else
                {
                    Console.WriteLine("Kanten Anzahl kleiner oder gleich Knotenanzahl!!");
                    edges = 1;
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
                        inserEdge(file, i, j, insertededges);
                    }
                    
                }
                for (var i = 0; i < edges; i++)
                {
                    var j = r.Next(1, init);
                    var k = r.Next(1, init);
                    if (j == k )
                    {
                        i--;
                    }
                    else
                    {
                        inserEdge(file, j, k, insertededges);
                    }
                }
                    file.WriteLine("}");
                    removedoubleedges(file, insertededges);
                file.Close();
          
            }
            
        }

        private static void removedoubleedges(StreamWriter file, List<String> insertededges)
        {
            //remove double edges
            //read file in list and check if there is an double entry
           
        }

        private static void inserEdge(StreamWriter file, int i, int j, List<String> insertededges)
        {

            String edge = j + " -- " + i + ";";
            String edge2 = i + " -- " + j + ";";
            if (!insertededges.Contains(edge) || !insertededges.Contains(edge2))
            {
                file.WriteLine(edge);
                Console.WriteLine("insert edge: " + edge);
            }
            else
            {
                Console.WriteLine("Edge was allrdy inserted");
            }
          
            insertededges.Add(edge);
            insertededges.Add(edge2);

        }
    }
}
