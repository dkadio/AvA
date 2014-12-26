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

       private override void sendIdTo(){
           Message idmsg = new Message(this.id, "id", Message.CONTROLL_MSG, this.CID);
           foreach (var n in neighBors)
           {
               //Console.WriteLine("Want to Send Id to my neighbors: " + n.id);
               sendMessage(idmsg, n);
           }
       }

       private override void readIdFromNeighbor(Message msg){
           //nachschauen um was es sich fuer einen knoten handelt.
           //entsprechend den nachbarknoten casaten und in einer liste verwalten
           foreach (var n in neighBors)
           {
               if (n.id == msg.senderId)
               {
                   castNode(n, msg);
               }
           }
       }

       private void castNode(Knoten n, Message msg)
       {
           if (msg.nodetyp.StartsWith("C"))
           {
               n = new CustomerNode(n.id, n.ip, n.port);
           }
           else if (msg.nodetyp.StartsWith("B"))
           {
               n = new BusinessNode(n.id, n.ip, n.port);
           }
       }

    }
}
