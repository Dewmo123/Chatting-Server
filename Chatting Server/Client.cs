using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class Program
    {

        static void Main(string[] args)
        {
            IPAddress serverIP = IPAddress.Parse(Console.ReadLine());

            IPEndPoint endPoint = new IPEndPoint(serverIP, 3001);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Connect(endPoint);
            Console.WriteLine("이름을 입력하세요");
            string name = Console.ReadLine();
            byte[] buffer = new byte[1024];
            Thread t1 = new Thread(() => receiveMessage(serverSocket));
            t1.Start();
            while (true)
            {
                string input = Console.ReadLine();
                string message = $"[{DateTime.Now}] {name} : {input}";
                if (message != "")
                {
                    int length = Encoding.UTF8.GetBytes(message, 0, message.Length, buffer, 0);
                    serverSocket.Send(buffer, 0, length, SocketFlags.None);
                }
            }
        }
        public static void receiveMessage(Socket socket)
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int length = socket.Receive(buffer);
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                Console.WriteLine(message);
            }
        }
    }
}
