using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knoten
{
    class CustomerNode : Node
    {
        public List<Produkt> products;
        public int werbungheard;
        public int kaufheard;
        public int buyCounter;
        public Node parent;
        public int echoCounter;
        public String max;

        public CustomerNode(int id, String ip, int port, String nodeTypeId, int buyCounter)
            : base(id, ip, port)
        {
            Random r = new Random();
            this.nodeTypeId = nodeTypeId;
            this.werbungheard = r.Next(0, 10);
            this.kaufheard = r.Next(0, 10);
            this.buyCounter = buyCounter;
            this.products = new List<Produkt>();
            echoCounter = 0;
        }

        public CustomerNode(int id, String ip, int port, String nodeTypeId)
            : base(id, ip, port)
        {
            Random r = new Random();
            this.nodeTypeId = nodeTypeId;
            this.werbungheard = r.Next(0, 10);
            this.kaufheard = r.Next(0, 10);
            this.buyCounter = 0;
            this.products = new List<Produkt>();
            echoCounter = 0;
        }

        public CustomerNode(int id, String nodeTypeId, int buyCounter)
            : base(id)
        {
            Random r = new Random();
            this.nodeTypeId = nodeTypeId;
            this.werbungheard = r.Next(0, 10);
            this.kaufheard = r.Next(0, 10);
            this.buyCounter = buyCounter;
            this.products = new List<Produkt>();
            echoCounter = 0;
        }

        public override void readMessage(Message msg)
        {
            base.readMessage(msg);
            switch (msg.typ)
            {
                //bei einer kampagnen nachricht.
                case Message.CAMPAIGN_MSG:
                    analyseCampaign(msg);
                    break;
                case Message.BUY_MSG:
                    //ein anderer Knoten hat ein produkt gekauft
                    analyseBuyMsg(msg);
                    break;
                case Message.EXPLORER_MSG:
                    //merke dir von wem du die nachricht erhalten hast wenn es die erste ist

                    checkParent(msg);
                    
                    echoBack(msg);
                    break;
                case Message.ECHO_MSG:
                    checkParent(msg);
                    echoBack(msg);

                    break;
            }
        }

        private void echoBack(Message msg)
        {
            max = max + msg.nachricht;
            if (echoCounter == neighBors.Count)
            {
                
                Console.WriteLine("---!!!Sende echo");
                Console.WriteLine(max);
                msg.typ = Message.ECHO_MSG;
                msg.nachricht = max;
                sendMessage(msg, parent);
                Status = (int)Farbe.Gruen;
            }
        }

        private void checkParent(Message msg)
        {
            Console.WriteLine("!!!Checkparent");
            echoCounter++;
            if (echoCounter == 1)
            {
                foreach (var node in allNodes)
                {
                    if (node.id == msg.senderId)
                    {
                        parent = node;
                    }
                }
                Console.WriteLine("!!!my parent is: " + parent.id);
                sendExplorer(msg);
            }
            Status = (int)Farbe.Rot;
        }

        private void sendExplorer(Message msg)
        {

            msg.typ = Message.EXPLORER_MSG;
            msg.senderId = this.id;
            msg.nachricht = msg.nachricht + ":" +this.id + ":";
            foreach (var n in neighBors)
            {
                if (n.id != parent.id)
                {
                    sendMessage(msg, n);
                }

            }
        }

        private void analyseBuyMsg(Message msg)
        {
            Produkt current = new Produkt(msg.nachricht);
            if (products.Contains(current))
            {
                //bereits gehört --> counter hochzählen
                products[products.IndexOf(current)].kaufCounter++;
                checkKaufentscheidungForBuy(products[products.IndexOf(current)], msg);
                Console.WriteLine("Buy nachricht erhalten und kenne das produkt schon; kaufcounter(Produkt gekauft): " + products[products.IndexOf(current)].kaufCounter);
            }
            else
            {
                products.Add(current);
            }
        }

        private void checkKaufentscheidungForBuy(Produkt produkt, Message msg)
        {
            if (produkt.kaufCounter > kaufheard && buyCounter > produkt.buyCounter)
            {
                //schicke nachricht an firma und nachbarn und zähle den buycounter hoch
                BuyProduct(produkt, msg);
            }
            else
            {
                Console.WriteLine("--Buy Counter erreicht oder Schwelle nicht erreicht: ");
                Console.WriteLine("Buycounter: " + buyCounter);
                Console.WriteLine("ProduktBuycounter(Produkt gekauft): " + produkt.buyCounter);
                Console.WriteLine("Kaufschwelle fuer Buynachrichten: " + kaufheard);
            }
        }

        private void BuyProduct(Produkt produkt, Message msg)
        {
            Console.WriteLine("Buy Produkt: " + produkt.produktName);
            produkt.buyCounter++;
            msg.typ = Message.BUY_MSG;
            msg.senderId = this.id;
            foreach (var n in neighBors)
            {
                if (n.nodeTypeId == "CID")
                {
                    sendMessage(msg, n);
                }
            }
            msg.typ = Message.CONTROLL_MSG;

            msg.nachricht = "addme";
            Console.WriteLine("Sende ADDME NACHRICHT!");
            produkt.extractIdFromName();
            foreach (var bn in allNodes)
            {
                if (bn.id == produkt.id)
                {

                    //sende nachricht an firma die produkt verkauft
                    sendMessage(msg, bn);
                }
            }
            addNewFriend();
        }

        private void addNewFriend()
        {
            bool found = true;
            foreach (var nn in allNodes)
            {
                if (!neighBors.Contains(nn) && nn.nodeTypeId == "CID" && found)
                {
                    Console.WriteLine("Added new neighbor: ");
                    nn.getinformation();
                    neighBors.Add(nn);
                    found = false;
                }
            }
        }

        private void analyseCampaign(Message msg)
        {
            //analysiere die nachricht und tu was zu tun ist
            //sende selbe nachricht weiter an alle customer knoten
            //erhöhe t und t' für erhaltenes produkt
            Produkt current = new Produkt(msg.nachricht);
            if (products.Contains(current))
            {
                //bereits gehört --> counter hochzählen
                products[products.IndexOf(current)].werbungCounter++;
                checkKaufentscheidungForWerbung(products[products.IndexOf(current)], msg);
                Console.WriteLine("Buy nachricht erhalten und kenne das produkt schon; kaufcounter: " + products[products.IndexOf(current)].werbungCounter);

            }
            else
            {
                products.Add(current);
            }
        }

        private void checkKaufentscheidungForWerbung(Produkt produkt, Message msg)
        {
            //checke ob die schwelle für das produkt erreicht ist 
            if (produkt.werbungCounter > werbungheard && buyCounter > produkt.werbungCounter)
            {
                BuyProduct(produkt, msg);
            }
            else
            {
                Console.WriteLine("--Buy Counter ist abgelaufen oder Schwelle nicht erreicht: ");
                Console.WriteLine("Buycounter: " + buyCounter);
                Console.WriteLine("ProduktBuycounter(Produkt gekauft): " + produkt.buyCounter); ;
                Console.WriteLine("Kaufschwelle fuer Werbung: " + werbungheard);
            }
        }


        public override void printid()
        {
            Console.WriteLine("CustomerKnoten");
        }

        public override void getinformation()
        {
            Console.WriteLine("*************info*******************");
            Console.WriteLine("id: " + id + ", ip:port: " + ip + ":" + port);
            Console.WriteLine("Schwellen  t, t': " + kaufheard + " " + werbungheard);
            Console.WriteLine("BuyCounter: " + buyCounter);
            Console.WriteLine("Nachbaranzahl: " + neighBors.Count);
            foreach (var n in neighBors)
            {
                Console.WriteLine(n.id + " " + n.GetType());
            }
            Console.WriteLine("*************!info******************");
        }

        public override void getProductAndNeihborInfo()
        {
            getinformation();
            Console.WriteLine("*************Product and Neighbors*******************");
            Console.WriteLine("Produke gekauft: " + products.Count);
            foreach (var p in products)
            {
                Console.WriteLine(p.produktName + " / gekauft: " + p.buyCounter + " / werbungheard: " + p.werbungCounter + " / kaufnachrichten: " + p.kaufCounter);
            }
            Console.WriteLine("************************************");
            Console.WriteLine("Nachbaranzahl: " + neighBors.Count);
            Console.WriteLine("*************!Product and Neighbors*******************");

        }
    }
}
