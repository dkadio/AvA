using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knoten
{
    class BusinessNode : Node
    {
        public Produkt produkt;
        public int etat;

        /*
         * Erstellt einen neuen Businesskniten und initialisiert sein Produkt 
         */
        public BusinessNode(int id, String ip, int port, String nodeTypeId)
            : base(id, ip, port)
        {
            this.nodeTypeId = nodeTypeId;
            this.produkt = new Produkt("Produkt" + this.id, this.id);
            this.etat = new Random().Next(10, 15);
            R = -1;
        }

        /*
        * Erstellt einen neuen Businessknoten und initialisiert sein Produkt 
        */
        public BusinessNode(int id, String nodeTypeId)
            : base(id)
        {
            this.nodeTypeId = nodeTypeId;
            this.produkt = new Produkt("Produkt" + this.id, this.id);
            this.etat = new Random().Next(10, 20);
            R = -1;
        }


        public void startKampagne()
        {
            while (etat > 0)
            {
                Console.WriteLine("Starte Kampagne, etat: " + etat);
                etat = etat - 1;
                Message msg = new Message(this.id, produkt.produktName, Message.CAMPAIGN_MSG);
                //Für alle Customernachbarn
                foreach (var cn in neighBors)
                {
                    if (cn.nodeTypeId == "CID")
                    {
                        sendMessage(msg, cn);
                    }
                }
            }
        }

        public override void readMessage(Message msg)
        {
            base.readMessage(msg);
            switch (msg.typ)
            {
                //bei einer kampagnen nachricht.
                case Message.CAMPAIGN_MSG:

                    break;
            }
        }

        public override void ctrlMsg(Message msg)
        {
            base.ctrlMsg(msg);
            switch (msg.nachricht)
            {
                case "addme":
                    addNodeToNeighbors(msg);
                    break;
                case "init":
                    //if init -> start kampagne
                    startKampagne();
                    break;
            }

        }

        private void addNodeToNeighbors(Message msg)
        {
            foreach (var nn in allNodes)
            {
                if (nn.id == msg.senderId)
                {
                    if (!neighBors.Contains(nn))
                    {
                        Console.WriteLine("Nachbar hinzugefügt " + nn.id);
                        neighBors.Add(nn);
                        etat++;
                    }
                }
            }
            
            startKampagne();
        }

        /**
         * Test Funktion zum nachsehen welcher Typ Knoten es ist
         */
        public override void printid()
        {
            Console.WriteLine("BusinessKnoten");
        }

        public override void getinformation()
        {
            Console.WriteLine("*************info*******************");
            Console.WriteLine("id: " + id + ", ip:port: " + ip + ":" + port);
            Console.WriteLine("Produkt: " + produkt.produktName);
            Console.WriteLine("Nachbaranzahl: " + neighBors.Count);
            foreach (var n in neighBors)
            {
                Console.WriteLine(n.id + " " + n.GetType());
            }
            Console.WriteLine("*************!info******************");
        }

        public override void getProductAndNeihborInfo()
        {
            Console.WriteLine("*************Product and Neighbors*******************");
            Console.WriteLine("Nachbaranzahl: " + neighBors.Count);
            Console.WriteLine("*************!Product and Neighbors*******************");

        }
    }
}
