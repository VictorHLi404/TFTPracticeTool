using UnityEngine;
using CsvHelper;

using System.IO;
using System.Globalization;
using System.Linq;
using SQLite;
using System;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
public static class DatabaseBuilder
{
    // A collection of functions that build sqlite tables from csv files
    // contains the functionality needed to build the champion database, shop odds, bag sizes, and trait levels + names
    // all game related stuff located in 

    private static string dbName = @"Assets\Scripts\CRUD\BaseGameInformation.db";
    private static string championCSVFile = @"Assets\Scripts\CRUD\CSVFiles\ChampionDataSheet.csv";
    private static string shopOddsCSVFile = @"Assets\Scripts\CRUD\CSVFiles\ShopOdds.csv";
    private static string defaultBagSizesCSVFile = @"Assets\Scripts\CRUD\CSVFiles\DefaultBagSizes.csv";
    private static string traitLevelsCSVFile = @"Assets\Scripts\CRUD\CSVFiles\TraitLevels.csv";
    private static string traitColorsCSVFile = @"Assets\Scripts\CRUD\CSVFiles\TraitColors.csv";
    private static string XPLevelsCSVFile = @"Assets\Scripts\CRUD\CSVFiles\XPLevels.csv";
    private static string itemsFile = @"Assets\Scripts\CRUD\CSVFiles\Items.csv";

    public static void generateNewDatabase()
    { // generate a brand new database if one does not exist yet
        using (var connection = new SQLiteConnection(dbName))
        {
            connection.CreateTable<ChampionDatabaseEntity>();
            connection.CreateTable<ShopOddsDatabaseEntity>();
            connection.CreateTable<DefaultBagSizesDatabaseEntity>();
            connection.CreateTable<TraitLevelsDatabaseEntity>();
            connection.CreateTable<TraitColorsDatabaseEntity>();
            connection.CreateTable<XPLevelsDatabaseEntity>();
            connection.CreateTable<ItemDatabaseEntity>();
 
            connection.Close();
        }
    }

    public static void initializeDatabase()
    {
        generateNewDatabase();
        Debug.Log("Database built!");
        BuildChampionTable();
        Debug.Log("Champion Table generated!");
        BuildShopOdds();
        Debug.Log("Shop Odds generated!");
        BuildDefaultBagSizes();
        Debug.Log("Default Bag Sizes generated!");
        BuildTraitLevels();
        Debug.Log("Trait Levels generated!");
        BuildTraitColors();
        Debug.Log("Trait Colors generated");
        BuildXPLevels();
        Debug.Log("XP levels built!");
        BuildItems();
        Debug.Log("Items built!");
    }

    public static void BuildChampionTable()
    { // a function to generate a new champion table based off of a csv file
        List<List<string>> dataPackage = ConvertCSVFileTo2DList(championCSVFile); // generate 2d list from csv file

        using (var connection = new SQLiteConnection(dbName))
        {
            var databaseId = 1;
            foreach (var championCSVData in dataPackage)
            {
                ChampionEnum championEnum;
                if (!Enum.TryParse(championCSVData[1], out championEnum))
                    Debug.LogError($"Failed to parse {championCSVData[1]} into a ChampionEnum.");

                var championEntity = new ChampionDatabaseEntity
                {
                    ChampionEnum = championEnum,
                    DatabaseId = databaseId,
                    ChampionName = championCSVData[2],
                    Cost = int.TryParse(championCSVData[3], out var v) ? v : throw new Exception($"failed to parse Cost {championCSVData[3]}"),
                    Trait1 = championCSVData[4],
                    Trait2 = championCSVData[5],
                    Trait3 = championCSVData[6],
                    ShopIconName = championCSVData[7],
                    ChampionIconName = championCSVData[8]
                };
                connection.Insert(championEntity);
                databaseId++;
            }
            connection.Close();
        }
    }

    public static void BuildShopOdds()
    {
        List<List<string>> dataPackage = ConvertCSVFileTo2DList(shopOddsCSVFile); // generate 2d list from csv file
        using (var connection = new SQLiteConnection(dbName))
        {
            foreach (var shopOddsCSVData in dataPackage)
            {
                var shopOddsEntity = new ShopOddsDatabaseEntity
                {
                    Level = int.TryParse(shopOddsCSVData[0], out var level) ? level : throw new Exception($"Failed to parse {shopOddsCSVData[0]} as Level."),
                    OneCostOdds = int.TryParse(shopOddsCSVData[1], out var oneCostOdds) ? oneCostOdds : throw new Exception($"Failed to parse {shopOddsCSVData[1]} as OneCostOdds."),
                    TwoCostOdds = int.TryParse(shopOddsCSVData[2], out var twoCostOdds) ? twoCostOdds : throw new Exception($"Failed to parse {shopOddsCSVData[2]} as TwoCostOdds."),
                    ThreeCostOdds = int.TryParse(shopOddsCSVData[3], out var threeCostOdds) ? threeCostOdds : throw new Exception($"Failed to parse {shopOddsCSVData[3]} as ThreeCostOdds."),
                    FourCostOdds = int.TryParse(shopOddsCSVData[4], out var fourCostOdds) ? fourCostOdds : throw new Exception($"Failed to parse {shopOddsCSVData[4]} as FourCostOdds."),
                    FiveCostOdds = int.TryParse(shopOddsCSVData[5], out var fiveCostOdds) ? fiveCostOdds : throw new Exception($"Failed to parse {shopOddsCSVData[5]} as FiveCostOdds.")
                };
                connection.Insert(shopOddsEntity);
            }
            connection.Close();
        }
    }
    public static void BuildDefaultBagSizes()
    {
        List<List<string>> dataPackage = ConvertCSVFileTo2DList(defaultBagSizesCSVFile); // generate 2d list from csv file
        using (var connection = new SQLiteConnection(dbName))
        {
            foreach (var defaultBagSizeCSVData in dataPackage)
            {
                var defaultBagSizeEntity = new DefaultBagSizesDatabaseEntity
                {
                    ChampionCost = int.TryParse(defaultBagSizeCSVData[0], out var championCost) ? championCost : throw new Exception($"Failed to parse {defaultBagSizeCSVData[0]} as ChampionCost."),
                    BagSize = int.TryParse(defaultBagSizeCSVData[1], out var bagSize) ? bagSize : throw new Exception($"Failed to parse {defaultBagSizeCSVData[1]} as BagSize."),
                };
                connection.Insert(defaultBagSizeEntity);
            }
            connection.Close();
        }
    }

