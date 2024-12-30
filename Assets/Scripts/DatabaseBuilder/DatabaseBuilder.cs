using UnityEngine;
using CsvHelper;

using System.IO;
using System.Globalization;
using Mono.Data.Sqlite;
using System.Linq;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
public static class DatabaseBuilder {
    // A collection of functions that build sqlite tables from csv files
    // contains the functionality needed to build the champion database, shop odds, bag sizes, and trait levels + names
    // all game related stuff located in 

    private static string dbName = @"Data Source=Assets\Scripts\Databases\Resources.db";
    private static string championCSVFile = @"Assets\Scripts\DatabaseBuilder\CSVFiles\ChampionDataSheet.csv";
    private static string shopOddsCSVFile = @"Assets\Scripts\DatabaseBuilder\CSVFiles\ShopOdds.csv";
    private static string defaultBagSizesCSVFile = @"Assets\Scripts\DatabaseBuilder\CSVFiles\DefaultBagSizes.csv";
    private static string traitLevelsCSVFile = @"Assets\Scripts\DatabaseBuilder\CSVFiles\TraitLevels.csv";

    public static void generateNewDatabase() { // generate a brand new database if one does not exist yet
        using (var connection = new SqliteConnection(dbName)) {
            connection.Open();
            connection.Close();
        }
    }

    public static void buildChampionTable() { // a function to generate a new champion table based off of a csv file
        List<List<string>> dataPackage = convertCSVFileTo2DList(championCSVFile); // generate 2d list from csv file
        using (var connection = new SqliteConnection(dbName)) { // add to champion table
            connection.Open();
            using (var command = connection.CreateCommand()) { // generate the champion table
                command.CommandText = "CREATE TABLE IF NOT EXISTS Champions (DatabaseID VARCHAR (20), ChampionName VARCHAR (20), Cost VARCHAR (20), Trait1 VARCHAR (20), Trait2 VARCHAR (20), Trait3 VARCHAR (20), ShopIconName VARCHAR(20), ChampionIconName VARCHAR(20));";
                command.ExecuteNonQuery();
            }
            foreach (List<string> championData in dataPackage) {
                using (var command = connection.CreateCommand()) {
                    string formattedValue = generateFormattedValueForSQL(championData);
                    command.CommandText = "INSERT INTO Champions (DatabaseID, ChampionName, Cost, Trait1, Trait2, Trait3, ShopIconName, ChampionIconName) VALUES " + formattedValue; 
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }

    public static void buildShopOdds() {
        List<List<string>> dataPackage = convertCSVFileTo2DList(shopOddsCSVFile); // generate 2d list from csv file
        using (var connection = new SqliteConnection(dbName)) { // add to champion table
            connection.Open();
            using (var command = connection.CreateCommand()) { // generate the champion table
                command.CommandText = "CREATE TABLE IF NOT EXISTS ShopOdds (Levels VARCHAR (20), [1Cost] VARCHAR (20), [2Cost] VARCHAR (20), [3Cost] VARCHAR (20), [4Cost] VARCHAR (20), [5Cost] VARCHAR (20));";
                command.ExecuteNonQuery();
            }
            foreach (List<string> levelData in dataPackage) {
                using (var command = connection.CreateCommand()) {
                    string formattedValue = generateFormattedValueForSQL(levelData);
                    command.CommandText = "INSERT INTO ShopOdds (Levels, [1Cost], [2Cost], [3Cost], [4Cost], [5Cost]) VALUES " + formattedValue; 
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }
    public static void buildDefaultBagSizes() {
        List<List<string>> dataPackage = convertCSVFileTo2DList(defaultBagSizesCSVFile); // generate 2d list from csv file
        using (var connection = new SqliteConnection(dbName)) { // add to champion table
            connection.Open();
            using (var command = connection.CreateCommand()) { // generate the champion table
                command.CommandText = "CREATE TABLE IF NOT EXISTS DefaultBagSizes (Cost VARCHAR (20), BagSize VARCHAR (20));";
                command.ExecuteNonQuery();
            }
            foreach (List<string> bagData in dataPackage) {
                using (var command = connection.CreateCommand()) {
                    string formattedValue = generateFormattedValueForSQL(bagData);
                    command.CommandText = "INSERT INTO DefaultBagSizes (Cost, BagSize) VALUES " + formattedValue; 
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }

    public static void buildTraitLevels() {
        List<List<string>> dataPackage = convertCSVFileTo2DList(traitLevelsCSVFile); // generate 2d list from csv file
        using (var connection = new SqliteConnection(dbName)) { // add to champion table
            connection.Open();
            using (var command = connection.CreateCommand()) { // generate the champion table
                command.CommandText = "CREATE TABLE IF NOT EXISTS TraitLevels (TraitName VARCHAR (20), Tier1 VARCHAR (20), Tier2 VARCHAR (20), Tier3 VARCHAR (20), Tier4 VARCHAR (20), Tier5 VARCHAR (20));";
                command.ExecuteNonQuery();
            }
            foreach (List<string> traitData in dataPackage) {
                using (var command = connection.CreateCommand()) {
                    string formattedValue = generateFormattedValueForSQL(traitData);
                    command.CommandText = "INSERT INTO TraitLevels (TraitName, Tier1, Tier2, Tier3, Tier4, Tier5) VALUES " + formattedValue; 
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }

    private static string generateFormattedValueForSQL(List<string> rowData) { // generate a usable INSERT INTO SQL command from a list of values,
        // NOTE: function must assume that rowData is at least of length 1, look into safeguarding for later
        string formattedValue = "(\'";
        for (int i = 0; i < rowData.Count()-1; i++) { // TAKE EVERY VALUE EXCEPT FOR THE LAST ONE, AS LAST ONE HAS SPECIAL ENDING
            formattedValue += (rowData[i] + "\', \'");
        }
        formattedValue += (rowData[rowData.Count()-1] +  "\')");
        return formattedValue;

    }
    private static List<List<string>> convertCSVFileTo2DList(string csvFileLocation) {
        List<List<string>> dataPackage = new List<List<string>>();
        using (var streamReader = new StreamReader(csvFileLocation)) { // read csv File
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture)) {
                csvReader.Read();
                csvReader.ReadHeader();
                string[] headerRow = csvReader.HeaderRecord;
                while (csvReader.Read()) {
                    List<string> rowData = convertRowToList(csvReader, headerRow);
                    dataPackage.Add(rowData);
                }
            }
        }
        return dataPackage;
    }

    private static List<string> convertRowToList(CsvReader csvReader, string[] headers) { // helper function to generate list of strings from csv row
        List<string> currentList = new List<string>();
        foreach (string header in headers) {
            string headerValue = csvReader.GetField(header);
            if (headerValue != null) {
                currentList.Add(headerValue);
            }
            else {
                currentList.Add("");
            }
        }
        return currentList;
    }

}
