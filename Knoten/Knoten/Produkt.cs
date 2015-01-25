using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knoten
{
    [Serializable]
    public class Produkt : IEquatable<Produkt>
    {
        public String produktName;
        public int id;
        public int kaufCounter; // wie oft haben freunde das gekauft
        public int werbungCounter; // wie oft hat man darüber werbung erhalten
        public int buyCounter; // wie oft is das produkt wirklich gekauft worden

        public Produkt()
        {

        }

        public Produkt(String produktName)
        {
            this.produktName = produktName;
            this.id = -1;
            kaufCounter = 0;
            werbungCounter = 0;
            buyCounter = 0;
        }
        
        public Produkt(String produktName, int id)
        {
            this.produktName = produktName;
            this.id = id;
            kaufCounter = 0;
            werbungCounter = 0;
            buyCounter = 0;
        }

        public void extractIdFromName()
        {
            this.id = Convert.ToInt32(produktName.Substring(produktName.Length-1));
        }

        public bool Equals(Produkt other)
        {
            return produktName == other.produktName;
        }
    }
}
