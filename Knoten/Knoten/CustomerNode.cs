using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knoten
{
    class CustomerNode : Knoten
    {
        public List<Produkt> products;
        public int werbungheard;
        public int kaufheard;
        public int buyCounter;
        
        public CustomerNode(int id, String ip, int port, String nodeTypeId, int buyCounter) : base(id, ip, port)
        {
            Random r = new Random();
            this.nodeTypeId = nodeTypeId;
            this.werbungheard = r.Next(0, 10);
            this.kaufheard = r.Next(0,10);
            this.buyCounter = buyCounter;
            this.products = new List<Produkt>();
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
                Console.WriteLine("Buy nachricht erhalten und kenne das produkt schon; kaufcounter: " + products[products.IndexOf(current)].kaufCounter);
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
                Console.WriteLine("ProduktBuycounter: " + produkt.buyCounter);
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
            bool found =  true;
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
                Console.WriteLine("ProduktBuycounter: " + produkt.buyCounter); ;
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
