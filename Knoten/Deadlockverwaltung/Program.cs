using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadlockverwaltung
{
    class Program
    {
        static void Main(string[] args)
        {
             Verwaltung v;
            if (args[0] == "filea.txt")
            {
                v = new Verwaltung(5000, args[0]);

            }
            else
            {
                v = new Verwaltung(5001, args[0]);
            }
            v.ListenForRequest();
            
        }
    }
}
