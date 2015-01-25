using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knoten;
using System.Threading;
using System.Net.Sockets;
using System.Xml.Serialization;
namespace Observer
{

    class Observer
    {
        public volatile int R;
        public volatile int S;
        public int S2;
        public int R2;

        public int id;
        public String ip;
        public int port;
        public bool _end;
        public List<Node> allNodes;
        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.
        private volatile bool _shouldStop;
        public Observer(int id, String ip, int port)
        {
            this.id = id;
            this.ip = ip;
            this.port = port;
            R = 0;
            S = 0;
            S2 = 0;
            R2 = 0;
            allNodes = new List<Node>();
            _end = false;
        }

        public void readNodes(String filePath)
        {
            string line;
            int counter = 0;
            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                String[] words = line.Split(new char[] { ' ', ':' });
                String ip = words[1];
                int port = Convert.ToInt32(words[2]);
                int id = Convert.ToInt32(words[0]);
                String nodetype = words[3];
                allNodes.Add(new Node(id, ip, port));
                counter++;
                //Console.WriteLine(ip);
                // Console.WriteLine(port);
                // Console.WriteLine(id);
            }

            file.Close();

            // Suspend the screen.
        }

        public void DoWork()
        {
            while (!_shouldStop)
            {
                Console.WriteLine("worker thread: working...");
                //sende nachricht an erste node
                Message msg = new Message(this.id, "observe", Message.CONTROLL_MSG);
                foreach (var node in allNodes)
                {
                    sendMessage(msg, node);
                    //empfange nachricht von ihm
                    Listen();
                }
               
                
                //werte s und r aus
                //leg dich schalfen
                Thread.Sleep(10000);
                Console.WriteLine("R: " + R + ", S: " + S + "--> reset");
                resetObserver();
                
               
            }
            Console.WriteLine("worker thread: terminating gracefully.");
        }
        public void RequestStop()
        {
            _shouldStop = true;
        }

        public bool Terminated(){

                return S == R;
           
        }

        public void sendMessage(Message msg, Node node)
        {

            try
            {
                //send a msg to a tcp listener
                TcpClient client = new TcpClient(node.ip, node.port);
                //Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);

                NetworkStream stream = client.GetStream();
                var writer = new XmlSerializer(typeof(Message));
                // Send the message to the connected TcpServer. 
                //stream.Write(data, 0, data.Length);
                Console.WriteLine("Send " + msg.typ + " Msg ##############################");
                Console.WriteLine("Starte Sende " + DateTime.Now);
                writer.Serialize(stream, msg);
                Console.WriteLine("Sent  to " + node.id + ", msg: " + msg.nachricht + ", at " + DateTime.Now);
                stream.Close();
                client.Close();
                Console.WriteLine("Verbindung Beendet " + DateTime.Now + Environment.NewLine);
            }
            catch (SocketException)
            {
                Console.WriteLine(" At Sending the Msg: " + msg.nachricht + ", node " + node.id + " not available " + DateTime.Now);
            }
        }

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

                    Console.Write(Environment.NewLine + "Waiting for a connection... " + Environment.NewLine);

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    XmlSerializer reader = new XmlSerializer(typeof(Message));
                    Message msg = (Message)reader.Deserialize(stream);


                    readMessage(msg);
                    // Shutdown and end connection
                    client.Close();

                



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

        private void readMessage(Message msg)
        {
            String[] sr = msg.nachricht.Split(':');
            S = S + Convert.ToInt32(sr[0]);
            R = R + Convert.ToInt32(sr[1]);
            Console.WriteLine(msg.nachricht);
            Console.WriteLine("R: " + R + ", S: " + S);
        }

        public void resetObserver()
        {
            S = 0;
            R = 0;
        }

        public int getRS()
        {
            return R + S;
        }
    }
}
