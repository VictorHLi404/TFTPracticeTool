using UnityEngine;
using CsvHelper;

using System.IO;
using System.Globalization;
using Mono.Data.Sqlite;
using System.Linq;
using System;
using System.Collections.Generic;
public static class DatabaseBuilder
{
    // A collection of functions that build sqlite tables from csv files
    // contains the functionality needed to build the champion database, shop odds, bag sizes, and trait levels + names
    // all game related stuff located in 

    private static string dbName = @"Data Source=Assets\Scripts\CRUD\BaseGameInformation.db";
    private static string championCSVFile = @"Assets\Scripts\CRUD\CSVFiles\ChampionDataSheet.csv";
    private static string shopOddsCSVFile = @"Assets\Scripts\CRUD\CSVFiles\ShopOdds.csv";
    private static string defaultBagSizesCSVFile = @"Assets\Scripts\CRUD\CSVFiles\DefaultBagSizes.csv";
    private static string traitLevelsCSVFile = @"Assets\Scripts\CRUD\CSVFiles\TraitLevels.csv";
    private static string traitColorsCSVFile = @"Assets\Scripts\CRUD\CSVFiles\TraitColors.csv";
    private static string XPLevelsCSVFile = @"Assets\Scripts\CRUD\CSVFiles\XPLevels.csv";
    private static string itemsFile = @"Assets\Scripts\CRUD\CSVFiles\Items.csv";

