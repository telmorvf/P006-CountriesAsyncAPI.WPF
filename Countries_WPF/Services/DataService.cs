using Countries_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading;

namespace Countries_WPF.Services
{
    public class DataService
    {
        private SQLiteConnection connection;

        private SQLiteCommand command;

        private DialogService dialogService;

        /// <summary>
        /// Construtor - it's needed to create the database anda table
        /// Create Directory of the Database and Creates the database/table
        /// </summary>
        public DataService()
        {
            dialogService = new DialogService();

            try
            {
                //create directory of SQLite
                if (!Directory.Exists("Data"))
                {
                    Directory.CreateDirectory("Data");
                }

                //Conection to the SQLite
                var path = @"Data\Countries.sqlite";
                connection = new SQLiteConnection("Data Source=" + path);

                //Open Connection and create the table in SQLite
                connection.Open();

                string sqlcommand = "create table if not exists countries(" +
                    "Name varchar(250), " +
                    "Capital varchar(250), " +
                    "Region varchar(250), " +
                    "Subregion varchar(250), " +
                    "Population int, " +
                    "Gini decimal" +
                    ")";

                command = new SQLiteCommand(sqlcommand, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                dialogService.ShowMessage("Erro DataService", e.Message);
            }
        }

        /// <summary>
        /// Before Send data to SQLite, delete the data
        /// </summary>
        public void DeleteData()
        {
            connection.Open();
            try
            {
                string sql = "delete from countries";
                command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro DeleteData", e.Message);
            }
            connection.Close();
        }

        /// <summary>
        /// After Receive data from API - Load Data From List and send to SQLite Database 
        /// </summary>
        /// <param name="List of countries"></param>
        public void SaveData(List<Country> countries)
        {
            try
            {
                connection.Open();
                string V0, V1, V2, V3, V4, V5;
                foreach (var country in countries)
                {
                    V0 = country.Name == "not available" ? "null" : $"'{country.Name.Replace("'", "''")}'";
                    V1 = country.Capital == "not available" ? "null" : $"'{country.Capital.Replace("'", "''")}'";
                    V2 = country.Region == "not available" ? "null" : $"'{country.Region.Replace("'", "''")}'";
                    V3 = country.Subregion == "not available" ? "null" : $"'{country.Subregion.Replace("'", "''")}'";
                    V4 = country.Population == "not available" ? "null" : $"{(country.Population.Replace(",", "."))}";
                    V5 = country.Gini == "not available" ? "null" : $"{(country.Gini.Replace(",", "."))}";

                    string sql = string.Format(
                        "insert into countries (Name, Capital, Region, Subregion, Population, Gini) " +
                        "values({0},{1},{2},{3},{4},{5})",
                                V0, V1, V2, V3, V4, V5
                        );

                    command = new SQLiteCommand(sql, connection);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro SaveData", e.Message);
            }
        }

        /// <summary>
        /// LOAD LOCAL DATA - If no API Connection, load data from SQLite
        /// </summary>
        /// <returns>List of Countries</returns>
        public List<Country> GetData(IProgress<int> progress, IProgress<string> bandeiras)
        {
            List<Country> countries = new List<Country>();
            try
            {
                string sql = "select Name, Capital, Region, Subregion, Population, Gini from countries";

                // Open Connection and create the table in SQLiteLoaded at
                connection.Open();
                command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    countries.Add(new Country
                    {
                        Name = reader["Name"] != DBNull.Value ? (string)reader["Name"] : null,
                        Capital = reader["Capital"] != DBNull.Value ? (string)reader["Capital"] : null,

                        Region = reader["Region"] != DBNull.Value ? (string)reader["Region"] : null,
                        Subregion = reader["Subregion"] != DBNull.Value ? (string)reader["Subregion"] : null,

                        Population = reader["Population"] != DBNull.Value ? (string)reader["Population"].ToString() : null,
                        Gini = reader["Gini"] != DBNull.Value ? (string)reader["Gini"].ToString() : null
                    });
                }
                connection.Close();

                int n = 1;
                foreach (var country in countries)
                {
                    Thread.Sleep(45);
                    var percentComplete = (n * 100) / countries.Count;
                    progress.Report(percentComplete);
                    n++;

                    bandeiras.Report(country.Bandeira);
                }
                return countries;
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro LocalCountries", e.Message);
                return null;
            }
        }

    }
}
