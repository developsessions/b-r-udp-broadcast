using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UdpBroadcast
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 4711);
            Console.WriteLine("Nachrichten eingeben, welche als Broadcast versendet werden sollen:");
            CreateUdpReceiver();

            do
            {
                string line = Console.ReadLine();
                byte[] bytes = Encoding.ASCII.GetBytes(line);
                udpClient.Send(bytes, bytes.Length, ip);
            } while (true);
        }

        static void CreateUdpReceiver()
        {
            UdpClient udpClient = new UdpClient(4712);
            CancellationTokenSource source = new CancellationTokenSource();
            Task.Run(async () =>
            {
                while (!source.IsCancellationRequested)
                {
                    var result = await udpClient.ReceiveAsync();
                    ConsoleColor oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("IP: {0} Message: {1}", result.RemoteEndPoint.Address, Encoding.ASCII.GetString(result.Buffer));
                    Console.ForegroundColor = oldColor;
                }
            }, source.Token);
        }
    }
}
