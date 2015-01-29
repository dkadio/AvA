using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Deadlockverwaltung
{
    class Verwaltung
    {
        int port;
        String fileName;
        bool schreibrecht;
        int counter;

        public Verwaltung(int port, String file){
            this.port = port;
            fileName = file;
            schreibrecht = true;
            counter = 0;
        }


        public void ListenForRequest()
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
                int i;
                // Enter the listening loop.
                while (true)
                {
                    Console.Write(Environment.NewLine + "Waiting for a connection... " + counter + Environment.NewLine);

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = listener.AcceptTcpClient();
                   // Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    // Loop to receive all the data sent by the client.

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        //Console.WriteLine("*********new MSG*******");
                        //Console.WriteLine("Received " + DateTime.Now + ": {0}", data);
                       
                        // read the received Data and check if is a controll or a normal msg
                          byte[] msg = System.Text.Encoding.ASCII.GetBytes(checkPermission(data));

                        // Send back a response.
                            stream.Write(msg, 0, msg.Length);
                        //   Console.WriteLine("Sent " + DateTime.Now + ":{0}", data);

                    }
                    
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

        private string checkPermission(string data)
        {
            if (data == fileName)
            {
                //fordert schreibrecht an
                if (schreibrecht)
                {
                    Console.WriteLine("Gewähre schreibrecht");
                    counter++;
                    schreibrecht = false;
                    return "true";
                }
                else
                {
                    Console.WriteLine("verbiete schreibrecht");
                    return "false";
                }
                
            }
            else if (data == fileName + ":not")
            {
                //will schreibrechte auflösen
                if (!schreibrecht)
                {
                    Console.WriteLine("Löse schreibrecht auf");
                    schreibrecht = true;
                    return "false";
                }
                else
                {
                    Console.WriteLine("Löse schreibrecht nicht auf");
                    return "true";
                }
            }
            return "default";
        }
    }
}
