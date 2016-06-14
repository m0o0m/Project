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
            connectionString = "Server=DESKTOP-731KBAG;Database=GoldenDB;Trusted_Connection=True;MultipleActiveResultSets=True;";
            connection = new SqlConnection(connectionString);
            connection.Open();
        }
        public bool installDatabaseColumns()
        {
            string Query;
            Query =
                "CREATE TABLE Users" +
                "(" +
                "ID INT                     NOT NULL IDENTITY(1,1)," +
                "FIRSTNAME VARCHAR (30)     NOT NULL," +
                "LASTNAME VARCHAR (30)      NOT NULL," +
                "USERNAME VARCHAR (20)      NOT NULL," +
                "PASSWORD VARCHAR (64)      NOT NULL," +
                "EMAIL VARCHAR(128)         NOT NULL," +
                "COUNTRY VARCHAR(40)                ," +
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
                "SeatsCount INT                     ," +
                "BigBlind INT               NOT NULL," +
                "MaxBuyin INT               NOT NULL," +
                "MinBuyin INT               NOT NULL," +
                "PRIMARY KEY (ID)                    " +
                ");";
            //new SqlCommand(Query, connection).BeginExecuteNonQuery();

            return true;
        }

        public bool addTable(string tableId, string tableName,string tableType,int maxBuyin,int minBuyin,int BigBlindStake,int seatsCount)
        {
            string Query = "SELECT tableId FROM tables WHERE tableId = '" + tableId + "';";
            SqlCommand command = new SqlCommand(Query);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
                return false;

            Query = "INSERT INTO tables ( tableId, tableName, tableType, MaxBuyin, MinBuyin, BigBlindStake , seatsCount) VALUES ( ";
            Query += "";// add values here

            command = new SqlCommand(Query);
            command.ExecuteNonQuery();

            return true;
        }

        public SqlDataReader getNLHoldemTables()
        {
            string Query = "SELECT * FROM tables WHERE tableType = 'NoLimitHoldem';";
            SqlCommand command = new SqlCommand(Query);
            return command.ExecuteReader();
        }
        public SqlDataReader getLHoldemTables()
        {
            string Query = "SELECT * FROM tables WHERE tableType = 'LimitedHoldem';";
            SqlCommand command = new SqlCommand(Query);
            return command.ExecuteReader();
        }
        public SqlDataReader getNLOmahaTables()
        {
            string Query = "SELECT * FROM tables WHERE tableType = 'NoLimitOmaha';";
            SqlCommand command = new SqlCommand(Query);
            return command.ExecuteReader();
        }
        public SqlDataReader getLOmahaTables()
        {
            string Query = "SELECT * FROM tables WHERE tableType = 'LimitOmaha';";
            SqlCommand command = new SqlCommand(Query);
            return command.ExecuteReader();
        }

        public SqlDataReader getUserBalance(string username)
        {
            string Query = "SELECT balance WHERE username = '" + username+ "';";
            SqlCommand command = new SqlCommand(Query);
            return command.ExecuteReader();
        }
        public void setUserBalance(int amount)
        {
            string Query = "UPDATE users SET balance = '" + amount +"';";
            SqlCommand command = new SqlCommand(Query);
            command.ExecuteNonQuery();
        }
        public string getUserSession(string username)
        {
            string Query = "SELECT sessionKey WHERE username =" + "'"+username+"'";
            SqlCommand command = new SqlCommand(Query);
            SqlDataReader reader = command.ExecuteReader();
            return reader.GetString(reader.GetOrdinal("sessionKey"));
        }
    }
}
