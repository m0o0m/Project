using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Poker_Server_v1._1
{
    class ClientManager
    {
        private Database DB;
        private List<Client> clients;
        public ClientManager()
        {
            clients = new List<Client>();
            DB = new Database();
        }
        
        /// <returns>if client is ok it returns the client else returns null</returns>
        public Client addClient(TcpClient ClientSocket)
        {
            try
            {
                byte[] buffer = new byte[128];
                int rcvBytes = 0;

                NetworkStream stream = ClientSocket.GetStream();

                rcvBytes = stream.Read(buffer, 0, 128); //reading the first data from client
                //64 byte for session
                //20 byte for username
                //one byte for seprator
                if (rcvBytes > 85) { Client.Kill(ClientSocket); return null; }
                Array.Resize(ref buffer, rcvBytes);

                string firstConnectionData;
                if (Messages.decode(buffer, out firstConnectionData))
                {
                    string[] temp = firstConnectionData.Split(";".ToCharArray(), 2);
                    if (temp.Length != 2 || temp[0].Length != 32 || temp[1].Length > 20) { Client.Kill(ClientSocket); return null; }
                    //temp[0] is session
                    //temp[1] is username

                    //now we should check session of user
                    string session = DB.getUserSession(temp[1]);
                    if (session == temp[0])
                    {
                        Client newClient = null;
                        Client FindedClient = clients.Find(delegate (Client c) { return c.Username == temp[1]; });
                        if (FindedClient == null)
                        {
                            //client with this username (temp[1]) not exist
                            newClient = new Client(ClientSocket, temp[1], temp[0]);
                            //now add new client to storage
                            clients.Add(newClient);
                            newClient.Start();
                            return newClient;
                        }
                        else //client exist
                        {
                            //client exist with this username temp[1]
                            //now we should kill the last client 
                            //but after it we should store his last tables that he sitted on
                            object rsvTables = FindedClient.reservedTables;
                            object sittedTables = FindedClient.sittedTables;

                            FindedClient.Kill();
                            clients.Remove(FindedClient);
                            //renew the Finded Client
                            newClient = new Client(ClientSocket, temp[1], temp[0]);
                            newClient.reservedTables = (List<string>)rsvTables;  // set last rsv tables data here
                            newClient.sittedTables = (List<string>)sittedTables; //set last tables data here

                            clients.Add(newClient);
                            newClient.Start();
                            return newClient;
                        }
                    }
                }
                //else
                Client.Kill(ClientSocket);
            }

            catch (System.IO.IOException e)
            {
                ClientSocket.Close();
            }
            catch (System.ObjectDisposedException e)
            {
                Client.Kill(ClientSocket);
            }
            return null;
        }
        public void removeClient(string username)
        {
            
        }
    }
}
