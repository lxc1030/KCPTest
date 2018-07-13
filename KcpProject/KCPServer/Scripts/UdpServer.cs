using Network_Kcp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KCPServer
{
    public class UdpServer
    {
        private static UdpServer instance;
        public static UdpServer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UdpServer();
                }
                return instance;
            }
            set
            {
                value = instance;
            }
        }



        public static string IP = "192.168.1.110";
        public static int PORT = 12000;

        public Dictionary<IPEndPoint, KCPPlayer> ListUserKCP;


        public KCPSocket k_Socket;


        UdpClient myClient;
        Thread thrRecv;
        public UdpServer()
        {
            ListUserKCP = new Dictionary<IPEndPoint, KCPPlayer>();
        }
        public void Init()
        {
            NetworkDebuger.IsUnity = false;
            NetworkDebuger.EnableLog = true;
            NetworkDebuger.EnableSave = true;
            NetworkDebuger.Log(nameof(NetworkDebuger) + " Init");


            //开启非可靠传输udp监听客户端连接
            IPEndPoint localIpep = new IPEndPoint(IPAddress.Parse(IP), PORT);
            //myClient = new UdpClient(localIpep);
            //thrRecv = new Thread(ConnectReceive);
            //thrRecv.Start(myClient);
            NetworkDebuger.Log(GetType().Name + " Init");
            //kcpSocket 
            //k_Socket = new KCPSocket(PORT, 1, AddressFamily.InterNetwork);


            KCPPlayer p1 = new KCPPlayer();
            //p1.Init(localIpep, new IPEndPoint(IPAddress.Parse("192.168.1.111"), 12345));
            p1.Init(localIpep, new IPEndPoint(IPAddress.Any, 0));

            Thread th = new Thread(SetUpdate);
            th.IsBackground = true;
            th.Start(p1);
            string writeIN = Console.ReadLine();
            Console.WriteLine("？？？？？？？");
        }

        private void ConnectReceive(object obj)
        {
            UdpClient myClient = obj as UdpClient;
            IPEndPoint remoteIpep = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    byte[] bytRecv = myClient.Receive(ref remoteIpep);
                    ThreadPool.QueueUserWorkItem(DealConnect, new object[] { remoteIpep, bytRecv });
                }
                catch (Exception ex)
                {
                    NetworkDebuger.LogException(ex.Message);
                }
            }
        }


        private void DealConnect(object data)
        {
            //object[] all = data as object[];
            //IPEndPoint remoteIpep = all[0] as IPEndPoint;
            //byte[] bytRecv = all[1] as Byte[];
            //string info = Encoding.UTF8.GetString(bytRecv);
            //NetworkDebuger.Log(GetType().Name, "收到地址{0}发来的消息{1}", remoteIpep.ToString(), info);
            //if (info.Contains("ConnectRequest"))
            //{
            //    NetworkDebuger.Log(info);
            //    string userId = "uuu";
            //    KCPPlayer player = SetKCPPlayer(remoteIpep, userId);
            //    lock (ListUserKCP)
            //    {
            //        ListUserKCP.Add(remoteIpep, player);
            //    }
            //    myClient.Send(bytRecv, bytRecv.Length, remoteIpep);
            //}
        }


        //private KCPPlayer SetKCPPlayer(IPEndPoint endPoint, string userId)
        //{
        //    KCPPlayer p1 = new KCPPlayer();
        //    p1.Init(k_Socket, endPoint);
        //    Thread th = new Thread(SetUpdate);
        //    th.IsBackground = true;
        //    th.Start(p1);

        //    return p1;
        //}
        private void SetUpdate(object obj)
        {
            KCPPlayer player = obj as KCPPlayer;
            while (true)
            {
                player.OnUpdate();
                System.Threading.Thread.Sleep(10);
            }
        }
    }
}