    public static void BuildTraitLevels()
    {
        List<List<string>> dataPackage = ConvertCSVFileTo2DList(traitLevelsCSVFile); // generate 2d list from csv file
        using (var connection = new SQLiteConnection(dbName))
        {
            foreach (var traitLevelCSVData in dataPackage)
            {
                var traitLevelsEntity = new TraitLevelsDatabaseEntity
                {
                    TraitName = traitLevelCSVData[0],
                    TierOne = int.TryParse(traitLevelCSVData[1], out var tierOne) ? tierOne : null,
                    TierTwo = int.TryParse(traitLevelCSVData[2], out var tierTwo) ? tierTwo : null,
                    TierThree = int.TryParse(traitLevelCSVData[3], out var tierThree) ? tierThree : null,
                    TierFour = int.TryParse(traitLevelCSVData[4], out var tierFour) ? tierFour : null,
                    TierFive = int.TryParse(traitLevelCSVData[5], out var tierFive) ? tierFive : null,
                    TierSix = int.TryParse(traitLevelCSVData[6], out var tierSix) ? tierSix : null,
                    TierSeven = int.TryParse(traitLevelCSVData[7], out var tierSeven) ? tierSeven : null,
                    TierEight = int.TryParse(traitLevelCSVData[8], out var tierEight) ? tierEight : null,
                    TierNine = int.TryParse(traitLevelCSVData[9], out var tierNine) ? tierNine : null,
                    TierTen = int.TryParse(traitLevelCSVData[10], out var tierTen) ? tierTen : null,
                };
                connection.Insert(traitLevelsEntity);
            }
            connection.Close();
        }
    }

    public static void BuildTraitColors()
    {
        List<List<string>> dataPackage = ConvertCSVFileTo2DList(traitColorsCSVFile);
        using (var connection = new SQLiteConnection(dbName))
        {
            foreach (var traitColorsCSVData in dataPackage)
            {
                var traitColorsEntity = new TraitColorsDatabaseEntity
                {
                    TraitName = traitColorsCSVData[0],
                    TierOne = int.TryParse(traitColorsCSVData[1], out var tierOne) ? tierOne : null,
                    TierTwo = int.TryParse(traitColorsCSVData[2], out var tierTwo) ? tierTwo : null,
                    TierThree = int.TryParse(traitColorsCSVData[3], out var tierThree) ? tierThree : null,
                    TierFour = int.TryParse(traitColorsCSVData[4], out var tierFour) ? tierFour : null,
                    TierFive = int.TryParse(traitColorsCSVData[5], out var tierFive) ? tierFive : null,
                    TierSix = int.TryParse(traitColorsCSVData[6], out var tierSix) ? tierSix : null,
                    TierSeven = int.TryParse(traitColorsCSVData[7], out var tierSeven) ? tierSeven : null,
                    TierEight = int.TryParse(traitColorsCSVData[8], out var tierEight) ? tierEight : null,
                    TierNine = int.TryParse(traitColorsCSVData[9], out var tierNine) ? tierNine : null,
                    TierTen = int.TryParse(traitColorsCSVData[10], out var tierTen) ? tierTen : null,
                };
                connection.Insert(traitColorsEntity);
            }
            connection.Close();
        }
    }

    public static void BuildXPLevels()
    {
        List<List<string>> dataPackage = ConvertCSVFileTo2DList(XPLevelsCSVFile); // generate 2d list from csv file
        using (var connection = new SQLiteConnection(dbName))
        {
            foreach (var XPLevelsCSVData in dataPackage)
            {
                var XPLevelsEntity = new XPLevelsDatabaseEntity
                {
                    Level = int.TryParse(XPLevelsCSVData[0], out var level) ? level : throw new Exception($"Failed to parse {XPLevelsCSVData[0]} as Level"),
                    XPRequirement = int.TryParse(XPLevelsCSVData[1], out var xpRequirement) ? xpRequirement : throw new Exception($"Failed to parse {XPLevelsCSVData[1]} as XPRequirement")
                };
                connection.Insert(XPLevelsEntity);
            }
            connection.Close();
        }
    }

    public static void BuildItems()
    {
        List<List<string>> dataPackage = ConvertCSVFileTo2DList(itemsFile); // generate 2d list from csv file
        using (var connection = new SQLiteConnection(dbName))
        {
            foreach (var itemCSVData in dataPackage)
            {
                var itemEntity = new ItemDatabaseEntity
                {
                    ComponentOne = itemCSVData[0],
                    ComponentTwo = itemCSVData[1],
                    CompletedItem = itemCSVData[2]
                };
                connection.Insert(itemEntity);
            }
            connection.Close();
        }
    }

    private static List<List<string>> ConvertCSVFileTo2DList(string csvFileLocation)
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
                    List<string> rowData = ConvertRowToList(csvReader, headerRow);
                    dataPackage.Add(rowData);
                }
            }
        }
        return dataPackage;
    }

    private static List<string> ConvertRowToList(CsvReader csvReader, string[] headers)
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
