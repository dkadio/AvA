using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Deadlock
{
    public class Resource
    {
        public int port;
        public static int FILEPORT_A = 5000;
        public static int FILEPORT_B = 5001;
        public static int VERWALTUNG_TIMEOUT = 180000;
        public static String HOST = "localhost";
        public TcpListener listener;
        public Resource(int port)
        {
            this.port = port;
            listener = new TcpListener(port);
        }

        public Message Listen()
        {
            //Maybe use newer version
            


            // Start listening for client requests.
            listener.Start();

            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;


            Console.Write(Environment.NewLine + "Waiting for a connection... ");

            // Perform a blocking call to accept requests.
            // You could also user server.AcceptSocket() here.
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Connected!");

            data = null;

            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            XmlSerializer reader = new XmlSerializer(typeof(Message));
            Message msg = (Message)reader.Deserialize(stream);

            // Shutdown and end connection 
            client.Close();
            return msg;


        }

        public void Connect(string ip, int port, Message msg)
        {
            try
            {
                //send a msg to a tcp listener
                TcpClient client = new TcpClient(ip, port);
                //Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);

                NetworkStream stream = client.GetStream();
                var writer = new XmlSerializer(typeof(Message));
                // Send the message to the connected TcpServer. 
                //stream.Write(data, 0, data.Length);
               // Console.WriteLine("Send " + msg.typ + " Msg ##############################");
                writer.Serialize(stream, msg);
                stream.Close();
                client.Close();
            }
            catch (SocketException)
            {
                Console.WriteLine(" At Sending the Msg: " + msg.nachricht + ", prozess nicht erreicht ");
            }
        }



        public  List<int> getPortList(String filePath)
        {
            List<int> list = new List<int>();
             string line;
            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                list.Add(Convert.ToInt32(line));
            }
            return list;
        }
    }
}
