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

        public Knoten(int id, String ip, int port) 
        {
            this.id = id;
            this.ip = ip;
            this.port = port;
            allNodes = new List<Knoten>();
            neighBors = new List<Knoten>();
            end = true;
        }

        public Knoten(){
            allNodes = new List<Knoten>();
            neighBors = new List<Knoten>();
            end = true;
        }

        public Knoten(int id)
        {
            allNodes = new List<Knoten>();
            neighBors = new List<Knoten>();
            this.id = id;
            end = true;
        }

        /**
         * Listen on inc msg, Format 
         *                      "ctrl:controllmsg;msg:message" 
         *                      "sysmsg:normale message"
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
                        Console.WriteLine("Received " + DateTime.Now + ": {0}", data);

                        // read the received Data and check if is a controll or a normal msg
                        readMessage(data);

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }

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

        public void readMessage(String msg)
        {
            if(msg.Contains("ctrl:")){
                Console.WriteLine("ctrl emfpangen");
                if (msg.Contains("ctrl:end"))
                {
                    end = false;
                }
                if (msg.Contains("ctrl:endall"))
                {
                    end = false;
                    foreach (var node in allNodes)
                    {
                        sendMessage(msg);
                    }
                }
            }        
        }

        private void sendMessage(String msg)
        {
            //send a msg to a tcp listener
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
                String[] words = line.Split(' ');
                String[] ipPort = words[1].Split(':');

                String ip = ipPort[0];
                int port = Convert.ToInt32(ipPort[1]);
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
                        // use a newer version with ip adresse
                    }
                    else if(neighBors.Count < 3)
                    {
                        neighBors.Add(info);
                    }
                }
            }
        }
    }
}
