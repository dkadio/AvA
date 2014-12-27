using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Knoten;
using System.Xml.Serialization;
using System.IO;
namespace Steuerung
{
   class Program
    {   
        //used to send commands on special port with special msg
        static void Main(string[] args)
        {
            
            
            if (args.Length == 0) 
            {
                Console.WriteLine(".exe ctrl/msg msg port");
            }
            else 
            { 
           
            try
            {
              
                
                IFormatter formatter = new BinaryFormatter();
                
                String server = "localhost";
                String message = args[1];
                String msgtype = args[0];
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = Convert.ToInt32(args[2]);
                TcpClient client = new TcpClient(server, port);

                Message msg = new Message(-1, message, msgtype);

                // Translate the passed message into ASCII and store it as a Byte array.
                //Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();
              ///////  formatter.Serialize(stream, new Message(1, "jbasd", "ctrl"));
                // Send the message to the connected TcpServer. 
               // stream.Write(data, 0, data.Length);
                WriteXML(stream, msg);
                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                //  data = new Byte[256];

                // String to store the response ASCII representation.
                //    String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                //   Int32 bytes = stream.Read(data, 0, data.Length);
                //    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                //     Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            }
        }
       
       public static void WriteXML(NetworkStream stream, Message msg)
        {
           // Message msg = new Message(-1, "nachricht", "ctrl");
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Message));
            writer.Serialize(stream, msg);
        }
    }
}
