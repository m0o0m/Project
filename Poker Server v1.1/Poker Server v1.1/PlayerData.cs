using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_Server_v1._1
{
    class PlayerData
    {
        public string Username;
        public Client client;
        public int[] Cards;
        public int Chips;
        public int TimeBank;
        public bool isInGame;
        public bool haveMove;
        public pokerActions lastMove;

        public Hand hand;

        public PlayerData(Client c, int Chips, int timeBank)
        {
            Username = c.Username;
            client = c;
            this.Chips = Chips;
            Cards = new int[2];
            Cards[0] = -1;
            Cards[1] = -1;
            TimeBank = timeBank;
        }

    }
}