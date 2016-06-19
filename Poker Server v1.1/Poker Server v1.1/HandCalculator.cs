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
            checker = 31744;//1111100000...
            for (i = 0; i < 10; i--)
            {
                for (j = 0; j < 4; j++)
                {
                    if ((checker & flager[j]) == checker)
                    {
                        //yes we have royal flash
                        l = 14 - i;
                        for (k = 0; k < 5; k++)
                        {
                            if (l == 14)
                            {
                                hand.cards[i] = l * (j + 1);//j + 1 is card type ( 1 - 4)
                            }
                        }
                        hand.handRate = handRates.RoyalFlush;
                        return hand; //end if we have royal flash
                    }
                }
                checker <<= 1 ;// check next royal flush
                hand.free();//free the hand
            }
            #endregion

            #region Is Four of Kind
            checker = 16384;//1 << 14 ->            0100000000000000
            for (i =0; (checker >>= 1) != 0; i++)//first be 0010000000000000 
            {
                //this while loop do this works 14 time if not find a fourofkind
                // one time for two section

                //check that flager on all cards type is true
                if (   ((checker & flager[1]) == checker) 
                    && ((checker & flager[2]) == checker)
                    && ((checker & flager[3]) == checker)
                    && ((checker & flager[4]) == checker))
                {
                    //is a four of kind
                    //now we should create a hand

                    l = 14 - i;//i is offset from 14th flag... i (0 - 13)
                    if (l == 14) l = 1;// 14 is ace
                    //l is number of card

                    for (i = 0; i < 4; i++)
                        hand.cards[i] = l * (i + 1);// i is card type
                    //now we choosed 4 cards so next we should choose fifth card that it is
                    //maximum card and it is not the value of ' l variable '

                    //now find a maximum card for fifth card
                    checker1 = 16384;//from 14th card we check to 2th card
                    for ( i = 0 ; (checker1 <<= 1) != 1; i++)
                    {
                        for (j = 0; j < 4; j++)
                            if ((checker1 & flager[j]) == checker1 && checker1 != checker)
                            {
                                k = 14 - i;//i is offset and k is card number
                                if (k == 14) l = 1; //14 is ace

                                if (k != l)//k != fourofkind's card number
                                {
                                    hand.cards[4] = l * (j + 1);//j + 1 is card type ( 1 - 4)
                                    hand.handRate = handRates.FourOfKind;
                                    return hand;
                                }
                            }
                    }
                }
            }
            #endregion

            #region Is FullHouse
            checker1 = 16384;// 1 << 14    010000000...
            for(k = 0; ((checker1 >>= 1) != 1 ); k++)// k is offset
            {
                //count of checker
                int fullhouseCounter = 0;
                for (i = 0; i < 4; i++)
                    if ((checker1 & flager[i]) == checker1)
                    {
                        l = 14 - k; // k is offset
                        hand.cards[fullhouseCounter] = l * (i + 1) ;
                        fullhouseCounter++;
                    }
                if(fullhouseCounter == 3)
                {
                    //the first three cards are checker1 and value has been set to l
                    //now we should find a pair
                    checker2 = 16384;// 1 << 14 
                    for (k = 0; (checker2 >>= 1) != 1 ; k++)//k is offset
                    {
                        fullhouseCounter = 0;//set counter to zero

                        for (i = 0; i < 4; i++) // i + 1 is card type 
                            if ((checker2 & flager[i]) == checker2)
                            {
                                l = 14 - k;//k is offset
                                //l is card number
                                hand.cards[fullhouseCounter] = l * (i + 1); // i + 1 is card type
                                fullhouseCounter++;
                            }
                        if (fullhouseCounter >= 2)
                        {
                            //it is the two cards that we need
                            //and we have a full house here
                            //we have five card in the hand.card not more we need
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
                for ( j = 0; (checker <<= 1) != 1; j++) // i is offset
                {
                    if ((checker & flager[i]) == checker)
                  {
                        l = 14 - i;
                        if (l == 14) l = 1; //14 is ace
                        //j is offset
                        //l is card number
                        hand.cards[flashCounter] = l * ( i + 1); // i is card type
                        flashCounter++;
                        if (flashCounter == 5)
                        {
                            //here we have a flash 
                            //and it is the best flash hand
                            hand.handRate = handRates.Flush;
                            return hand;
                        }
                    }
                }
            }
            #endregion

            #region Is Straight
            checker = 16384;
            int straightCounter = 0;
            for (k = 0; (checker <<= 1) != 0; k++)//k is offset
            {
                for (i = 0; i < 4; i++)//i + 1 is card type
                {
                    if ((checker & flager[i]) == checker)
                    {
                        l = 14 - k;//k is offset and l is car number
                        if (l == 14) l = 1;//14 is ace
                        hand.cards[straightCounter++] = l * (i + 1); // i + 1 is card type
                        break;
                    }
                    if (straightCounter == 5)
                    {
                        //here we have an straight
                        hand.handRate = handRates.Straight;
                        return hand;
                    }
                    if (i == 3)
                    {
                        //here the str8 is cutted 
                        //free last 
                        hand.free();
                        straightCounter = 0;
                        break;
                    }
                }
            }
            #endregion

            #region Is three of kind
            checker1 = 16384;// 1 <<= 14 010000..
            int threeCounter;
            for (j = 0; (checker1 >>= 1) != 0; j++)//j is offset
            {
                threeCounter = 0;
                for (i = 0; i < 4; i++)//for each card type
                {
                    if ( (flager[i] & checker1) != 0)
                    {
                        l = 14 - i;
                        if (l == 14) l = 1; // 14 is ace
                        hand.cards[threeCounter++] = l * (i + 1);// i + 1 is card type
                        if (threeCounter == 3)
                        {
                            //here we have three of kind
                            hand.handRate = handRates.ThreeOfKind;

                            //now we should find two max
                            counter = 0;
                            checker2 = 16384; // 1 <<= 14 01000...
                            for (j = 0; (checker2 >>= 1) != 1; j++)//j is offset
                            {
                                for (i = 0; i < 4; i++)//i is card type
                                    if ((checker2 & flager[i]) != 0)
                                    {
                                        l = 14 - j;
                                        if (l == 14) l = 1; // 14 is ace
                                        hand.cards[counter++] = l * (i + 1);//i + 1 is card type and l is card number
                                        if (counter == 2)
                                            return hand;//end
                                    }
                                return hand;//end
                            }
                        }
                    }
                }
            }
            #endregion

            #region is Two pair
            checker = 16384;// 1 <<= 14 010000...
            int counter2 = 0;
            for (j = 0; (checker >>= 1) != 1; j++)// j  is offset
            {
                counter = 0;
                for (i = 0; i < 4; i++)//i + 1 is card type
                {
                    if ((checker & flager[i]) == checker)
                    {
                        l = 14 - j;//j is offset
                        if (l == 14)//14 is ace
                            l = 1;

                        if (counter2 == 0)
                            hand.cards[counter] = l * (i + 1);//i + 1 is card type
                        else if (counter2 == 1)
                            hand.cards[counter + 2] = l * (i + 1);
                        counter++;

                        if (counter == 2)
                        {
                            counter2++;
                            if (counter2 == 2)//here we have a two pair
                            {
                                hand.handRate = handRates.TwoPair;
                                //finding maximum card
                                checker2 = 16384;
                                for (j = 0; (checker2 <<= 1) != 1; j++)//j is offset
                                {
                                    for (i = 0; i < 4; i++)//i + 1 is card type
                                        if ((checker2 & flager[i]) != checker2)
                                        {
                                            l = (14 - j);
                                            if (l == 14)//14 is ace
                                                l = 1;

                                            bool exist = false;
                                            for (k = 0; k < 4; k++)
                                                if (hand.cards[k] == (l * (i + 1)))
                                                    exist = true;
                                            if (!exist)
                                            {
                                                hand.cards[4] = l * (i + 1);//it is max card
                                                return hand;
                                            }
                                        }
                                }
                                return hand;
                            }
                        }
                    }
                }
            }
            #endregion

            #region is one pair
            checker = 16384;//1 << 14 0100..
            for (j = 0; (checker <<= 1) != 1; j++)
            {
                counter = 0;
                for (i = 0; i < 4; i++)
                {
                    if ((checker & flager[i]) == checker)//it exist
                    {
                        l = 14 - j;
                        if (l == 14)
                            l = 1;
                        hand.cards[counter] = l * (i + 1);
                        counter++;
                        if (counter == 2)
                        {
                            hand.handRate = handRates.Pair;
                            //now we should find three maximum card
                            checker = 16384;
                            for (j = 0; (checker <<= 1) != 1; j++)//j is offset
                            {
                                for (i = 0; i < 4; i++)
                                {
                                    if ((checker & flager[i]) == checker)
                                    {
                                        l = 14 - j;
                                        if (l == 14) l = 1;

                                        bool exist = false;
                                        for (k = 0; k < 2; k++)
                                            if (l * (i + 1) == hand.cards[k])
                                            {
                                                exist = true;
                                                break;
                                            }
                                        if (!exist)
                                            hand.cards[counter++] = l * (i + 1);
                                        if (counter == 4)
                                            return hand;
                                    }
                                }
                            }
                            return hand;
                        }
                    } 
                }
            }
            #endregion

            #region choose maximum cards
            checker = 16384;// 1 <<= 14; 01000...
            counter = 0;
            hand.handRate = handRates.HighCard;
            for( j = 0; (checker <<= 1) != 1; j++)//j is offset
            {
                for(i = 0; i < 4; i++)
                {
                    if ((checker & flager[i]) == checker)
                    {
                        l = 14 - j;
                        if (l == 14) l = 1;
                        hand.cards[counter++] = l * (i + 1);                                                 
                        break;
                    }
                }
                if (counter >= 5)
                    return hand;
            }
            #endregion

            #endregion

            return hand;
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

        /// <summary>
        /// compare two hand to each other
        /// </summary>
        /// <returns>
        ///      0 if they are equal
        ///      1 if the first one is better
        ///      2 if the second one is better
        ///     -1 if error accured
        /// </returns>
        public static int cmpHands(Hand hand1,Hand hand2)
        {
            if ((int)hand1.handRate > (int)hand2.handRate)
                return 1;
            if ((int)hand1.handRate < (int)hand2.handRate)
                return 2;
            //the hand rate is equal
            //we should check each hand
            switch (hand1.handRate)
            {
                case handRates.HighCard:
                    break;
                case handRates.Pair:
                    break;
                case handRates.TwoPair:
                    break;
                case handRates.ThreeOfKind:
                    break;
                case handRates.Straight:
                    break;
                case handRates.Flush:
                    break;
                case handRates.FullHouse:
                    break;
                case handRates.FourOfKind:
                    break;
                case handRates.RoyalFlush:
                    break;
            }
            return -1;
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
