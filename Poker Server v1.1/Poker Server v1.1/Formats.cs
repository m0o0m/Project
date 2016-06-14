using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_Server_v1._1
{
    class Formats
    {
    }

    class ClientHeaderData
    {
        public string[] Name;
        public string[] Data;

        private int Index;

        public ClientHeaderData(int x)
        {
            Name = new string[x];
            Data = new string[x];
            Index = 0;
        }
        public bool add(string name, string data)
        {
            //first we should check  --> working
            this.Name[Index] = name;
            this.Data[Index++] = data;
            return true;
        }
        public string get(string name)
        {
            for (int i = 0; i < Index; i++)
                if (Name[i] == name)
                    return Data[i];
            return "";
        }
    }

}
