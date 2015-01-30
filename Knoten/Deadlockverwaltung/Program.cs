using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadlock;
namespace Deadlockverwaltung
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Verantwortlich fuer: " + args[0]);
             Verwaltung v;
            if (args[0] == "filea.txt")
            {
                v = new Verwaltung(Resource.FILEPORT_A, args[0]);

            }
            else
            {
                v = new Verwaltung(Resource.FILEPORT_B, args[0]);
            }
            v.initFile();
            v.ListenForRequest();
            
        }
    }
}
