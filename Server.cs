using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using HTTP;
namespace Server
{
    public class TcpServer
    {
        private const int Port = 12345;
        public HTTP_Config config;

        public TcpServer(HTTP_Config config)
        {

            this.config = config;
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, Port);

                server.Start();

                Console.WriteLine($"Server started on {localAddr}:{Port}...");
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    Client_Handler client_Handler;

                    client_Handler = new HTTP.Client_Handler(client, config);
                    

                    client_Handler.handle_stream();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                server?.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}