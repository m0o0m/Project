using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Poker_Server_v1._1
{
    class Server
    {
        public ServerStatus State;
        
        private string ipaddress;
        private short Port;

        private TcpListener serverTcpListener;
        private Thread listenerThread;

        ClientManager clientManager;

        Core GameCore;

        private bool SendResponse(TcpClient client, ClientHeaderData CData)
        {
            // here we send him response header data
            string response;
            response = "HTTP/1.1 101 \r\n" +
                "Upgrade: websocket\r\n" +
                "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                    SHA1.Create().ComputeHash(
                        Encoding.UTF8.GetBytes(
                                    CData.get("Sec-WebSocket-Key").Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                )
                            )
                        ) + "\r\nConnection: upgrade\r\n\r\n";

            NetworkStream stream = client.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes(response);

            try
            {
                stream.Write(buffer, 0, buffer.Length);
                return true;
            }
            catch (System.IO.IOException)
            {
                Client.Kill(client);
                return false;
            }
        }
        private bool ClientIsValid(string request, out ClientHeaderData CData)
        {
            CData = new ClientHeaderData(1);// it is temporary

            string[] tempLines = request.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] tmp;
            if (tempLines.Length < 1)
                return false;
            CData = new ClientHeaderData(tempLines.Length - 1);
            for (int i = 0; i < tempLines.Length; i++)
            {
                if (i == 0)
                {
                    tmp = tempLines[i].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (tmp[0] != "GET")
                        return false;
                    if (tmp[1][0] != '/')
                        return false;
                }
                else
                {
                    tmp = tempLines[i].Split(":".ToCharArray(), 2, StringSplitOptions.RemoveEmptyEntries);
                    if (!CData.add(tmp[0], tmp[1]))
                        return false;
                }
            }
            return true;
        }
        private void acceptingClients()
        {
            TcpClient TempTcpClinet;
            Globals.addLog("Wait for Clients...", Color.Black);
            
            while (State == ServerStatus.Running )
            {
                Thread.Sleep(100);
                try
                {
                    TempTcpClinet = serverTcpListener.AcceptTcpClient();
                    Globals.addLog("a client with ip " + Client.getIpByTcpClient(TempTcpClinet) + " want to connect to server... ", Color.Black);

                    Thread tempThread = new Thread(new ThreadStart(delegate () {
                        Client tempClient;
                        tempClient = processClient(TempTcpClinet);//client process and end
                    }));
                    tempThread.Start();
                }
                catch (SocketException)
                {
                }
            }
        }
        private Client processClient(TcpClient tcpClient)
        {
            int recv = 0, length = 0;
            string data = "";
            byte[] buffer = new byte[1000];

            NetworkStream Stream;

            try
            {
                Stream = tcpClient.GetStream();

                recv = Stream.Read(buffer, 0, 1000);

                length += recv;
                data += UnicodeEncoding.UTF8.GetString(buffer, 0, length);

                ClientHeaderData CData;

                if (ClientIsValid(data, out CData))
                {
                    if (SendResponse(tcpClient, CData))
                    {
                        Client tempClient;
                        tempClient = clientManager.addClient(tcpClient);
                        Globals.addLog("Client with ip " + Client.getIpByTcpClient(tcpClient) + " Connected successfully. ", Color.Green);
                        return tempClient;
                    }
                }

            }
            catch (System.IO.IOException)
            {
                //Client.Kill(client);
            }
            catch (ObjectDisposedException)
            {
                //Client.Kill(client); 
            }
            catch (System.InvalidOperationException)
            {
                //Client.Kill(client);
            }
            return null;
        }
        public Server()
        {
            GameCore = new Core();
            Globals.GameCore = GameCore;
            Globals.serverStatus = State;
            State = ServerStatus.Stoped;
        }
        public bool Start(string ipaddress, short Port)
        {
            if (State == ServerStatus.Stoped)
            {
                try
                {
                    Globals.addLog("Starting Server...", Color.Black);
                    this.ipaddress = ipaddress;
                    this.Port = Port;

                    GameCore.Start();

                    serverTcpListener = new TcpListener(IPAddress.Parse(this.ipaddress), Port);
                    serverTcpListener.Start();

                    listenerThread = new Thread(new ThreadStart(acceptingClients));
                    listenerThread.Start();//CALL acceptingClients

                    clientManager = new ClientManager();//init client manager 

                    State = ServerStatus.Running;
                    Globals.addLog("Server Started on" + this.ipaddress + ":" + this.Port, Color.Green);

                    return true;
                }
                catch (ArgumentNullException)
                {
                    Globals.addLog("Invalid ip address !", Color.Red);
                }
                catch (FormatException)
                {
                    Globals.addLog("invalid ip address!", Color.Red);
                }
                catch (Exception e)
                {
                    Globals.addLog("Can not start server an error accured! ("+e.Message+")", Color.Red);
                }
            }
            else
            {
                MessageBox.Show("Can not start server again\nServer Running or Paused!");         
            }
            return false;
        }
        public void Remuse()
        {
        }
        public bool Stop()
        {
            serverTcpListener.Stop();
            listenerThread.Abort();
            State = ServerStatus.Stoped;
            Globals.addLog("Server stoped.", Color.Red);
            return true;
        }
        public void Pause()
        {
        }
    }
}
