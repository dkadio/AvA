using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Deadlock;
namespace Deadlockverwaltung
{
    class Verwaltung
    {
        int port;
        String fileName;
        bool schreibrechtFrei;
        int counter;
        Resource resource;
        List<int> portList;
        List<Message> requests;

        public Verwaltung(int port, String file)
        {
            this.port = port;
            fileName = file;
            schreibrechtFrei = true;
            counter = 0;
            resource = new Resource(port);
            portList = resource.getPortList("portlist.txt");
            requests = new List<Message>();
        }


        public void ListenForRequest()
        {
            while (true)
            {
                Message msg = resource.Listen();
                checkPermission(msg);
            }

        }

        private void checkPermission(Message msg)
        {
            switch (msg.typ)
            {
                case Message.RELEASE_FILE:
                    //prozess will schreibrecht aufloesen
                    Console.WriteLine("Schreibrecht wurde aufgeloest" + Environment.NewLine);
                    msg.typ = Message.RELEASED_FILE;
                    resource.Connect(Resource.HOST, msg.senderId, msg);
                    schreibrechtFrei = true;
                    if (requests.Count > 0)
                    {
                        grantMessage(msg);
                    }
                    break;
                case Message.REQUEST_FILE:
                    //prozess will schreibrecht anfrodern
                    requests.Add(msg);
                    Console.WriteLine("Adde Prozess zur queue: " + msg.senderId + " gr: " + requests.Count + Environment.NewLine);
                    if (schreibrechtFrei)
                    {
                        grantMessage(msg);
                    }
                    break;
                case Message.RENOUNCE_FILE:
                    //prozess verzichtet auf seine schreibrechte
                    Console.WriteLine("Auf schreibrecht wurde verzichtet: " + msg.senderId + Environment.NewLine);
                    requests.Remove(msg);
                    msg.typ = Message.RENOUNCE_FILE_OK;
                    resource.Connect(Resource.HOST, msg.senderId, msg);
                    break;
            }
        }

        private void grantMessage(Message msg)
        {
            requests[0].typ = Message.GRANTED_FILE;
            resource.Connect(Resource.HOST, requests[0].senderId, requests[0]);
            requests.Remove(msg);
            Console.WriteLine("Arbeite queue ab: " + msg.senderId + " gr: " + requests.Count + Environment.NewLine);
            schreibrechtFrei = false;
        }

        internal void initFile()
        {
            //ueberschreib den inhalt der datei mit 000000
        }
    }
}
