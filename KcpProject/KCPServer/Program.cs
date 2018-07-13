using Network_Kcp;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KCPServer
{
    class Program
    {
        private static KCPPlayer p1, p2;
        static void Main(string[] args)
        {
            UdpServer.Instance.Init();
            return;
        }
    }


}
