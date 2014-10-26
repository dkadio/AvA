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
            Knoten k = new Knoten();
            k.readNodes("test.txt");
        }
    }
}
