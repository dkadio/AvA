using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Knoten
{
    
    class Knoten
    {
        private int id;             
        private String ip;
        private int port;
        private List<Knoten> allNodes;   //are all information about the other nodes

        public Knoten(int id, String ip, int port) 
        {
            this.id = id;
            this.ip = ip;
            this.port = port;
            allNodes = new List<Knoten>();
        }

        public Knoten(){}

        public void Listen()
        {
            allNodes = new List<Knoten>();
        }

        /**
         * Read a List of Nodes
         * Beispiel:
            1 isl-s-01:5000
            2 isl-s-01:5001
            3 127.0.0.1:2712
         *  @param filePath the Path where The file is stored
         */
        public void readNodes(String filePath)
        {
            string line;
            int counter = 0;
            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader("../../"+filePath);
            while ((line = file.ReadLine()) != null)
            {
                String[] words = line.Split(' ');
                String[] ipPort = words[1].Split(':');

                String ip = ipPort[0];
                int port = Convert.ToInt32(ipPort[1]);
                int id = Convert.ToInt32(words[0]);

                counter++;
                Console.WriteLine(ip);
                Console.WriteLine(port);
                Console.WriteLine(id);
            }

            file.Close();

            // Suspend the screen.
            Console.ReadLine();
        }


    }
}
