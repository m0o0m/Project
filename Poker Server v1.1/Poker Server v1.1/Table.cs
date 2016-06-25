using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Poker_Server_v1._1
{
    class Table
    {
        public PlayerData[] playerData;

        public Thread tableThread;

        private Client[] Watchers;
        private int watchersCount;

        public TableData tableData;

        public Table()
        {
            Watchers = new Client[0];
            watchersCount = 0;
            tableData = new TableData();
            tableData.Cards = new int[52];
            tableData.tableSituation = TableSituation.Null;
        }
        
        public string tableName { get; protected set; }
        public string tableId { get; protected set; }
        public int seatsCount { get; protected set; }
        public float maxBuyin { get; protected set; }
        public float minBuyin { get; protected set; }
        public float bigBlind { get; protected set; }

        public void nextTurn()
        {
            int newPos = NextPos(tableData.currentPos);
            for (int i = 0; i < PlayersCount(); i++)
            {
                if (!isVacant(newPos))
                    if (playerData[newPos] != null && playerData[newPos].isInGame)
                        if (playerData[newPos].haveMove)
                        {
                            tableData.currentPos = newPos;/// here we should work
                            this.TableRelease(
                                new string[] { "type", "actType", "tableId", "pos" },
                                new string[] { "tableData", "TurnPos", this.tableId, newPos.ToString() }
                                );
                            return;
                        }
                newPos = NextPos(newPos);
            }
        }
        public void nextTableSituation()
        {
            switch (tableData.tableSituation)
            {
                case TableSituation.preFlop:
                    tableData.tableSituation = TableSituation.Flop;
                    //send first three cards
                    this.TableRelease(
                        new string[] { "type", "actType", "tableId",
                            "c1",
                            "c2",
                            "c3"
                        },
                        new string[] { "tableData"  , "flopShow", this.tableId,
                            this.tableData.FlopCards[0].ToString(),
                            this.tableData.FlopCards[1].ToString(),
                            this.tableData.FlopCards[2].ToString()
                        });
                    break;
                case TableSituation.Flop:
                    tableData.tableSituation = TableSituation.Turn;
                    this.TableRelease(
                        new string[] { "type", "actType", "tableId", "ct" },
                        new string[] { "tableData", "turnShow", this.tableId, this.tableData.FlopCards[3].ToString() });
                    //send turn card
                    break;
                case TableSituation.Turn:
                    tableData.tableSituation = TableSituation.River;
                    this.TableRelease(
                        new string[] { "type", "actType", "tableId", "riverCard" },
                        new string[] { "act", "riverShow", this.tableId, this.tableData.FlopCards[4].ToString() });
                    break;
            }
        }
        public bool isAnyToAct()
        {
            if (PlayingPlayerCount() == 1)
                return false;
            for (int i = 0; i < seatsCount; i++)
                if (playerData[i] != null && playerData[i].isInGame)
                    if (playerData[i].haveMove)
                        return true;
            return false;
        }
        public pokerActions WaitForAction()
        {
            tableData.currentAct.action = pokerActions.Null;
            //first time to act
            int ExistTime = 30; // second to for player act
            int timeLeft = 0;
            bool timeBankStarted = false;

            while (ExistTime != 0)
            {
                Thread.Sleep(100);
                timeLeft += 100;//100 milisecond
                if (timeLeft >= 1000)
                {
                    timeLeft = 0;
                    ExistTime--;
                    this.TableRelease(
                        new string[] { "type", "actType", "tableId", "t", "pos" },
                        new string[] { "tableData", "timeExist", this.tableId, ExistTime.ToString(), this.tableData.currentPos.ToString() }
                        );
                }
                if (tableData.currentAct.action != pokerActions.Null)
                {
                    if (tableData.currentAct.action == pokerActions.TimeBank)
                    {
                        //here player started time bank by clicking button
                        timeBankStarted = true;
                        ExistTime = playerData[tableData.currentPos].TimeBank;//bug
                        tableData.currentAct.action = pokerActions.Null;
                        this.TableRelease(
                            new string[] { "type", "actType", "tableId", "t", "pos" },
                            new string[] { "tableData", "timebank", this.tableId, ExistTime.ToString(), this.tableData.currentPos.ToString() }
                            );
                    }
                    else
                    {
                        if (timeBankStarted)
                            playerData[tableData.currentPos].TimeBank = ExistTime;//bug
                        return tableData.currentAct.action;
                    }
                }
            }
            //player didn't do anything... here we can start a disconnect protection // have work mamal
            return pokerActions.TimeOut;
        }
        public void processAction(pokerActions action)
        {
            switch (action)
            {
                case pokerActions.TimeOut:
                    fold(tableData.currentPos);//have work here
                    break;
                case pokerActions.Fold:
                    fold(tableData.currentPos);
                    break;
                case pokerActions.Check:
                    check(tableData.currentPos);
                    break;
                case pokerActions.Call:
                    call(tableData.currentPos);
                    break;
                case pokerActions.Bet:
                    bet(tableData.currentPos, tableData.currentAct.betAmount);
                    break;
                default:
                    break;
            }
        }

        public bool isFull()
        {
            for (int i = 0; i < seatsCount; i++)
                if (playerData[i] == null)
                    return false;
            return true;
        }
        public bool isVacant(int posistion)
        {
            if (playerData[posistion] == null)
                return true;
            return false;
        }
        public string getTableName()
        {
            return tableName;
        }
        public void addWatcher(Client c)
        {
            if (watchersCount > 1000)
            {
                //send an error message
                return;
            }
            if (watchersCount == Watchers.Length)
            {
                watchersCount++;
                Array.Resize(ref Watchers, watchersCount);
                Watchers[watchersCount - 1] = c;
            }
            else if (watchersCount < Watchers.Length)
            {
                int index = -1;
                for (int i = 0; i < Watchers.Length; i++)
                    if (Watchers[i] == null)
                        index = i;
                if (index != -1)
                    Watchers[index] = c;
                else
                    c.Kill();
            }
        }
        public void removeWatcher(Client c)
        {
            int index = -1;
            for (int i = 0; i < Watchers.Length; i++)
                if (Watchers[i] == c)
                    index = i;
            if (index != -1)
            {
                Watchers[index] = null;
                watchersCount--;
            }
        }
        public void TableRelease(string[] indexes, string[] data)
        {
            for (int i = 0; i < watchersCount; i++)
                if (Watchers[i] != null)
                    Watchers[i].send(indexes, data);
        }
        public void sitUp(int pos)
        {
            playerData[pos] = null;
        }
        public bool sitDown(Client c, int chip, int pos)
        {
            if (chip < minBuyin || chip > maxBuyin)
                return false;
            if (!isVacant(pos))
                return false;
            if (!c.isAvailable)
                return false;

            c.reservedTables.Remove(this.tableId);

            int timeBank = 60;
            playerData[pos] = new PlayerData(c, chip, timeBank);

            c.sittedTables.Add(tableId);//add table to client

            TableRelease(
                new string[] { "type", "actType", "username", "chip", "position", "tableId", "timeBank" },
                new string[] { "act", "sitdown", c.Username, chip.ToString(), pos.ToString(), this.tableId, this.playerData[pos].TimeBank.ToString() });
            return true;
        }
        public int PlayersCount()
        {
            int count = 0;
            for (int i = 0; i < seatsCount; i++)
                if (!isVacant(i))
                    count++;
            return count;
        }
        public int PlayingPlayerCount()
        {
            int count = 0;
            for (int i = 0; i < seatsCount; i++)
                if (playerData[i] != null && playerData[i].isInGame)
                    count++;
            return count;
        }
        public int NextPos(int currentPos)
        {
            //return next pos if exist otherwise return it self
            for (int i = currentPos; i < seatsCount; i++)
            {
                if (i <= currentPos)
                    continue;
                if (playerData[i] != null)
                    if (playerData[i].isInGame)
                        return i;
            }
            for (int i = 0; i <= currentPos; i++)
            {
                if (playerData[i] != null)
                    if (playerData[i].isInGame)
                        return i;
            }
            return -1;
        }
        public bool isHisPos(Client c)
        {
            for (int i = 0; i < seatsCount; i++)
                if (playerData[i].client == c)
                    if (tableData.currentPos == i)
                        return true;
            return false;
        }
        public int findPos(Client c)
        {
            for (int i = 0; i < seatsCount; i++)
                if (playerData[i].client == c)
                    return i;
            return -1;
        }
        protected void fold(int pos)
        {
            playerData[pos].isInGame = false;
            playerData[pos].haveMove = false;
            this.TableRelease(
                new string[] { "type", "actType", "tableId", "pos" },
                new string[] { "act", "fold", this.tableId, pos.ToString() }
                );
        }
        protected void check(int pos)
        {
            if (tableData.toCall == 0)
            {
                playerData[pos].haveMove = false;
                this.TableRelease(
                    new string[] { "type", "actType", "tableId", "pos" },
                    new string[] { "act", "check", this.tableId, pos.ToString() }
                    );
            }
        }
        protected void call(int pos)
        {
            //checking process should be later in gamecore
            playerData[pos].Chips -= tableData.toCall;
            Client c = playerData[pos].updateClient();
            c.decBalance(tableData.toCall);
            tableData.pot += tableData.toCall;
            playerData[pos].haveMove = false; // but player is in game
            this.TableRelease(
                new string[] { "type", "actType", "pos", "tableId", "amount" },
                new string[] { "act", "call", pos.ToString(), this.tableId, tableData.toCall.ToString() }
                );
        }
        protected void bet(int pos, int betAmount)
        {
            if (betAmount >= 2 * tableData.toCall)
            {
                playerData[pos].Chips -= betAmount;
                tableData.pot += betAmount;
                playerData[pos].haveMove = false;
                this.TableRelease(
                    new string[] { "type", "actType", "tableId", "pos", "amount" },
                    new string[] { "act", "raise", this.tableId, pos.ToString(), betAmount.ToString() }
                    );
                for (int i = 0; i < seatsCount; i++)
                {
                    if (playerData[i].isInGame)//and
                        if (i != pos)//and
                            if (playerData[i].Chips != 0)
                                playerData[i].haveMove = true;
                }
            }
            else if (tableData.currentAct.betAmount > tableData.toCall)
            {
                playerData[pos].Chips -= betAmount;
                tableData.pot += betAmount;
                playerData[pos].haveMove = false;
                this.TableRelease(
                    new string[] { "type", "actType", "tableId", "pos", "amount" },
                    new string[] { "act", "raise", this.tableId, pos.ToString(), betAmount.ToString() }
                    );
                for (int i = 0; i < seatsCount; i++)
                {
                    if (playerData[i].haveMove)//and
                        if (i != pos)//and
                            if (playerData[i].Chips != 0)
                                playerData[i].haveMove = true;
                }
            }
        }
    }
}

namespace Poker_Server_v1._1
{
    enum TableSituation { Null, preFlop, Flop, Turn, River, Ended }
    enum pokerActions { Null, Check, Bet, Call, Fold, TimeOut, TimeBank }
    class ActionStorage
    {
        public pokerActions action = pokerActions.Null;
        public int pos;
        public int betAmount;
    }
}