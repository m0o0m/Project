using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Poker_Server_v1._1
{
    class NoLimitHoldem : Table
    {
        public NoLimitHoldem(string TableID, string TableName, int SeatsCount, int MinBuyin, int MaxBuyin, int BigBlind)
        {
            this.tableName = TableName;
            this.tableId = TableID;
            this.seatsCount = SeatsCount;
            this.bigBlind = BigBlind;
            this.minBuyin = MinBuyin;
            this.maxBuyin = MaxBuyin;
            this.playerData = new PlayerData[SeatsCount];
            this.tableData.reservedUserName = new string[SeatsCount];
        }
        public void InitNewGame()
        {
            tableData.shuffle();//shuffling the cards
            tableData.NextCardIndex = 0;

            tableData.pot = 0;// set pot size to zero 

            for (int i = 0; i < seatsCount; i++)
            {
                if (playerData[i] != null)
                    if (!isVacant(i))
                    {
                        playerData[i].isInGame = true;
                        playerData[i].haveMove = true;
                        playerData[i].Cards[0] = tableData.Cards[tableData.NextCardIndex++];
                        playerData[i].Cards[1] = tableData.Cards[tableData.NextCardIndex++];
                        Client c = playerData[i].updateClient(playerData[i].client);
                        c.send(
                            new string[] { "type", "dataType", "c1", "c2", "tableId" },
                            new string[] { "tableData", "ucard", playerData[i].Cards[0].ToString(), playerData[i].Cards[1].ToString(), this.tableId }
                            );
                    }
                    else
                        playerData[i].isInGame = false;
            }
            tableData.toCall = bigBlind;

            tableData.dealerPos = NextPos(tableData.dealerPos);

            //find index of bb an sb
            int sb = NextPos(tableData.dealerPos);
            int bb = NextPos(sb);

            //decrease their chips
            playerData[sb].Chips -= bigBlind / 2;
            tableData.pot += bigBlind / 2;
            this.TableRelease(
                new string[] { "type", "actType", "tableId", "pos", "amount" },
                new string[] { "act", "sb", this.tableId, sb.ToString(), (bigBlind / 2).ToString() }
                );

            playerData[bb].Chips -= bigBlind;
            tableData.pot += bigBlind;
            this.TableRelease(
                new string[] { "type", "actType", "tableId", "pos", "amount" },
                new string[] { "act", "bb", this.tableId, bb.ToString(), bigBlind.ToString() }
                );
            //now set turn to bb because the nextTurn function change it to next player
            tableData.currentPos = bb;

            for (int i = 0; i < 5; i++)
            {
                tableData.FlopCards[i] = tableData.Cards[tableData.NextCardIndex++];
            }
            tableData.tableSituation = TableSituation.preFlop;

            this.TableRelease(
                new string[] { "type", "dataType", "tableId", "dealerPos" },
                new string[] { "tableData", "initnewgame", this.tableId, this.tableData.dealerPos.ToString() }
                );
        }
        public void processWinner()
        {

        }
        public void act()
        {
            while (Server.isRunning())
            {
                Thread.Sleep(100);

                if (tableData.tableSituation == TableSituation.Null)
                {
                    if (PlayersCount() >= 2)
                    {
                        InitNewGame();
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
                else if (tableData.tableSituation == TableSituation.Ended)
                {
                    processWinner();
                    tableData.tableSituation = TableSituation.Null; //it is end of game
                }
                else
                {
                    if (isAnyToAct())
                    {
                        nextTurn();
                        pokerActions playerAction = WaitForAction();
                        processAction(playerAction);
                    }
                    else
                    {
                        if (PlayingPlayerCount() >= 2 && tableData.tableSituation != TableSituation.River)
                        {
                            nextTableSituation();
                        }
                        else
                        {
                            tableData.tableSituation = TableSituation.Ended;
                        }
                    }
                }
            }
        }
    }
    class LimitedHoldem : Table
    {
    }
    class NoLimitOmaha : Table
    {
    }
    class LimitedOmaha : Table
    {
    }
}
