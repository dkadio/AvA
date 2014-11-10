﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace Knoten
{
    

    class Knoten 
    {
        public const string CTRLMSG = "ctrl";
        public const string NORMALMSG = "msg";
        public int id { get; set; }
        public String ip { get; set; }
        public int port { get; set; }
        public List<Knoten> allNodes { get; set; }   //are all information about the other nodes
        public List<Knoten> neighBors { get; set; }
        public Boolean end;
        public Boolean initator;
        public Boolean sendId;
        public List<Rumor> rumors;

        public Knoten(int id, String ip, int port)
        {
            this.id = id;
            this.ip = ip;
            this.port = port;
            allNodes = new List<Knoten>();
            neighBors = new List<Knoten>();
            end = true;
            sendId = true;
            rumors = new List<Rumor>();

        }

        public Knoten()
        {
            allNodes = new List<Knoten>();
            neighBors = new List<Knoten>();
            end = true;
            sendId = true;
            rumors = new List<Rumor>();

        }

        public Knoten(int id)
        {
            allNodes = new List<Knoten>();
            neighBors = new List<Knoten>();
            this.id = id;
            end = true;
            sendId = true;
            rumors = new List<Rumor>();
        }

        /**
         * Listen to inc msg, Format 
         *                      "ctrl#controllmsg" 
         *                      "msg#normale message"
         */
        public void Listen()
        {
            //Maybe use newer version
            TcpListener listener = new TcpListener(port);

            try
            {

                // Start listening for client requests.
                listener.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (end)
                {
                    Console.WriteLine("* * * *********************************");
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;
                    XmlSerializer reader = new XmlSerializer(typeof(Message));
                    Message msg = (Message) reader.Deserialize(stream);
                    // Loop to receive all the data sent by the client.
/*
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("*********new MSG*******");
                        Console.WriteLine("Received " + DateTime.Now + ": {0}", data);

                        // read the received Data and check if is a controll or a normal msg


                        //  byte[] msg = System.Text.Encoding.ASCII.GetBytes("ctrl#danke");

                        // Send back a response.
                        //    stream.Write(msg, 0, msg.Length);
                        //   Console.WriteLine("Sent " + DateTime.Now + ":{0}", data);

                    }
                    */

                    readMessage(msg);
                    // Shutdown and end connection
                    client.Close();

                }

                

            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("Client hat unerwartet beendet");
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                listener.Stop();
            }

        }

        /**
         * reads a msg and decide if its a ctrl oder a normal msg
         */
        public void readMessage(Message msg)
        {
            Console.WriteLine("*********new MSG*******");
            Console.WriteLine("Received from " + msg.senderId + ": {0}", msg.nachricht + "  " + DateTime.Now);
            Console.WriteLine("Werte Nachricht aus");

            if (msg.typ != null)
            {
                //String msgtyp = msg.Split('#')[0];
                // msg = msg.Split('#')[1];
                switch (msg.typ)
                {
                    case Knoten.CTRLMSG:
                        Console.WriteLine(msg.typ + "/Knontrollnachricht erhalten");
                        ctrlMsg(msg);
                        break;
                    case Knoten.NORMALMSG:
                        Console.WriteLine(msg.typ + "/Normale Nachricht erhalten");
                        normalMsg(msg);
                        break;
                }
            }
        }

        /**
         * Analyse a normal Msg
         */
        private void normalMsg(Message msg)
        {
            if (rumors.Contains(new Rumor(0, msg.nachricht)))
            {
                Console.WriteLine("Know this rumor Allready");
                Console.WriteLine("*#+#+#+#+#+#+#+#+#+#+#+#+#+#+#+#+#+#+#00");
                rumors[rumors.IndexOf(new Rumor(0, msg.nachricht))].counter++;
                Console.WriteLine(rumors[rumors.IndexOf(new Rumor(0, msg.nachricht))].counter);
            }
            else
            {
                rumors.Add(new Rumor(0, msg.nachricht));
                try
                {

                    if (initator)
                    {
                        Console.WriteLine("I am the Initiator");
                        //im the initator and send this msg to all my neighbors
                        msg.senderId = this.id;
                        foreach (var node in neighBors)
                        {
                            sendMessage(msg, node);
                        }

                    }
                    else
                    {
                        //im not the iniotator so i have to look at the id 
                        Console.WriteLine("I am no Initiator");

                        int incId = msg.senderId;
                        msg.senderId = this.id;
                        Console.WriteLine("I send this msg to all my neighbors");
                        foreach (var node in neighBors)
                        {
                            if (!(incId == node.id))
                            {
                                sendMessage(msg, node);
                            }

                        }

                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Im not an Initator");
                }
            }
        }

        /**
         * Analyse a Controllmsg
         */
        private void ctrlMsg(Message msg)
        {
            switch (msg.nachricht)
            {
                case "end":
                    end = false;
                    break;
                case "endall":
                    end = false;
                    foreach (var node in allNodes)
                    {
                        if (node.id != id)
                        {
                            Console.WriteLine("End node " + node.id);
                            sendMessage(msg, node);
                        }
                    }
                    break;
                case "init":
                    initator = true;
                    Console.WriteLine("Knoten " + this.id + " ist jetzt Initiator");
                    break;
                case "id":
                    Console.WriteLine("Knoten " + msg.senderId + " hat mir id gesendet");
                    break;
            }
        }

        /**
         * Sends a msg to a node
         */
        private void sendMessage(Message msg, Knoten node)
        {

            if (sendId)
            {
                sendId = false;
                sendIdTo();

            }
            try
            {
                //send a msg to a tcp listener
                TcpClient client = new TcpClient(node.ip, node.port);
                //Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);

                NetworkStream stream = client.GetStream();
                var writer = new XmlSerializer(typeof(Message));
                // Send the message to the connected TcpServer. 
                //stream.Write(data, 0, data.Length);
                writer.Serialize(stream, msg);
                Console.WriteLine("##############################");
                Console.WriteLine("Sent  to " + node.id +  msg.nachricht + "  " + DateTime.Now);
                Console.WriteLine("##############################");
                stream.Close();
                client.Close();
            }
            catch (SocketException)
            {
                Console.WriteLine("At Sending the Msg: " + msg.nachricht + ", Knoten " + node.id + " nicht vergeben oder nicht erreichbar");
            }
        }

        private void sendIdTo()
        {
            //Console.WriteLine("before i send the message to neighbours i have to send my it to them");
            Message init = new Message(this.id, "id", Knoten.CTRLMSG);
            foreach (var n in neighBors)
            {
                //Console.WriteLine("Want to Send Id to my neighbors: " + n.id);
                sendMessage(init, n);
            }
        }

        /**
         * Read a List of Nodes
         * Beispiel:
            1 isl-s-01:5000
            2 isl-s-01:5001
            3 127.0.0.1:2712
         *  @param filePath the Path where The file is stored
         */
        public void readNodes(String filePath)
        {
            string line;
            int counter = 0;
            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader("../../" + filePath);
            while ((line = file.ReadLine()) != null)
            {
                String[] words = line.Split(new char[] { ' ', ':' });
                String ip = words[1];
                int port = Convert.ToInt32(words[2]);
                int id = Convert.ToInt32(words[0]);
                allNodes.Add(new Knoten(id, ip, port));
                counter++;
                //Console.WriteLine(ip);
                // Console.WriteLine(port);
                // Console.WriteLine(id);
            }

            file.Close();

            // Suspend the screen.
        }

        /**
         * Init a this node, adds the neibors and gets the information about hisself
         */
        public void nodeInit()
        {
            if (this.id != 0 && allNodes.Count != 0)
            {
                //search for your own ip and port
                foreach (var info in allNodes)
                {
                    if (info.id == this.id)
                    {
                        //own informations
                        this.ip = info.ip;
                        this.port = info.port;
                    }
                     else if(neighBors.Count < 3) //TODO: das braucht man nicht mehr wenn der alles aus der graphviz datei ausliest
                     {
                         neighBors.Add(info);
                    }
                }
            }
        }

        /*
         * reads a graphviz file and adds the right neighbours
         * Format:
         * graph G {
            1 -- 2;
            3 -- 2;
            4 -- 2;
            4 -- 3;
            5 -- 1;
            5 -- 3;
            }
         */
        internal void readGraph(string path)
        {
         //   for (var i = 0; i < neighBors.Count; i++)
             //Delete the older Items and add the new neighbors
           //     neighBors.Remove(neighBors[i]);
            neighBors = new List<Knoten>();
            string line;
            int counter = 0;
            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader("C:\\Users\\deniz\\Source\\Repos\\AvA\\Knoten\\Knoten/" + path);
            while ((line = file.ReadLine()) != null)
            {
                if (line.EndsWith(";"))
                {
                    line = line.Replace(" -- ", ";");
                    String[] word = line.Split(';');
                    int id1 = Convert.ToInt32(word[0]);
                    int id2 = Convert.ToInt32(word[1]);

                    if (id1 == this.id)
                    {
                        addneigbor(id2);
                    }
                    else if (id2 == this.id)
                    {
                        addneigbor(id1);
                    }
                }

                counter++;
                //Console.WriteLine(ip);
                // Console.WriteLine(port);
                // Console.WriteLine(id);
            }

            file.Close();
        }

        private void addneigbor(int id1)
        {
            foreach (var node in allNodes)
            {
                if (id1 == node.id)
                {
                    Console.WriteLine("nachbar hinzugefuegt, id: " + id1);
                    neighBors.Add(node);
                }

            }
        }
    }
}
