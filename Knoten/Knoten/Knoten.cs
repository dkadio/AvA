using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Knoten
{

    class Knoten
    {
        public int id { get; set; }
        public String ip { get; set; }
        public int port { get; set; }
        public List<Knoten> allNodes { get; set; }   //are all information about the other nodes
        public List<Knoten> neighBors { get; set; }
        public Boolean end;
        public Boolean initator;
        public Boolean sendId;

        public Knoten(int id, String ip, int port) 
        {
            this.id = id;
            this.ip = ip;
            this.port = port;
            allNodes = new List<Knoten>();
            neighBors = new List<Knoten>();
            end = true;
            sendId = true;
        }

        public Knoten(){
            allNodes = new List<Knoten>();
            neighBors = new List<Knoten>();
            end = true;
            sendId = true;
        }

        public Knoten(int id)
        {
            allNodes = new List<Knoten>();
            neighBors = new List<Knoten>();
            this.id = id;
            end = true;
            sendId = true;
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

            try{

                // Start listening for client requests.
                listener.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (end)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
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

                    } readMessage(data);

                    // Shutdown and end connection
                    client.Close();
                }
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
        public void readMessage(String msg)
        {
            if (msg.Contains("ctrl#") || msg.Contains("msg#")) { 
            String msgtyp = msg.Split('#')[0];
            msg = msg.Split('#')[1];
            switch(msgtyp){
                case "ctrl":
                    ctrlMsg(msg);
                    break;
                case "msg":
                    normaMsg(msg);
                    break;
            }
            }
        }

        /**
         * Analyse a normal Msg
         */
        private void normaMsg(string msg)
        {
            foreach(var node in neighBors)
            sendMessage(msg, node);
        }

        /**
         * Analyse a Controllmsg
         */
        private void ctrlMsg(string msg)
        {
            switch (msg)
            {
                case "end":
                    end = false;
                    break;
                case "endall":
                    end = false;
                    foreach (var node in allNodes)
                    {
                        if (node.id != id) { 
                           Console.WriteLine("Send msg to id: " + node.id);
                           sendMessage("ctrl#"+msg, node);
                        }
                    }
                    break;
                case "init":
                    initator = true;
                    Console.WriteLine("Knoten " + this.id + " ist jetzt Initiator");
                    break;
            }
        }

        /**
         * Sends a msg to a node
         */
        private void sendMessage(String msg, Knoten node)
        {
            if (sendId) { 
                sendId = false;
                foreach (var n in neighBors) 
                    sendMessage(node.id.ToString(), n);
            }
            try { 
            //send a msg to a tcp listener
            TcpClient client = new TcpClient(node.ip, node.port);
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);

            NetworkStream stream = client.GetStream();

            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent " + DateTime.Now + ":{0}", msg);
            stream.Close();
            client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine("Knoten " + node.id + " nicht vergeben oder nicht erreichbar");
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
               new System.IO.StreamReader("../../"+filePath);
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
                   // else if(neighBors.Count < 3) //TODO: das braucht man nicht mehr wenn der alles aus der graphviz datei ausliest
                   // {
                   //     neighBors.Add(info);
                   // }
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
            string line;
            int counter = 0;
            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader("../../" + path);
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
