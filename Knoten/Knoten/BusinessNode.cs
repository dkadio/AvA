using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knoten
{
    class BusinessNode : Knoten
    {
        public String BID;
        private int etat;

        public Produkt product;

        public BusinessNode(int id, String ip, int port)
            : base(id, ip, port)
        {
            this.etat = new Random().Next(5, 25);
            this.BID = "BID" + this.id;
            this.product = new Produkt(BID);
        }

        public void startCampaign()
        {
            if (this.etat > 0)
            {
                //dec etat
                this.etat -= 5;
                var cmsg = new Message(this.id, "Werbung", Message.CAMPAIGN_MSG, this.product);
                //send productid to neighbors
                foreach (var node in base.neighBors)
                {
                    base.sendMessage(cmsg, node);
                    
                }
            }
        }
    }
}
