using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Knoten
{
    class BusinessNode : Node
    {
        public Produkt produkt;
        public int etat;
        public int echoCounter;
        public int max;
        public bool echoInit;
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
            echoCounter = 0;
            echoInit = false;
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
            echoCounter = 0;
            echoInit = false;
        }


        public void startKampagne()
        {
            echoInit = true;
            echo();

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

        /**
         * Echo algo init
         */
        private void echo()
        {
            Status = (int)Farbe.Rot;
            Message msg = new Message(this.id, Message.EXPLORER_MSG, Message.EXPLORER_MSG);
            foreach (var n in neighBors)
            {
                sendMessage(msg, n);
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
                case Message.EXPLORER_MSG:
                   // if (echoInit)
                   // {
                      //  checkEcho(msg);
                   // }
                  //  else
                 //   {
                        sendEmptyEcho(msg);
                    //}
                    break;
                case Message.ECHO_MSG:
                    if (echoInit)
                    {
                        checkEcho(msg);
                    }
                    else
                    {
                        sendEmptyEcho(msg);
                    }
                    break;
            }
        }

        private void sendEmptyEcho(Message msg)
        {
            msg.typ = Message.ECHO_MSG;
            msg.nachricht = "0";
            
            foreach(var n in neighBors){
                if (n.id == msg.senderId)
                {
                    msg.senderId = this.id;
                    sendMessage(msg, n);
                }
            }
            echoCounter = 0;
            Status = (int)Farbe.Weiss;
        }

        private void checkEcho(Message msg)
        {
            echoCounter++;
            max = max + Convert.ToInt32(msg.nachricht);
            Console.WriteLine("!!Explorer MSG EC, echos erhalten: " + echoCounter+ "/" + neighBors.Count);
            Console.WriteLine(max);

            if (echoCounter == neighBors.Count)
            {
                Console.WriteLine("Alle haben echo erhalten: " + max + " Werden erreicht");
                echoInit = false;
                echoCounter = 0;
                max = 0;
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
                case "echo":
                    echoInit = true;
                    echo();
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
