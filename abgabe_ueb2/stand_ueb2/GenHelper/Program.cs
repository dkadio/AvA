using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenHelper
{

    /*
     * Dieses Programm generiert ein File mit init Daten für die Customer und Busisness nodes
     */
    class Program
    {
        static void Main(string[] args)
        {
            int businessnodes = Convert.ToInt32(args[1]);
            int customerNodes = Convert.ToInt32(args[2]);
            int allnodes = Convert.ToInt32(args[0]);
            String filename = "test.txt";
            foreach (String s in args)
            {
                Console.WriteLine(s);
            }
            Random r = new Random();
            StreamWriter file = new StreamWriter(filename);
            for (int i = 1; i <= allnodes; i++)
            {
                Boolean cob = customOrBusiness(businessnodes, customerNodes, allnodes, r);
                Console.WriteLine("cob: " + cob);
                Console.WriteLine(businessnodes);
                Console.WriteLine(customerNodes);
                if (cob && businessnodes > 0 )
                {
                    businessnodes = businessnodes - 1;
                    generateBusiNode(i, file);
                }
                else if (!cob && customerNodes > 0)
                {
                    customerNodes = customerNodes - 1;
                    generateCusoNode(i, file);
                }
                else if(customerNodes + businessnodes >= 1){
                    
                    if (businessnodes > 1)
                    {
                        businessnodes = businessnodes - 1;
                        generateBusiNode(i, file);
                    }
                    else
                    {
                        customerNodes = customerNodes - 1;
                        generateCusoNode(i, file);
                    }
                }
                else
                {
                    Console.WriteLine("Mehr Knoten gesamt als Bus und Cus zusammen --> random");
                    if (cob)
                    {
                        generateBusiNode(i, file);
                    }
                    else
                    {
                        generateCusoNode(i, file);
                    }
                }
               
            }
            file.Close();
        }

        private static void generateCusoNode(int i, StreamWriter file)
        {
            genNode(i, file, "CID");
        }

        private static void genNode(int i, StreamWriter file, string p)
        {
            if (i <= 10)
            {
                file.WriteLine(i + " localhost:500" + (i - 1) + " " + p);
            }
            else
            {
                file.WriteLine(i + " localhost:50" + (i - 1) + " " + p);
            }
        }

        private static bool customOrBusiness(int businessnodes, int customerNodes, int allnodes, Random r)
        {
            int value = r.Next(0, allnodes);
            Console.WriteLine("value: " + value);
            if (value < businessnodes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void generateBusiNode(int i, StreamWriter file)
        {
            genNode(i, file, "BID");
        }


    }
}
