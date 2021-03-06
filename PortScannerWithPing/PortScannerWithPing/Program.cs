﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System;
using System.Net.NetworkInformation;

namespace PortScanner
{
    class Program
    {
        static bool stop = false;
        static int startPort;
        static int endPort;

        static List<int> openPorts = new List<int>();

        static object consoleLock = new object();

        static int waitingForResponses;

        static int maxQueriesAtOneTime = 15000;




        static void Main(string[] args)
        {

            string caseSwitch = "IPAddressTarget";
            IPAddress ipAddress = IPAddress.Parse("0.0.0.0");

        

           while (!stop)
            {

                switch (caseSwitch)
                {

                    case "IPAddressTarget":
                        Console.WriteLine("Enter the IP Address of the target:");
                        string ip = Console.ReadLine();

                        if (!IPAddress.TryParse(ip, out ipAddress))
                        {
                            caseSwitch = "IPAddressTarget";
                            break;
                        }
                        else
                        {
                            caseSwitch = "firstPort";
                            break;
                        }


                    case "firstPort":
                        Console.WriteLine("Enter the starting port to start scanning on:");
                        string sp = Console.ReadLine();

                        if (!int.TryParse(sp, out startPort))
                        {
                            caseSwitch = "firstPort";
                            break;
                        }
                        else
                        {
                            caseSwitch = "lastPort";
                            break;
                        }


                    case "lastPort":
                        Console.WriteLine("Enter the end port:");
                        string ep = Console.ReadLine();

                        if (!int.TryParse(ep, out endPort))
                        {
                            caseSwitch = "lastPort";
                            break;
                        }
                        else
                        {

                            Console.WriteLine("");
                            Console.WriteLine("");
                            Console.WriteLine("");
                            Console.WriteLine("");

                            Console.WriteLine("Press any key to stop scanning...");

                            Console.WriteLine("");
                            Console.WriteLine("");

                            ThreadPool.QueueUserWorkItem(StartScan, ipAddress);

                            Console.ReadKey();

                            stop = true;

                            Console.WriteLine("Press any key to exit...");
                            Console.ReadKey();

                        }
                        break;

                    default:
                        break;
                }
            }

        }


        static void StartScan(object o)
        {
            IPAddress ipAddress = o as IPAddress;

            for (int i = startPort; i < endPort; i++)
            {
                lock (consoleLock)
                {
                    int top = Console.CursorTop;

                    Console.CursorTop = 7;
                    Console.WriteLine("Scanning port: {0}    ", i);

                    Console.CursorTop = top;
                }

                while (waitingForResponses >= maxQueriesAtOneTime)
                    Thread.Sleep(0);

                if (stop)
                    break;

                try
                {
                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    s.BeginConnect(new IPEndPoint(ipAddress, i), EndConnect, s);

                    Interlocked.Increment(ref waitingForResponses);
                }
                catch (Exception)
                {

                }
            }
        }

        static void EndConnect(IAsyncResult ar)
        {
            try
            {
                DecrementResponses();

                Socket s = ar.AsyncState as Socket;

                s.EndConnect(ar);

                if (s.Connected)
                {
                    int openPort = Convert.ToInt32(s.RemoteEndPoint.ToString().Split(':')[1]);

                    openPorts.Add(openPort);

                    lock (consoleLock)
                    {
                        Console.WriteLine("Connected TCP on port: {0}", openPort);
                    }

                    s.Disconnect(true);
                }
            }
            catch (Exception)
            {

            }
        }

        static void IncrementResponses()
        {
            Interlocked.Increment(ref waitingForResponses);

            PrintWaitingForResponses();
        }

        static void DecrementResponses()
        {
            Interlocked.Decrement(ref waitingForResponses);

            PrintWaitingForResponses();
        }

        static void PrintWaitingForResponses()
        {
            lock (consoleLock)
            {
                int top = Console.CursorTop;

                Console.CursorTop = 8;
                Console.WriteLine("Waiting for responses from {0} sockets       ", waitingForResponses);

                Console.CursorTop = top;

           
	          }
            }

        public bool PingTest()
        {
            Ping ping = new Ping();

            PingReply pingStatus = ping.Send(IPAddress.Parse("208.69.34.231"));

                if (pingStatus.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }

        }
    }
}

