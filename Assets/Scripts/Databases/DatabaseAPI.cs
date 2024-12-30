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
    private static string dbName = @"Data Source=Assets\Scripts\Databases\Resources.db";
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
                    for (int i = 1; i <= 3; i++ ) {
                        string currentHeader = "Trait" + i.ToString(); // sweep the row from Trait1 to Trait3
                        if ((string) reader[currentHeader] != "") { // if the trait spot is non-null
                            traitList.Add((string) reader[currentHeader]);
                        }
                    }
                    int a = int.Parse((string) reader["DatabaseID"]);
                    string b = (string) reader["ChampionName"];
                    int c = int.Parse((string) reader["Cost"]);
                    string d = (string) reader["ShopIconName"];
                    string e = (string) reader["ChampionIconName"];
                    return new UnitData(a, b, traitList, c, d, e);
                }
            }
        }

    }
}