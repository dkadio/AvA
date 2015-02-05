using System;
using System.Collections.Generic;
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

        public String fileA;
        public String fileB;
        public volatile bool rwFileA;
        public volatile bool rwFileB;
        public Resource resource;

        public List<int> portlist;
        public ClientProzess(String fileA, String fileB, int port)
        {
            //starte immer mit fileA 
            this.fileA = fileA;
            this.fileB = fileB;
            rwFileA = false;
            rwFileB = false;
            resource = new Resource(port);
            portlist = resource.getPortList("portlist.txt");
        }

        public bool schreibRechtAnfordern(String datei)
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
            return checkMsg(msg);

        }

        private bool checkMsg(Message msg)
        {
            //check if i got permission
            switch (msg.typ)
            {
                case Message.GRANTED_FILE:
                    //habe schreibrechte erhalten
                    Console.WriteLine("schreibrecht erhalten"+ Environment.NewLine);
                    
                    return true;
                case Message.RELEASED_FILE:
                    //habe schreibrecht aufgegeben
                    Console.WriteLine("schreibrecht aufgegeben" + Environment.NewLine);
                    return true;
                case Message.CONTROLL_MSG:
                    //habe kontrollnachricht erhalten --> edge chasing
                    //prüfe ob ich deadlock bin wenn ja verzichte auf schreibrecht wenn nein return true(behalte weiterhin schreibrechte)
                    //sende release msg --> lande in renounce wenn das geklappt hat 

                    //gib hier die schreibrecht auf wenn du eine solche nachricht bekommst
                    return false;
                case Message.RENOUNCE_FILE_OK:
                    //hier sollte ich landen wenn ich auf schreibrecht verzichtet habe
                    Console.WriteLine("schreibrecht verzichtet" + Environment.NewLine);

                    return false;
                case Message.REFUSAL_FILE:
                    //frag den prozess der die schreibrechte hat ob er sie aufgeben will
                    Console.WriteLine("Keien schreibrechte erhalten da prozess blockiert: " + msg.prozessId);
                    msg.typ = Message.CONTROLL_MSG;
                    msg.senderId = resource.port;
                    resource.Connect(Resource.HOST, msg.prozessId, msg);
                    return false;
                   
            }
            return false;
        }

        public bool schreibRechtAufgaben(String datei)
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
            return checkMsg(msg);
        }


        //inkrementiert den wert der angegebenen Datei
        public void inkrementFile(String datei)
        {
            Console.WriteLine("ich inkrementiere " + datei );
        }

        //dekrementiert den wert der angegebenen Datei
        public void dekrementFile(String datei)
        {
            Console.WriteLine("ich dekrementiere " + datei);
        }

        //haengt pi ans ende
        public void changeFileName()
        {
            Console.WriteLine("ich aendere namen");
        }



    }
}
