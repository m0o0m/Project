using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Poker_Server_v1._1
{
    class Database
    {
        private SqlConnection connection;
        private string connectionString;
       
        public Database()
        {
            connectionString = "Server=DESKTOP-1L1I0GO;Database=GoldenDB;Trusted_Connection=True;MultipleActiveResultSets=True;";
            connection = new SqlConnection(connectionString);
            connection.Open();
        }
        public bool installDatabaseColumns()
        {
            try
            {
                string Query;
                Query =
                    "CREATE TABLE Users" +
                    "(" +
                    "ID INT                     NOT NULL IDENTITY(1,1)," +
                    "FIRSTNAME VARCHAR (30)             ," +
                    "LASTNAME VARCHAR (30)              ," +
                    "USERNAME VARCHAR (20)      NOT NULL," +
                    "PASSWORD VARCHAR (64)      NOT NULL," +
                    "EMAIL VARCHAR(128)         NOT NULL," +
                    "COUNTRY VARCHAR(40)        NOT NULL," +
                    "CITY VARCHAR(40)                   ," +
                    "SALT VARCHAR (32)          NOT NULL," +
                    "SESSIONID VARCHAR (64)         NULL," +
                    "MOBILENUMBER VARCHAR (20)          ," +
                    "VERIFYKEY VARCHAR (5)              ," +
                    "VERIFIED INT                       ," +
                    "ONLINE INT                         ," +
                    "PRIMARY KEY (ID)                    " +
                    ");";
                new SqlCommand(Query, connection).BeginExecuteNonQuery();
                Query =
                    "CREATE TABLE CashgameTables" +
                    "(" +
                    "ID INT                     NOT NULL IDENTITY(1,1)," +
                    "TableName VARCHAR (30)     NOT NULL," +
                    "TableID  VARCHAR (30)      NOT NULL," +
                    "TableType VARCHAR (30)     NOT NULL," +
                    "SeatsCount INT                     ," +
                    "BigBlind FLOAT             NOT NULL," +
                    "MaxBuyin FLOAT             NOT NULL," +
                    "MinBuyin FLOAT             NOT NULL," +
                    "PRIMARY KEY (ID)                    " +
                    ");";
                new SqlCommand(Query, connection).BeginExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public bool addTable(string tableId, string tableName,string tableType,string maxBuyin,string minBuyin,string BigBlindStake,int seatsCount)
        {
            string Query = "SELECT tableId FROM CashgameTables WHERE tableId = '" + tableId + "';";
                SqlCommand command = new SqlCommand(Query,connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
                return false;

            Query = String.Format("INSERT INTO CashgameTables ( tableId, tableName, tableType, MaxBuyin, MinBuyin, BigBlind , seatsCount) VALUES" +
                                                             "(   '{0}',     '{1}',     '{2}',    '{3}',    '{4}',          '{5}',   '{6}')",
                                                             tableId,    tableName, tableType, maxBuyin, minBuyin,  BigBlindStake, seatsCount);
            
            command = new SqlCommand(Query,connection);
            command.ExecuteNonQuery();

            return true;
        }
        public SqlDataReader getNLHoldemTables()
        {
            string Query = "SELECT * FROM CashgameTables WHERE tableType = 'HNL';";
            SqlCommand command = new SqlCommand(Query,connection);
            return command.ExecuteReader();
        }
        public SqlDataReader getLHoldemTables()
        {
            string Query = "SELECT * FROM CashgameTables WHERE tableType = 'HL';";
            SqlCommand command = new SqlCommand(Query,connection);
            return command.ExecuteReader();
        }
        public SqlDataReader getNLOmahaTables()
        {
            string Query = "SELECT * FROM CashgameTables WHERE tableType = 'ONL';";
            SqlCommand command = new SqlCommand(Query,connection);
            return command.ExecuteReader();
        }
        public SqlDataReader getLOmahaTables()
        {
            string Query = "SELECT * FROM CashgameTables WHERE tableType = 'OL';";
            SqlCommand command = new SqlCommand(Query,connection);
            return command.ExecuteReader();
        }
        public SqlDataReader getUserBalance(string username)
        {
            string Query = "SELECT balance FROM users WHERE username = '" + username+ "';";
            SqlCommand command = new SqlCommand(Query,connection);
            return command.ExecuteReader();
        }
        public void setUserBalance(int amount)
        {
            string Query = "UPDATE users SET balance = '" + amount +"';";
            SqlCommand command = new SqlCommand(Query,connection);
            command.ExecuteNonQuery();
        }
        public string getUserSession(string username)
        {
            string Query = "SELECT sessionId FROM users WHERE username =" + "'" + username + "'";
            SqlCommand command = new SqlCommand(Query, connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetString(reader.GetOrdinal("sessionId"));
            }
            return null;
        }
    }
}
