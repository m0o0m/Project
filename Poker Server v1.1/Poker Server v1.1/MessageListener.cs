using System;
using System.Threading;
using System.Net.Sockets;
using System.Drawing;

namespace Poker_Server_v1._1
{
    class MessageListener
    {
        Client client;
        NetworkStream Stream;
        string Message;
        int Length;
        byte[] buffer;

        public void onMessage(Client client, string msg)
        {
            int index = 0;
            if ((msg[index] != 't' || msg[index] != 'T') && (msg[++index] != '='))
            {
                client.Kill();
                return;
            }
            //now index should be 1
            //maximum length of type is ten character 
            // it means that we should block client if index be more than 12
            index++;
            string temp = "";
            while (msg[index] != ';')
            {
                temp += msg[index++];
                if (index >= 10 || index > msg.Length)
                {
                    client.Kill();
                    return;
                }
            }
            index++;
            //now index is start of details
            string[] tmp;
            switch (temp)
            {

                case "get"://it is a request to get data
                    Globals.DB.getUserSession("adasd");
                    break;
                case "rsv"://reserving a seat
                    tmp = msg.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length != 3) { client.Kill(); return; }
                    int pos = Int32.Parse(tmp[2]);
                    //tmp[1] is table id
                    Globals.GameCore.reserve(client, tmp[1], pos);
                    break;
                case "unrsv":
                    //ex : conn.send("t=sitd;HM008;")
                    tmp = msg.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length != 2) { client.Kill(); return; }
                    //tmp[1] // is table id
                    Globals.GameCore.unreserve(client, tmp[1]);
                    break;
                // there are action of client for on a table
                case "sitd"://sit down
                            //table id , chips to siting down, seat position
                            //ex : conn.send("t=sitd;HM007;2000;2;");
                    tmp = msg.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length != 4) { client.Kill(); return; }
                    int chip = Int32.Parse(tmp[2]);
                    int posTosit = Int32.Parse(tmp[3]);
                    Globals.GameCore.SitDown(client, tmp[1], chip, posTosit);
                    break;
                case "situ"://sit up
                            //table id
                    tmp = msg.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length != 3) { client.Kill(); return; }
                    int position = Int32.Parse(tmp[2]);
                    Globals.GameCore.SitUp(client, tmp[1], position);
                    break;
                case "bet"://betting on a table
                           //
                    break;
                case "check":
                    break;
                case "raise":
                    break;
                case "fold":
                    break;
                case "dlrg"://client send gift to the dealer it decrease a bigblind from his balance
                    break;
                case "emt"://it show an emotion on a table
                    break;
                case "cht"://client send a chat message 
                           //send message on table
                           //table id , message
                           //ex conn.send("t=cht;HM007;message");
                    tmp = msg.Split(new string[] { ";" }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length != 3) { client.Kill(); return; }
                    Globals.GameCore.Message(client, tmp[1], tmp[2]);
                    break;

                //there are action of client on lobby
                case "opt"://client send open table request
                           //we should add him to watchers
                    tmp = msg.Split(new string[] { ";" }, 3, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length != 2) { client.Kill(); return; }
                    Globals.GameCore.sendTableData(client,tmp[1]);
                    Globals.GameCore.addWatcher(client, tmp[1]);//tmp[1] is table id
                    break;
                case "clst"://client close an opened table
                    break;
                case "wtt"://client wait for a table
                    break;
                case "reg"://client register for a game
                    break;

                case "alv"://it is a test to check an alive connection
                    break;
                default:   //bad message client send so we should now kill him
                    client.Kill();
                    return;
            }
        }

        public MessageListener(Client client)
        {
            buffer = new byte[1024];
            this.client = client;
            Stream = this.client.ClientSocket.GetStream();
        }
        public void Run()
        {
            try
            {
                while (client.ClientSocket.Connected)
                {
                    Thread.Sleep(100);
                    Array.Resize(ref buffer, 1024);
                    Length = Stream.Read(buffer, 0, 1024);

                    Array.Resize(ref buffer, Length);
                    if (Messages.decode(buffer, out Message))
                        onMessage(client, Message);
                    else // client sended a bad data
                    {
                        Globals.addLog("Client ("+ client.Username +") with ip "+ Client.getIpByTcpClient(client.ClientSocket) + " send a bad data ",Color.Red);
                        client.Kill();
                        break;
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                Globals.addLog("Client (" + client.Username + ") Disconnect... ", Color.Black);
            }
        }
        
    }
}
