using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knoten
{
    class CustomerNode : Knoten
    {
        public String CID;
        public List<Produkt> productlist;

        public CustomerNode(int id, String ip, int port)
            : base(id, ip, port)
        {
            this.CID = "CID" + this.id;
        }

        public void buyProduct()
        {
            //send msg to neighbors 
        }

        public override void readMessage(Message msg)
        {
            base.readMessage(msg);
            if (msg.typ != null)
            {
                switch (msg.typ)
                {
                    case Message.CAMPAIGN_MSG:
                        Console.WriteLine("CAMPAING MSG");
                        Console.WriteLine();
                        campaingMsg(msg);
                        break;
                    default:
                        break;
                }
            }
        }

        private void campaingMsg(Message msg)
        {
            //leite nachricht an nachbarn weiter
            foreach (var node in neighBors)
            {
                if (msg.senderId != node.id)
                {
                    sendMessage(msg, node);
                }

            }
            // prüfe ob das produkt gekauft werden will

            // kaufe das produkt und sag es weiter
        }

    }
}
