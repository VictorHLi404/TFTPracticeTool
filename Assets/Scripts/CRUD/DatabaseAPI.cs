using UnityEngine;
using CsvHelper;

using System.IO;
using System.Globalization;
using Mono.Data.Sqlite;
using System.Linq;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using System.Data;
using NUnit.Framework;
using System.Security.Cryptography;

public static class DatabaseAPI {
    // class that reads from databases to provide data as needed.
    // generates data objects (units, traits, etc) 
    private static string dbName = @"Data Source=Assets\Scripts\CRUD\BaseGameInformation.db";

    public static UnitData getUnitData(string championName) {
        // given a champion name (MUST MATCH THAT FOUND INSIDE OF SQL DB STRICTLY), return a UnitData object of that champion.
        using (var connection = new SqliteConnection(dbName)) {
            connection.Open();
            using (var command = connection.CreateCommand()) {
                command.CommandText = "SELECT * FROM Champions WHERE ChampionName=" + "\'" + championName + "\'" + ";";
                using (IDataReader reader = command.ExecuteReader()) { // only 1 possible result given database structure, so just read once
                    reader.Read();
                    List<string> traitList = new List<string>();
                    // compile all traits into a list, ignore if empty
                    for (int i = 1; i <= 3; i++) {
                        string currentHeader = "Trait" + i.ToString(); // sweep the row from Trait1 to Trait3
                        if ((string) reader[currentHeader] != "") { // if the trait spot is non-null
                            traitList.Add((string) reader[currentHeader]);
                        }
                    } // shitty typecasting
                    return new UnitData(int.Parse((string) reader["DatabaseID"]),
                                        (string) reader["ChampionName"],
                                        traitList,
                                        int.Parse((string) reader["Cost"]),
                                        (string) reader["ShopIconName"],
                                        (string) reader["ChampionIconName"]);
                }
            }
        }
    }

    public static List<UnitData> getAllUnitData() {
        /*
        Return a list of all possible UnitDatas found in the database. Open a connection to the database, execute SQL query,
        and create new instances of unitdata.
        */
        List<UnitData> unitDataList = new List<UnitData>();
        using (var connection = new SqliteConnection(dbName)) {
            connection.Open();
            using (var command = connection.CreateCommand()) {
                command.CommandText = "SELECT * FROM Champions;";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                List<string> traitList = new List<string>();
                // compile all traits into a list, ignore if empty
                    for (int i = 1; i <= 3; i++) {
                        string currentHeader = "Trait" + i.ToString(); // sweep the row from Trait1 to Trait3
                        if ((string) reader[currentHeader] != "") { // if the trait spot is non-null
                            traitList.Add((string) reader[currentHeader]);
                        }
                    } // shitty typecasting
                    unitDataList.Add(new UnitData(int.Parse((string) reader["DatabaseID"]),
                                                    (string) reader["ChampionName"],
                                                    traitList, 
                                                    int.Parse((string) reader["Cost"]),
                                                    (string) reader["ShopIconName"],
                                                    (string) reader["ChampionIconName"]));
                }
                reader.Close();
            }
            connection.Close();
        }
        return unitDataList;
    }

    public static int getBagSize(UnitData champion) {
        /*
        Given a UnitData object, extract the cost of the unit and check its shop odds.
        */
        using (var connection = new SqliteConnection(dbName)) {
            connection.Open();
            using (var command = connection.CreateCommand()) {
                command.CommandText = $"SELECT BagSize FROM DefaultBagSizes WHERE Cost={champion.Cost}";
                    using (SqliteDataReader reader = command.ExecuteReader()) { // only 1 possible result given database structure, so just read once
                        reader.Read();
                        return int.Parse((string) reader["BagSize"]);
                    }
            }
        }
    }

    public static Dictionary<int, List<int>> getShopOdds() {
        /*
        Get all shop odds for all levels. Return a dictionary mapping from the level to a list of the odds.
        */
        Dictionary<int, List<int>> shopOddsDictionary = new Dictionary<int, List<int>>();
        using (var connection = new SqliteConnection(dbName)) {
        connection.Open();
        using (var command = connection.CreateCommand()) {
            command.CommandText = $"SELECT * FROM ShopOdds";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                List<int> levelOdds = new List<int>();
                for (int i = 1; i <= 5; i++) {
                    string currentHeader = i.ToString() + "Cost";
                    levelOdds.Add(int.Parse((string) reader[currentHeader]));
                }
                shopOddsDictionary[int.Parse((string) reader["Levels"])] = levelOdds;
            }
            reader.Close();
        }
        return shopOddsDictionary;
    }
    }

}