    public static void generateNewDatabase()
    { // generate a brand new database if one does not exist yet
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            connection.Close();
        }
    }

    public static void initializeDatabase()
    {
        generateNewDatabase();
        Debug.Log("Database built!");
        buildChampionTable();
        Debug.Log("Champion Table generated!");
        buildShopOdds();
        Debug.Log("Shop Odds generated!");
        buildDefaultBagSizes();
        Debug.Log("Default Bag Sizes generated!");
        buildTraitLevels();
        Debug.Log("Trait Levels generated!");
        buildTraitColors();
        Debug.Log("Trait Colors generated");
        buildXPLevels();
        Debug.Log("XP levels built!");
        buildItems();
        Debug.Log("Items built!");
    }

    public static void buildChampionTable()
    { // a function to generate a new champion table based off of a csv file
        List<List<string>> dataPackage = convertCSVFileTo2DList(championCSVFile); // generate 2d list from csv file
        using (var connection = new SqliteConnection(dbName))
        { // add to champion table
            connection.Open();
            using (var command = connection.CreateCommand())
            { // generate the champion table
                command.CommandText = "CREATE TABLE IF NOT EXISTS Champions (DatabaseID VARCHAR (20), ChampionName VARCHAR (20), Cost VARCHAR (20), Trait1 VARCHAR (20), Trait2 VARCHAR (20), Trait3 VARCHAR (20), ShopIconName VARCHAR(20), ChampionIconName VARCHAR(20));";
                command.ExecuteNonQuery();
            }
            foreach (List<string> championData in dataPackage)
            {
                using (var command = connection.CreateCommand())
                {
                    string formattedValue = generateFormattedValueForSQL(championData);
                    command.CommandText = "INSERT INTO Champions (DatabaseID, ChampionName, Cost, Trait1, Trait2, Trait3, ShopIconName, ChampionIconName) VALUES " + formattedValue;
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }

    public static void buildShopOdds()
    {
        List<List<string>> dataPackage = convertCSVFileTo2DList(shopOddsCSVFile); // generate 2d list from csv file
        using (var connection = new SqliteConnection(dbName))
        { // add to champion table
            connection.Open();
            using (var command = connection.CreateCommand())
            { // generate the champion table
                command.CommandText = "CREATE TABLE IF NOT EXISTS ShopOdds (Levels VARCHAR (20), [1Cost] VARCHAR (20), [2Cost] VARCHAR (20), [3Cost] VARCHAR (20), [4Cost] VARCHAR (20), [5Cost] VARCHAR (20));";
                command.ExecuteNonQuery();
            }
            foreach (List<string> levelData in dataPackage)
            {
                using (var command = connection.CreateCommand())
                {
                    string formattedValue = generateFormattedValueForSQL(levelData);
                    command.CommandText = "INSERT INTO ShopOdds (Levels, [1Cost], [2Cost], [3Cost], [4Cost], [5Cost]) VALUES " + formattedValue;
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }
    public static void buildDefaultBagSizes()
    {
        List<List<string>> dataPackage = convertCSVFileTo2DList(defaultBagSizesCSVFile); // generate 2d list from csv file
        using (var connection = new SqliteConnection(dbName))
        { // add to champion table
            connection.Open();
            using (var command = connection.CreateCommand())
            { // generate the champion table
                command.CommandText = "CREATE TABLE IF NOT EXISTS DefaultBagSizes (Cost VARCHAR (20), BagSize VARCHAR (20));";
                command.ExecuteNonQuery();
            }
            foreach (List<string> bagData in dataPackage)
            {
                using (var command = connection.CreateCommand())
                {
                    string formattedValue = generateFormattedValueForSQL(bagData);
                    command.CommandText = "INSERT INTO DefaultBagSizes (Cost, BagSize) VALUES " + formattedValue;
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }

    public static void buildTraitLevels()
    {
        List<List<string>> dataPackage = convertCSVFileTo2DList(traitLevelsCSVFile); // generate 2d list from csv file
        using (var connection = new SqliteConnection(dbName))
        { // add to champion table
            connection.Open();
            using (var command = connection.CreateCommand())
            { // generate the champion table
                command.CommandText = "CREATE TABLE IF NOT EXISTS TraitLevels (TraitName VARCHAR (20), Tier1 VARCHAR (20), Tier2 VARCHAR (20), Tier3 VARCHAR (20), Tier4 VARCHAR (20), Tier5 VARCHAR (20), Tier6 VARCHAR (20), Tier7 VARCHAR (20));";
                command.ExecuteNonQuery();
            }
            foreach (List<string> traitData in dataPackage)
            {
                using (var command = connection.CreateCommand())
                {
                    string formattedValue = generateFormattedValueForSQL(traitData);
                    command.CommandText = "INSERT INTO TraitLevels (TraitName, Tier1, Tier2, Tier3, Tier4, Tier5, Tier6, Tier7) VALUES " + formattedValue;
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }

    public static void buildTraitColors()
    {
        List<List<string>> dataPackage = convertCSVFileTo2DList(traitColorsCSVFile);
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS TraitColors (TraitName VARCHAR (20), Tier1 VARCHAR (20), Tier2 VARCHAR (20), Tier3 VARCHAR (20), Tier4 VARCHAR (20), Tier5 VARCHAR (20), Tier6 VARCHAR (20), Tier7 VARCHAR (20));";
                command.ExecuteNonQuery();
            }
            foreach (List<string> traitData in dataPackage)
            {
                using (var command = connection.CreateCommand())
                {
                    string formattedValue = generateFormattedValueForSQL(traitData);
                    command.CommandText = "INSERT INTO TraitColors (TraitName, Tier1, Tier2, Tier3, Tier4, Tier5, Tier6, Tier7) VALUES " + formattedValue;
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }

    public static void buildXPLevels()
    {
        List<List<string>> dataPackage = convertCSVFileTo2DList(XPLevelsCSVFile); // generate 2d list from csv file
        using (var connection = new SqliteConnection(dbName))
        { // add to champion table
            connection.Open();
            using (var command = connection.CreateCommand())
            { // generate the champion table
                command.CommandText = "CREATE TABLE IF NOT EXISTS XPLevels (Level VARCHAR(20), XPRequirement VARCHAR(20));";
                command.ExecuteNonQuery();
            }
            foreach (List<string> XPData in dataPackage)
            {
                using (var command = connection.CreateCommand())
                {
                    string formattedValue = generateFormattedValueForSQL(XPData);
                    command.CommandText = "INSERT INTO XPLevels (Level, XPRequirement) VALUES " + formattedValue;
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }

    public static void buildItems()
    {
        List<List<string>> dataPackage = convertCSVFileTo2DList(itemsFile); // generate 2d list from csv file
        using (var connection = new SqliteConnection(dbName))
        { // add to champion table
            connection.Open();
            using (var command = connection.CreateCommand())
            { // generate the champion table
                command.CommandText = "CREATE TABLE IF NOT EXISTS Items (Component1 VARCHAR(20), Component2 VARCHAR(20), CompletedItem VARCHAR(20));";
                command.ExecuteNonQuery();
            }
            foreach (List<string> itemData in dataPackage)
            {
                using (var command = connection.CreateCommand())
                {
                    string formattedValue = generateFormattedValueForSQL(itemData);
                    command.CommandText = "INSERT INTO Items (Component1, Component2, CompletedItem) VALUES " + formattedValue;
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }

    private static string generateFormattedValueForSQL(List<string> rowData)
    { // generate a usable INSERT INTO SQL command from a list of values,
        if (rowData.Count < 1)
        {
            throw new ArgumentException("Rowdata must contain at least one element");
        }
        // NOTE: function must assume that rowData is at least of length 1, look into safeguarding for later
        string formattedValue = "(\'";
        for (int i = 0; i < rowData.Count() - 1; i++)
        { // TAKE EVERY VALUE EXCEPT FOR THE LAST ONE, AS LAST ONE HAS SPECIAL ENDING
            formattedValue += rowData[i] + "\', \'";
        }
        formattedValue += rowData[rowData.Count() - 1] + "\')";
        return formattedValue;

    }
    private static List<List<string>> convertCSVFileTo2DList(string csvFileLocation)
    {
        List<List<string>> dataPackage = new List<List<string>>();
        using (var streamReader = new StreamReader(csvFileLocation))
        { // read csv File
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                string[] headerRow = csvReader.HeaderRecord;
                while (csvReader.Read())
                {
                    List<string> rowData = convertRowToList(csvReader, headerRow);
                    dataPackage.Add(rowData);
                }
            }
        }
        return dataPackage;
    }

    private static List<string> convertRowToList(CsvReader csvReader, string[] headers)
    { // helper function to generate list of strings from csv row
        List<string> currentList = new List<string>();
        foreach (string header in headers)
        {
            string headerValue = csvReader.GetField(header);
            if (headerValue != null)
            {
                currentList.Add(headerValue);
            }
            else
            {
                currentList.Add("");
            }
        }
        return currentList;
    }
}
