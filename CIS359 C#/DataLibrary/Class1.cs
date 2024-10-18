using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public static class Data
    {

        private static SQLiteConnection sqlite;

        static Data()
        {

            sqlite = new SQLiteConnection("Data Source=C:\\Users\\ppatr\\Desktop\\CIS359 C#\\DataBase\\MyAPI.db");
        }

        public static string GetData(int id)
        {
            string name = string.Empty;
            using (var connection = sqlite)
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"SELECT FirstName, LastName FROM Contact WHERE Contact_ID = $id";
                command.Parameters.AddWithValue("$id", id);

                using (var reader = command.ExecuteReader())
                {
                    while ( (reader.Read()))
                    {
                        name = reader.GetString(0) + " " + reader.GetString(1);
                    }
                }
            }
            return name;
        }

        public static DataTable selectQuery(string query)
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
            }
            catch (SQLiteException ex)
            {
                //Add your exception code here
            }
            sqlite.Close();
            return dt;
        }
    }
}
