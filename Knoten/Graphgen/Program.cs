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
                Console.WriteLine("graphgen.exe nodeanzahl");
            }
            else
            {
                Console.WriteLine("Generate graphiz");
                int init = Convert.ToInt32(args[0]);
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
                        file.WriteLine(i + " -- " + j + ";");
                        Console.WriteLine("inserted edge: " + i +" -- " + j);
                    }
                    
                }
                file.WriteLine("}");
                file.Close();
            }
            
        }
    }
}
