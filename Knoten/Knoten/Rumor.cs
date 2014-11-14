using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knoten
{
    class Rumor : IEquatable<Rumor>
    {
        public String rumor;
        public int counter;

        public Rumor(int p1, string p2)
        {
            // TODO: Complete member initialization
            this.counter = p1;
            this.rumor = p2;
        }

        public bool Equals(Rumor other)
        {
            return rumor == other.rumor;
        }

        public bool believe(int n)
        {
            if (counter >= n)
                return true;
            else return false;
        }
    }
}
