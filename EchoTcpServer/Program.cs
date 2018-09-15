using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EchoTcpServer
{
    class Program
    {
        public static readonly int Port = 7777;

        static void Main()
        {
            //IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPAddress localAddress = IPAddress.Loopback;

            // string hostName = Dns.GetHostName();
            // IPHostEntry s = Dns.GetHostEntry(hostName);
            // LocalAddress = s.AddressList[0];

            TcpListener serverSocket = new TcpListener(localAddress, Port);
            serverSocket.Start();
            Console.WriteLine("Echo server started on " + localAddress + " port " + Port);
            while (true)
            {
                try
                {
                    TcpClient clientConnection = serverSocket.AcceptTcpClient();
                    Task.Run(() => DoIt(clientConnection));
                }
                catch (SocketException)
                {
                    Console.WriteLine("Socket exception: Will continue working");
                }
            }
        }

        private static void DoIt(TcpClient clientConnection)
        {
            Console.WriteLine("Incoming client " + clientConnection.Client);
            NetworkStream stream = clientConnection.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);
            while (true)
            {
                string request = reader.ReadLine();
                if (string.IsNullOrEmpty(request)) { break; }
                Console.WriteLine("Request: " + request);
                string response = request;
                writer.WriteLine(response);
                writer.Flush();
            }

            clientConnection.Close();
            Console.WriteLine("Socket closed");
        }
    }
}