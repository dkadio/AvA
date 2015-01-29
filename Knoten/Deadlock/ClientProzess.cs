using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Deadlock
{
    class ClientProzess
    {

        public String fileA;
        public String fileB;
        public volatile bool rwFileA;
        public volatile bool rwFileB;
        static int portFileA = 5000;
        static int portFileB = 5001;
        public ClientProzess(String fileA, String fileB)
        {
            //starte immer mit fileA 
            this.fileA = fileA;
            this.fileB = fileB;
            rwFileA = false;
            rwFileB = false;
        }

        public bool schreibRechtAnfordern(String datei)
        {
            Console.WriteLine("Ich fordere schreibrecht auf " + datei);
            if (datei == "filea.txt")
            {
                if (Connect(datei, portFileA))
                {
                    Console.WriteLine("Habe schreibrecht erhalten");
                    return true;
                }
                else
                {
                    Console.WriteLine("Habe kein schreibrecht erhalten");
                    return false;
                }
            }
            else
            {
                if (Connect(datei, portFileB))
                {
                    Console.WriteLine("Habe schreibrecht erhalten");
                    return true;
                }
                else
                {
                    Console.WriteLine("Habe kein schreibrecht erhalten");
                    return false;
                }
            }

        }

        public bool schreibRechtAufgaben(String datei)
        {
            Console.WriteLine("Ich löse schreibrecht auf " + datei);
            if (datei == "filea.txt")
            {
                if (!Connect(datei + ":not", portFileA))
                {
                    Console.WriteLine("Habe schreibrecht aufgegeben");
                    return false;
                }
                else
                {
                    Console.WriteLine("hatte keine berechtigung");
                    return true;
                }
            }
            else
            {
                if (!Connect(datei + ":not", portFileB))
                {
                    Console.WriteLine("Habe schreibrecht aufgegeben");
                    return false;
                }
                else
                {
                    Console.WriteLine("hatte keine berechtigung");
                    return true;
                }
            }
        }

        public void inkrementFile(String datei)
        {
            Console.WriteLine("ich inkrementiere " + datei);
        }

        public void dekrementFile(String datei)
        {
            Console.WriteLine("ich dekrementiere " + datei);
        }

        public void changeFileName()
        {
            Console.WriteLine("ich aendere namen");
        }

        public bool Connect(String message, int port)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                TcpClient client = new TcpClient("localhost", port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                //Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                //Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
                return Convert.ToBoolean(responseData);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            return false;
        }
    }
}
