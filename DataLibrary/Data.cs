using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace DataLibrary
{
    public static class Data
    {
        private static readonly NLog.Logger _log = 
            NLog.LogManager.GetCurrentClassLogger();

        private static SQLiteConnection sqlite;

        //DEFAULT CONSTRUCTOR
        static Data()
        {
            try 
            {
                sqlite = new SQLiteConnection("Data Source=C:\\Users\\ppatr\\Desktop\\CIS359 C#\\DataBase\\MyAPI.db");
                
                _log.Info("{0}:DataLibrary:Data:{1}", DateTime.Now);
            }
            catch (Exception ex)
            {
                _log.Error("{0}:DataLibrary:Data:An error occurred:{1}",
                    DateTime.Now, ex.Message);
            }
        }

        public static string GetData(int id)
        {
           
            string name = string.Empty;

            //Exception Handling
            try
            {
                using (var connection = sqlite)
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                        @"SELECT FirstName, LastName FROM Contact WHERE Contact_ID = $id";
                    command.Parameters.AddWithValue("$id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while ((reader.Read()))
                        {
                            name = reader.GetString(0) + " " + reader.GetString(1);
                        }
                    }
                }

                _log.Info("{0}:DataLibrary:GetData:{1}", DateTime.Now,id);
            }
            catch (Exception ex)
            {
                _log.Error("{0}:DataLibrary:GetData:An error occurred:{1}",
                    DateTime.Now, ex.Message);
            }

            return name;
        }

        public static DataTable SelectQuery(string query)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                sqlite.Open(); // Open connection to the db
                cmd = sqlite.CreateCommand();
                cmd.CommandText = query; // Set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); // Fill the datatable

                _log.Info("{0}:DataLibrary:selectQuery:{1}", DateTime.Now, query);
            }
            catch (SQLiteException ex)
            {
                //Add your exception code here
                _log.Error("{0}:DataLibrary:selectQuery:An error occurred:{1}",
                    DateTime.Now, ex.Message);
            }
            sqlite.Close();
            return dt;
        }

        public static SQLiteDataReader GetDataReader(string strSQL)
        {
            SQLiteDataReader sldr;
            SQLiteCommand cmd;

            sqlite.Open();  // Open connection to the db
            cmd = sqlite.CreateCommand();
            cmd.CommandText = strSQL;  // Set the passed query
            sldr = cmd.ExecuteReader();

            return sldr;
        }

    }
}
