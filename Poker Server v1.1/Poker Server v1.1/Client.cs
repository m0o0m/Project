using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Drawing;

namespace Poker_Server_v1._1
{
    class Client
    {
        public TcpClient ClientSocket;
        public string Username;
        private string Session;

        public Thread ClientThread;   

        public bool isAvailable = true;

        public List<string> reservedTables = new List<string>();
        public List<string> sittedTables = new List<string>();// it should be a seat data
            
        public Client(TcpClient ClientSocket, string Username, string Session)
        {
            this.ClientSocket = ClientSocket;
            this.Username = Username;
            this.Session = Session;
        }
        public void Kill()
        {
            Globals.addLog("Client (" + Username + ") Disconnected.", Color.Black);
            ClientSocket.Close();
            //Username = null;
            Session = null;
            isAvailable = false;
        }
        public short decBalance(int amount)
        {

            return 0;
        }
        public short incBalance(int amount)
        {
            return 0;
        }
        public void Start()
        {
            //GameCore.sendPublicData(this);
            //GameCore.sendDataOf(this);
            //run meesage litener
            ClientThread = new Thread(new ThreadStart(delegate() {
                MessageListener listener = new MessageListener(this);
                listener.Run();
            }));
        }
        public void send(string[] dataIndexes, string[] data)
        {
            //client.send(new string[] { "type" , "action" , "name" , "amount" },new string[] { "tableAct" , "bet" , "gholi" , "2000"});
            if (dataIndexes.Length != data.Length)
                return;
            if (data.Length <= 0)
                return;
            string msg = "{\"ms\":{";
            msg += "\"" + dataIndexes[0] + "\":" + "\"" + data[0] + "\"";
            for (int i = 1; i < data.Length; i++)
            {
                msg += ",\"" + dataIndexes[i] + "\":" + "\"" + data[i] + "\"";
            }
            msg += "}}";
            if (this.ClientSocket.Connected)
            {
                NetworkStream stream = this.ClientSocket.GetStream();
                byte[] buffer = Messages.encode(msg);
                stream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                this.Kill();
            }
        }

        //
        //static methods below
        //
        public static void Kill(TcpClient client)
        {
            client.Close();
            return;
        }
        public static IPAddress getIpByTcpClient(TcpClient tcpClient)
        {
            IPEndPoint ipep = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
            return ipep.Address;
        }   
    }
}
