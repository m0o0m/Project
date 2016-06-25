using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Data.SqlClient;

namespace Poker_Server_v1._1
{
    class Core
    {
        private static NoLimitHoldem[] NoLimitedHoldemTables;
        private static LimitedHoldem[] LimitedHoldemTables;
        private static NoLimitOmaha[] NoLimitedOmahaTables;
        private static LimitedOmaha[] LimitedOmahaTables;

        private static bool started;

        public Core()
        {
            started = false;
        }
        public void loadGames()
        {
            //here we initialize table and tournoments
            int tableName, tableId, seatsCount, MaxBuyin, MinBuyin, BigBlind;
            int index;

            SqlDataReader reader = Globals.DB.getNLHoldemTables();

            tableName = reader.GetOrdinal("tableName");
            tableId = reader.GetOrdinal("tableId");
            seatsCount = reader.GetOrdinal("seatsCount");
            MaxBuyin = reader.GetOrdinal("MaxBuyin");
            MinBuyin = reader.GetOrdinal("MinBuyin");
            BigBlind = reader.GetOrdinal("BigBlind");
            
            NoLimitedHoldemTables = new NoLimitHoldem[0];
            while (reader.Read())
            {
                Array.Resize(ref NoLimitedHoldemTables, NoLimitedHoldemTables.Length + 1);
                index = NoLimitedHoldemTables.Length - 1;
                NoLimitedHoldemTables[index] = new NoLimitHoldem(
                    reader.GetString(tableId),
                    Convert.ToString(reader.GetValue(tableName)),
                    reader.GetInt32(seatsCount),
                    (float)reader.GetDouble(MinBuyin),
                    (float)reader.GetDouble(MaxBuyin),
                    (float)reader.GetDouble(BigBlind));                      
            }
        }
        public void gameLoopStart()
        {
            for (int i = 0; i < NoLimitedHoldemTables.Length ; i++)
            {
                int temp = i;
                NoLimitedHoldemTables[temp].tableThread = new Thread(new ThreadStart(delegate ()
                {
                    NoLimitedHoldemTables[temp].act();
                }));
                NoLimitedHoldemTables[temp].tableThread.Start();
                Globals.addLog("Table " + NoLimitedHoldemTables[temp].tableName + " thread started.", Color.Green);
            }
            //we do this work for other tables type too
        }
        public void Start()
        {
            if (!started)
            {
                loadGames();
                started = true;
                gameLoopStart();//run an infinite loop for each table
            }
        }
        //private static void updateData()
        //{
        //    // create cash games
        //    string TableName, TableID;
        //    int SeatsCount, LastId = 0, BigBlind, MaxBuyin, MinBuyin;
        //    do
        //    {
                

        //        TableName = DB.get("TableName", "CashgameTables", new string[] { "id", ">", LastId.ToString() });
        //        if (TableName != "")
        //        {
        //            LastId = Int32.Parse(DB.get("id", "CashgameTables", new string[] { "TableName", "=", TableName }));
        //            TableID = DB.get("TableID", "CashgameTables", new string[] { "id", "=", LastId.ToString() });
        //            SeatsCount = Int32.Parse(DB.get("SeatsCount", "CashgameTables", new string[] { "id", "=", LastId.ToString() }));
        //            MaxBuyin = Int32.Parse(DB.get("MaxBuyin", "CashgameTables", new string[] { "id", "=", LastId.ToString() }));
        //            MinBuyin = Int32.Parse(DB.get("MinBuyin", "CashgameTables", new string[] { "id", "=", LastId.ToString() }));
        //            BigBlind = Int32.Parse(DB.get("BigBlind", "CashgameTables", new string[] { "id", "=", LastId.ToString() }));

        //            int last = cashTables.Length;
        //            Array.Resize(ref cashTables, last + 1);
        //            cashTables[last] = new CashGameTable(TableID, TableName, SeatsCount, MinBuyin, MaxBuyin, BigBlind);
        //        }
        //    } while (TableName != "");

        //    // create tournments

        //    // create sits and goes

        //    // add players to tournoments
        //}

