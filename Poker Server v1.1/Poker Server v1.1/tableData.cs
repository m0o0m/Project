using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_Server_v1._1
{
    class TableData
    {
        public ActionStorage currentAct = new ActionStorage();
        public TableSituation tableSituation;

        public string[] reservedUserName;

        public int dealerPos = 0;

        public float pot;
        public float toCall;

        public int[] Cards;
        public int NextCardIndex = 0;
        public int[] FlopCards = new int[5];

        public int currentPos;
        public void shuffle()
        {
            Random rnd1 = new Random(Guid.NewGuid().GetHashCode());
            // 1  - 13 spade 
            // 14 - 26 heart 
            // 27 - 39 clubs 
            // 39 - 52 diamonds
            for (int i = 0; i < 52; i++)
            {
                Cards[i] = rnd1.Next(1, 52);
            }
        }

        public void releaseTo(Client client,ReleaseType type)
        {
            switch (type)
            {
                case ReleaseType.RELEASE_ALL:
                    break;
            }

        }
    }
}
