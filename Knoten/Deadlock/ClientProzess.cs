using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Deadlock
{
    class ClientProzess
    {
        //gerader Prozess dekrementiert file A
        public String fileA;
        public String fileB;
        public volatile bool rwFileA;
        public volatile bool rwFileB;
        public int deadlockCounter;
        public Resource resource;
        public String format = "#000000";

        public List<int> portlist;
        public ClientProzess(String fileA, String fileB, int port)
        {
            //starte immer mit fileA 
            this.fileA = fileA;
            this.fileB = fileB;
            if (port % 2 == 0)
            {
                this.fileB = fileA;
                this.fileA = fileB;
            }
            rwFileA = false;
            rwFileB = false;
            Console.WriteLine("port: " + port);
            resource = new Resource(port);
            deadlockCounter = 0;

            portlist = resource.getPortList("portlist.txt");
        }

        public Message schreibRechtAnfordern(String datei)
        {
            Console.WriteLine("Ich fordere schreibrecht auf " + datei);
            Message msg = new Message(resource.port, datei, Message.REQUEST_FILE);

            if (datei == fileA)
            {
                resource.Connect(Resource.HOST, Resource.FILEPORT_A, msg); // fordere schreibrecht für file a an 
                msg = resource.Listen(); //auf Antwort von PA warten
            }
            else
            {
                resource.Connect(Resource.HOST, Resource.FILEPORT_B, msg); // fordere schreibrecht für file b an 
                msg = resource.Listen(); // auf Antwort von PB warten
            }
            return msg;

        }

        private bool checkMsg(Message msg)
        {
            //check if i got permission
            switch (msg.typ)
            {
                case Message.GRANTED_FILE:
                    //habe schreibrechte erhalten
                    Console.WriteLine("schreibrecht erhalten" + Environment.NewLine);

                    return true;
                case Message.RELEASED_FILE:
                    //habe schreibrecht aufgegeben
                    Console.WriteLine("schreibrecht aufgegeben" + Environment.NewLine);
                    return true;
                case Message.CONTROLL_MSG:
                    //ich bin der blockierende Prozess ich gebe schreibrecht auf
                    Console.WriteLine("!!!Ich verzichte auf schreibrechte da ich blockiere" + Environment.NewLine);
                    if (rwFileA)
                    {
                        schreibRechtAufgaben(fileA);
                    }
                    else
                    {
                        schreibRechtAufgaben(fileB);
                    }
                    return false;
                case Message.RENOUNCE_FILE_OK:
                    //hier sollte ich landen wenn ich auf schreibrecht verzichtet habe
                    Console.WriteLine("schreibrecht verzichtet" + Environment.NewLine);

                    return false;
                case Message.REFUSAL_FILE:
                    //frag den prozess der die schreibrechte hat ob er sie aufgeben will
                    deadlockCounter++;
                    checkDeadlock(msg);
                    return false;

            }
            return false;
        }

        public Message schreibRechtAufgaben(String datei)
        {
            Message msg = new Message(resource.port, datei, Message.RELEASE_FILE);
            if (datei == fileA)
            {
                resource.Connect(Resource.HOST, Resource.FILEPORT_A, msg); // fordere schreibrecht für file a an 
                msg = resource.Listen(); //auf Antwort von PA warten
            }
            else
            {
                resource.Connect(Resource.HOST, Resource.FILEPORT_B, msg); // fordere schreibrecht für file b an 
                msg = resource.Listen(); // auf Antwort von PB warten
            }
            return msg;
        }


        //inkrementiert den wert der angegebenen Datei
        public void inkrementFile(String datei)
        {
            Console.WriteLine("ich inkrementiere " + datei);
            String[] lines = System.IO.File.ReadAllLines(datei);
            int value = Convert.ToInt32(lines[0]) + 1;
            Console.WriteLine(value);

            lines[0] = value.ToString(format);
            System.IO.File.WriteAllLines(datei, lines);
        }

        //dekrementiert den wert der angegebenen Datei
        public void dekrementFile(String datei)
        {
            Console.WriteLine("ich inkrementiere " + datei);
            String[] lines = System.IO.File.ReadAllLines(datei);
            int value = Convert.ToInt32(lines[0]) - 1;
            Console.WriteLine(value);
            lines[0] = value.ToString(format);
            System.IO.File.WriteAllLines(datei, lines);
        }

        //haengt pi ans ende
        public void changeFileName(String datei)
        {
            Console.WriteLine("ich aendere namen von " + datei);
            StreamWriter sw = new StreamWriter(datei, true);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(resource.port);
            sw.Close();
        }




        internal void start()
        {
            int i = 30;
            while (i > 0)
            {

                //im Konstruktor wird je nach Prozessnummer die richtige file zuerst angefordert
                //a anfordern
                if (!rwFileA)
                {
                    rwFileA = checkMsg(schreibRechtAnfordern(fileA));
                    Console.WriteLine(fileA + ": " + rwFileA);
                    Console.WriteLine(fileB + ": " + rwFileB);
                }
                //wenn a dann b anfordern
                if (rwFileA)
                {
                    rwFileB = checkMsg(schreibRechtAnfordern(fileB));
                    Console.WriteLine(fileA + ": " + rwFileA);
                    Console.WriteLine(fileB + ": " + rwFileB);

                }
                if (rwFileA && rwFileB)
                {
                    inkrementFile(fileA);
                    dekrementFile(fileB);
                    changeFileName(fileA);
                    changeFileName(fileB);
                    rwFileA = !checkMsg(schreibRechtAufgaben(fileA));
                    rwFileB = !checkMsg(schreibRechtAufgaben(fileB));
                }
                i--;
            }


            //a schreben
            //b schreiben
            //name aendern
            //freigeben
        }

        private void checkDeadlock(Message msg)
        {
            Console.WriteLine("DDC: " + deadlockCounter);

            Console.WriteLine("prozess blockiert: " + msg.prozessId + Environment.NewLine);
            if (deadlockCounter > new Random().Next(2, 10))
            {
                //da keine Priorität sagen wir einfach das es random ist wer die kontrollnachricht sendet
                Console.WriteLine("!!!!!Ich sag dem anderen Prozess das er schreibrechte aufgeben soll" + Environment.NewLine);
                msg.typ = Message.CONTROLL_MSG;
                msg.senderId = resource.port;
                resource.Connect(Resource.HOST, msg.prozessId, msg);
                deadlockCounter = 0;
            }

        }
    }
}
