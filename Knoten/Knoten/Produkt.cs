using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knoten
{
    [Serializable]
    public class Produkt
    {
        String BID;

        public Produkt()
        {

        }
        
        public Produkt(String BID)
        {
            this.BID = BID;
        }
    }
}
