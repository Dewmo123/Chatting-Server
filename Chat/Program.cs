using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat
{
    class Program
    {
        static List<Socket> clientSockets = new List<Socket>();

        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 3001);
            try
            {
                socket.Bind(endPoint);
                Console.WriteLine("서버 오픈");
                socket.Listen(3);
                while (true)
                {
                    if (socket != null)
                    {
                        Socket clientsocket = socket.Accept();
                        new Thread(() => chat(clientsocket)).Start();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private static void chat(Socket socket)
        {
            clientSockets.Add(socket);
            while (true)
            {
                byte[] buffer = new byte[1024];
                int length = socket.Receive(buffer);
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                Console.WriteLine(message);
                foreach (Socket clientSocket in clientSockets)
                {
                    clientSocket.Send(buffer);
                }
            }
        }
    }
}
