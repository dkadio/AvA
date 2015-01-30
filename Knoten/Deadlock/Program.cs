using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace Deadlock
{
    class Program
    {
        static void Main(string[] args)
        {
            bool schreibrechta = false;
            bool schreibrechtb = false;
            if (args.Length != 2 && false)
            {
                Console.WriteLine(".exe filea fileb");
            }
            else
            {
                ClientProzess cp = new ClientProzess(args[0], args[1], Convert.ToInt32(args[2]));
                int pid = Convert.ToInt32(args[2])%2;//Process.GetCurrentProcess().Id % 2;
                int i = 0;

                if (pid == 1)
                {
                    cp.resource.listener.Server.ReceiveTimeout = cp.resource.port-3000;
                    Console.WriteLine("Ich bin ungerader Prozess");
                    while (i < 100)
                    {
                        //starte mit File A
                        if (!schreibrechta)
                        {
                            schreibrechta = cp.schreibRechtAnfordern(cp.fileA);
                        }              
                        if (schreibrechta)
                        {
                            schreibrechtb = cp.schreibRechtAnfordern(cp.fileB);
                            if (!schreibrechtb)
                            {
                                //habe ctrl msg bekommen checke ob ich im deadlock bin
                            }
                        }

                        //wenn ich beide schreibrechte habe dann mache mit dem nächsten schritt weiter
                        if (schreibrechta && schreibrechtb)
                        {
                            cp.inkrementFile(cp.fileA);
                            cp.dekrementFile(cp.fileB);
                            cp.changeFileName();
                            schreibrechta = !cp.schreibRechtAufgaben(cp.fileA);
                            schreibrechtb = !cp.schreibRechtAufgaben(cp.fileB);
                        }


                        i++;
                    }
                }
                else
                {
                    Console.WriteLine("Ich bin gerader Prozess");
                    cp.resource.listener.Server.ReceiveTimeout = cp.resource.port - 1000;
                    while (i < 100)
                    {

                        //starte mit File B
                        if (!schreibrechtb)
                        {
                            schreibrechtb = cp.schreibRechtAnfordern(cp.fileB);
                        }
                        if (schreibrechtb)
                        {
                            schreibrechta = cp.schreibRechtAnfordern(cp.fileA);
                        }


                        //wenn ich beide schreibrechte habe dann mache mit dem nächsten schritt weiter
                        if (schreibrechta && schreibrechtb)
                        {
                            cp.inkrementFile(cp.fileA);
                            cp.dekrementFile(cp.fileB);
                            cp.changeFileName();
                            schreibrechta = !cp.schreibRechtAufgaben(cp.fileA);
                            schreibrechtb = !cp.schreibRechtAufgaben(cp.fileB);
                        }


                        i++;
                    }
                }

            }
            Console.ReadLine();

        }
    }
}
