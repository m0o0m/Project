using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_Server_v1._1
{
    enum handRates { Null = 0, HighCard, Pair, TwoPair, ThreeOfKind, Straight, Flush, FullHouse, FourOfKind, RoyalFlush }
    enum CardType { FalseCard = 0, Spade, Heart, Club, Diamond }
    class HandCalculator
    {
        public static Hand CalculateHoldem(int[] flopCards,int[] playerCards)
        {
            int[] flager = new int[4];
            Hand hand = new Hand();
               
            #region Copy to flager
            #region
            foreach (int card in flopCards)
            {
                switch (getCardType(card))
                {
                    case CardType.Club:
                        flager[0] |= 1 << getCardValue(card) - 1;
                        if (getCardValue(card) == 1)
                            flager[0] |= 1 << 13;// 14th bit
                        break; 
                    case CardType.Diamond:
                        flager[1] |= 1 << getCardValue(card) - 1;
                        if (getCardValue(card) == 1)
                            flager[1] |= 1 << 13;// 14th bit
                        break;
                    case CardType.Heart:
                        flager[2] |= 1 << getCardValue(card) - 1;
                        if (getCardValue(card) == 1)
                            flager[2] |= 1 << 13;// 14th bit
                        break;
                    case CardType.Spade:
                        flager[3] |= 1 << getCardValue(card) - 1;
                        if (getCardValue(card) == 1)
                            flager[3] |= 1 << 13;// 14th bit
                        break;
                }
            }
            #endregion
            #region
            foreach (int card in playerCards)
            {
                switch (getCardType(card))
                {
                    case CardType.Club:
                        flager[0] |= 1 << getCardValue(card) - 1;
                        if (getCardValue(card) == 1)
                            flager[0] |= 1 << 13;// 14th bit
                        break;
                    case CardType.Diamond:
                        flager[1] |= 1 << getCardValue(card) - 1;
                        if (getCardValue(card) == 1)
                            flager[1] |= 1 << 13;// 14th bit
                        break;
                    case CardType.Heart:
                        flager[2] |= 1 << getCardValue(card) - 1;
                        if (getCardValue(card) == 1)
                            flager[2] |= 1 << 13;// 14th bit
                        break;
                    case CardType.Spade:
                        flager[3] |= 1 << getCardValue(card) - 1;
                        if (getCardValue(card) == 1)
                            flager[3] |= 1 << 13;// 14th bit
                        break;
                }
            }
            #endregion
            #endregion

            #region Find hand
            int checker;
            int checker1, checker2;
            int i, j ,k ,l;
            int counter;

            #region Is Flash Royal
            //we brute force all possible situation 
            //for a royal flash
            checker = 31744;//1111100000..
            for (i = 0; i < 10; i--)
            {
                for (j = 0; j < 4; j++)
                {
                    if ((checker & flager[j]) == checker)
                    {
                        //yess we have royal flash
                        l = 14 - i;
                        for (k = 0; k < 5; k++)
                        {
                            if (l == 14)
                            {
                                hand.cards[i] = l * (j + 1);
                            }
                        }
                        hand.handRate = handRates.RoyalFlush;
                        return hand;
                    }
                }
                checker <<= 1 ;
                hand.free();
            }
            #endregion

            #region Is Four of Kind
            checker = 16384;//1 << 14 ->            0100000000000000
            for (i =0; (checker >>= 1) != 0; i++)//first be 0010000000000000 
            {
                //this while loop do this works 14 time
                // one time for two section
                if (   ((checker & flager[1]) == checker) 
                    && ((checker & flager[2]) == checker)
                    && ((checker & flager[3]) == checker)
                    && ((checker & flager[4]) == checker))
                {
                    //is a four of kind
                    l = 14 - i;
                    if (l == 14) l = 1;
                    for (i = 0; i < 4; i++)
                        hand.cards[i] = l * (i + 1);
                    //now find a maximum card for fifth card
                    checker1 = 16384;
                    for ( k = 0 ; (checker1 <<= 1) != 1; k++)
                    {
                        for (j = 0; j < 4; j++)
                            if ((checker1 & flager[j]) == checker1 && checker1 != checker )
                            {
                                l = 14 - k;
                                hand.cards[4] = (l * (j + 1));
                            }
                    }

                    hand.handRate = handRates.FourOfKind;
                    return hand;
                }
            }
            #endregion

            #region Is FullHouse
            checker1 = 16384;// 1 << 14
            for(k = 0; ((checker1 <<= 1) != 1 ); k++)
            {
                //count of checker
                int fullhouseCounter = 0;
                for (i = 0; i < 4; i++)
                    if ((checker1 & flager[i]) == checker1)
                    {
                        l = 14 - k;
                        hand.cards[fullhouseCounter] = l * (i + 1) ;
                        fullhouseCounter++;
                    }
                if(fullhouseCounter >= 3)
                {
                    //the first three cards are checker1
                    checker2 = 16384;
                    for (k = 0; (checker2 <<= 1) != 1 ; k++)
                    {
                        fullhouseCounter = 0;
                        for (i = 0; i < 4; i++)
                            if ((checker2 & flager[2]) == checker2)
                            {
                                l = 14 - k;
                                hand.cards[fullhouseCounter] = l * (i + 1);
                                fullhouseCounter++;
                            }
                        if (fullhouseCounter >= 2)
                        {
                            //it is the two cards that we need
                            //and we have a full house here
                            hand.handRate = handRates.FullHouse;
                            return hand;
                        }
                    }
                }//else continue the while
                hand.free();
            }
            #endregion

            #region Is Flash
            int flashCounter;
            for (i = 0; i < 4; i++)
            {
                flashCounter = 0;
                hand.free();
                checker = 16384;
                while ((checker <<= 1) != 1)
                {
                    if ((checker & flager[i]) == checker)
                  {
                        hand.cards[flashCounter] = 0; // here think and complete l4t3r
                        flashCounter++;
                        if (flashCounter >= 5)
                        {
                            //here we have a flash 
                            //and it is the best flash hand
                            break;
                        }
                    }
                }
            }
            #endregion

            #region Is Straight
            checker = 16384;
            int straightCounter = 0;
            while ((checker <<= 1) != 0)
            {
                for (i = 0; i < 4; i++)
                {
                    if ((checker & flager[i]) == checker)
                    {
                        straightCounter++;
                        break;
                    }
                    if (straightCounter == 5)
                    {
                        //here we have an straight
                    }
                    if (i == 3)
                    {
                        //free last 
                        straightCounter = 0;
                        break;
                    }
                }
            }
            #endregion

            #region Is three of kind
            checker1 = 16384;
            int threeCounter;
            while ((checker1 <<= 1) != 0)
            {
                threeCounter = 0;
                for (i = 0; i < 4; i++)
                {
                    if ((flager[i] & checker1) != 0)
                    {
                        threeCounter++;
                        if (threeCounter >= 3)
                        {
                            //here we have three of kind

                            //now we should find two max
                            counter = 0;
                            checker2 = 16384;
                            while ((checker2 <<= 1) != 1)
                            {
                                for (i = 0; i < 4; i++)
                                    if ((checker2 & flager[i]) != checker2)
                                    {
                                        counter++;
                                    }
                                if (counter >= 2)//end
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region is Two pair
            checker = 16384;//10000...
            int counter2 = 0;
            while ((checker <<= 1) != 1)
            {
                counter = 0;
                for (i = 0; i < 4; i++)
                {
                    if ((checker & flager[i]) == checker)
                    {
                        counter++;
                    }
                }
                if (counter == 2)
                {
                    counter2++;
                    if (counter2 == 2)//here we have a two pair
                    {
                        //finding maximums
                        counter = 0;
                        checker2 = 16384;
                        while ((checker2 <<= 1) != 1)
                        {
                            for (i = 0; i < 4; i++)
                                if ((checker2 & flager[i]) != checker2)
                                {
                                    counter++;
                                }
                            if (counter >= 3)//end
                                break;
                        }
                    }
                }
            }
            #endregion

            #region is one pair
            #endregion

            #region choose maximum cards
            checker = 16384;//1000...
            counter = 0;
            while ((checker <<= 1) != 1)
            {
                for(i = 0; i < 4; i++)
                {
                    if ((checker & flager[i]) == checker)
                    {
                        //we should save it here
                        counter++;
                        break;
                    }
                }
                if (counter >= 5)
                    break;
            }
            #endregion
            
            #endregion
              
            return null;
        }
        public static Hand CalculateOmaha(int[] flopCards,int[] playerCards) 
        {
            Hand lastHand = null;
            Hand tempHand = null;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; i < j; j++)
                {
                    tempHand = CalculateHoldem(flopCards, new int[] { playerCards[i], playerCards[j] });
                    if (lastHand == null || (cmpHands(tempHand, lastHand) == 1))
                        lastHand = tempHand;
                }
            }
            return null;
        }
        static int getCardValue(int CardNumber)
        {
            CardType type = getCardType(CardNumber);
            if (type != CardType.FalseCard)
                return (CardNumber % 13) + 1;// 1 to 13
            return 0;// means is false Card
        }
        static CardType getCardType(int CardNumber)
        {
            if (CardNumber >= 1 || CardNumber <= 13)
                return CardType.Spade;
            if (CardNumber >= 14 || CardNumber <= 26)
                return CardType.Heart;
            if (CardNumber >= 27 || CardNumber <= 39)
                return CardType.Club;
            if (CardNumber >= 40 || CardNumber <= 52)
                return CardType.Diamond;
            return CardType.FalseCard;
        }
        public static int cmpHands(Hand hand1,Hand hand2)
        {
            return 0;
        }
    }
    class Hand
    {
        public Hand()
        {
            cards = new int[5];
            free();
        }
        public void free()
        {
            handRate = handRates.Null;
            for (int i = 0; i < 5; i++)
                cards[i] = -1;
        }
        public int[] cards;
        public handRates handRate;
    }
}
