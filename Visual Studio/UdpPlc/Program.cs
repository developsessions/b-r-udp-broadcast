using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UdpPlc
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient udpClient = new UdpClient(4711);
            UdpClient udpClientSend = new UdpClient(4712);
            CancellationTokenSource source = new CancellationTokenSource();
            Task.Run(async () =>
            {
            while (!source.IsCancellationRequested)
            {
                    var result = await udpClient.ReceiveAsync();
                    IPEndPoint ep = result.RemoteEndPoint;
                    ep.Port = 4712;
                    udpClientSend.Send(result.Buffer, result.Buffer.Length, ep);
                    ConsoleColor oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("IP: {0} Message: {1}", result.RemoteEndPoint.Address, Encoding.ASCII.GetString(result.Buffer));
                    Console.ForegroundColor = oldColor;
                }
            }, source.Token);

            Console.WriteLine("Udp Plc Simulator is started ...");
            Console.ReadKey();
        }
    }
}