        /////////////////////////
        // here we write some function to check data and then call function for each table
        ////////////////////////
        public void SitDown(Client c, string tableId, int chip, int pos)
        {
            if (!c.isAvailable)
                return;
            NoLimitHoldem table = find(tableId);
            if (table == null) { c.Kill(); return; }
            //checking inputs
            if (chip > table.maxBuyin || chip < table.minBuyin)
            {
                c.send(
                new string[] { "type", "msgtype", "message" },
                new string[] { "srvmsg", "badinput", "Please buy chips between Minimum buyin and Maximum buyin. " }
                );
                return;
            }
            if (!table.isVacant(pos))
            {
                c.send(
                new string[] { "type", "msgtype", "message" },
                new string[] { "srvmsg", "error", "Sorry.. Can not sit down here" }
                );
                return;
            }
            if (c.decBalance(chip) == -1)
            {
                c.send(
                new string[] { "type", "msgtype", "message" },
                new string[] { "srvmsg", "error", "Sorry.. You don't have enought balance to sit on table.\n Please deposit and try again." }
                );
                return;
            }
            if (c.sittedTables.Find(delegate (string str) { return str == tableId; }) != null)
            {
                c.send(
                    new string[] { "type", "msgtype", "message" },
                    new string[] { "srvmsg", "error", "Sorry.. You already siited down on this table" }
                    );
                return;
            }
            if (table.tableData.reservedUserName[pos] != c.Username)
            {
                //the player didn't reserved the table after
                c.Kill();
                return;
            }
            table.sitDown(c, chip, pos);
        }
        public void SitUp(Client c, string tableId, int pos)
        {
            NoLimitHoldem table = find("tableId");
            if (table != null)
            {
                c.Kill();
                return;
            }
            if (table.playerData[pos].client != c)
            {
                float addingChip = table.playerData[pos].Chips;
                table.sitUp(pos);
                c.incBalance(addingChip);
            }
        }
        public void Message(Client client, string tableId, string message)
        {
        }
        public void Bet(Client c, string tableId, int betAmount)
        {
            NoLimitHoldem table = find(tableId);

            if (table == null) { c.Kill(); return; }

            if (!table.isHisPos(c)) return;

            if (table.tableData.toCall > betAmount) { c.Kill(); return; }

            table.tableData.currentAct.action = pokerActions.Bet;
            table.tableData.currentAct.betAmount = betAmount;
        }
        public void Fold(Client c, string tableId)
        {
            NoLimitHoldem table = find(tableId);

            if (table == null) { c.Kill(); return; }

            if (!table.isHisPos(c)) return;

            table.tableData.currentAct.action = pokerActions.Fold;

        }
        public void Check(Client c, string tableId)
        {
            NoLimitHoldem table = find(tableId);

            if (table == null) { c.Kill(); return; }

            if (!table.isHisPos(c)) return;

            if (table.tableData.toCall != 0) { c.Kill(); return; }
        }
        public void Call(Client c, string tableId)
        {
            NoLimitHoldem table = find(tableId);

            if (table == null) { c.Kill(); return; }

            if (!table.isHisPos(c)) return;

            table.tableData.currentAct.action = pokerActions.Call;

        }
        public void reserve(Client c, string tableId, int pos)
        {
            //we should start a timer for 30 second after it we should delete him
            NoLimitHoldem table = find(tableId);
            if (table == null)
                return;
            if (pos >= table.seatsCount)
                return;
            if (c.sittedTables.Find(delegate (string str) { return str == tableId; }) != null)
            {
                c.send(
                    new string[] { "type", "msgtype", "message" },
                    new string[] { "srvmsg", "error", "Sorry.. You already siited down on this table" }
                    );
                return;
            }

            object myObj = c.reservedTables.Find(delegate (string str) { return str == tableId; });
            if (myObj != null) // he reserve a seat on this table later or not 
                return;
            if (table.tableData.reservedUserName[pos] != null || !table.isVacant(pos))//some one reserve this seat or seat on it he can not seat here
                return;

            c.reservedTables.Add(tableId);
            table.tableData.reservedUserName[pos] = c.Username;

            ThreadStart timer = new ThreadStart(delegate () {
                table.TableRelease(
                    new string[] { "type", "msgtype", "pos", "username", "tableId" },
                    new string[] { "srvmsg", "rsv", pos.ToString(), c.Username, table.tableId }
                    );
                c.send(
                    new string[] { "type", "msgtype", "pos", "username", "tableId", "minBuyIn", "maxBuyIn" },
                    new string[] { "srvmsg", "rsvShow", pos.ToString(), c.Username, table.tableId, table.minBuyin.ToString(), table.maxBuyin.ToString() }
                    );
                int seconds = 30;
                int counter = 0;
                string checker = c.reservedTables.Find(delegate (string str) { return str == tableId; });
                while (seconds > 0 && checker != null && c.isAvailable)//here i think we have a bug .... check it later mamal
                {
                    checker = c.reservedTables.Find(delegate (string str) { return str == tableId; });
                    Thread.Sleep(100);
                    if (checker == null) break;
                    counter += 100;
                    if (counter == 1000)
                    {
                        seconds--;
                        counter = 0;
                        if (c.reservedTables.Find(delegate (string str) { return str == tableId; }) == null)
                            break;
                        table.TableRelease(
                            new string[] { "type", "msgtype", "pos", "timeExist", "tableId" },
                            new string[] { "srvmsg", "rsvTime", pos.ToString(), seconds.ToString(), table.tableId }
                            );
                    }
                }
                // time out or disconnect or cancel
                table.TableRelease(
                    new string[] { "type", "msgtype", "tableId", "pos" },
                    new string[] { "srvmsg", "ursv", table.tableId, pos.ToString() }
                    );
                c.send(
                    new string[] { "type", "msgtype", "tableId", "pos" },
                    new string[] { "srvmsg", "unrsv", table.tableId, pos.ToString() }
                    );
                c.reservedTables.Remove(tableId);
                table.tableData.reservedUserName[pos] = null;
            });
            Thread th = new Thread(timer);
            th.Start();
        }
        public void unreserve(Client c, string tableId)
        {
            NoLimitHoldem table = find(tableId);
            if (table == null)
                return;
            string test = c.reservedTables.Find(delegate (string str) { return str == tableId; });
            if (test == null)
                return;
            c.reservedTables.Remove(tableId);
            int i = 0;
            for (i = 0; i < table.tableData.reservedUserName.Length; i++)
                if (table.tableData.reservedUserName[i] == c.Username)
                    table.tableData.reservedUserName[i] = null;
            table.TableRelease(
                    new string[] { "type", "msgtype", "pos", "tableId" },
                    new string[] { "srvmsg", "ursv", i.ToString(), table.tableId }
                    );
        }
        public void addWatcher(Client c, string tableId)
        {
            NoLimitHoldem table = find(tableId);
            if (table == null)
                return;
            table.addWatcher(c);
        }
        public NoLimitHoldem find(string tableId)
        {
            foreach (NoLimitHoldem table in NoLimitedHoldemTables)
                if (table.tableId == tableId)
                    return table;
            return null;
        }
    }
}
