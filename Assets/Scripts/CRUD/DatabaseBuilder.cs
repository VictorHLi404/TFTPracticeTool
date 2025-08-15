using UnityEngine;
using CsvHelper;
using Cysharp.Threading.Tasks;
using System.IO;
using System.Globalization;
using SQLite;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public static class DatabaseBuilder
{
    // A collection of functions that build sqlite tables from csv files
    // contains the functionality needed to build the champion database, shop odds, bag sizes, and trait levels + names
    // all game related stuff located in 

    private static string dbName = @":memory:";
    public static SQLiteConnection connection;
    public static bool databaseHasBeenBuilt = false;
    private static string championCSVFile = @"CSVFiles\ChampionDataSheet.csv";
    private static string shopOddsCSVFile = @"CSVFiles\ShopOdds.csv";
    private static string defaultBagSizesCSVFile = @"CSVFiles\DefaultBagSizes.csv";
    private static string traitLevelsCSVFile = @"CSVFiles\TraitLevels.csv";
    private static string traitColorsCSVFile = @"CSVFiles\TraitColors.csv";
    private static string XPLevelsCSVFile = @"CSVFiles\XPLevels.csv";
    private static string itemsFile = @"CSVFiles\Items.csv";

    public static void generateNewDatabase()
    { // generate a brand new database if one does not exist yet
        connection = new SQLiteConnection(dbName);
        connection.CreateTable<ChampionDatabaseEntity>();
        connection.CreateTable<ShopOddsDatabaseEntity>();
        connection.CreateTable<DefaultBagSizesDatabaseEntity>();
        connection.CreateTable<TraitLevelsDatabaseEntity>();
        connection.CreateTable<TraitColorsDatabaseEntity>();
        connection.CreateTable<XPLevelsDatabaseEntity>();
        connection.CreateTable<ItemDatabaseEntity>();
    }

    public static async UniTask initializeDatabase()
    {
        if (!databaseHasBeenBuilt)
        {
            Debug.Log("Starting to build database...");
            generateNewDatabase();
            Debug.Log("Database built!");
            await BuildChampionTable();
            Debug.Log("Champion Table generated!");
            await BuildShopOdds();
            Debug.Log("Shop Odds generated!");
            await BuildDefaultBagSizes();
            Debug.Log("Default Bag Sizes generated!");
            await BuildTraitLevels();
            Debug.Log("Trait Levels generated!");
            await BuildTraitColors();
            Debug.Log("Trait Colors generated");
            await BuildXPLevels();
            Debug.Log("XP levels built!");
            await BuildItems();
            Debug.Log("Items built!");
            databaseHasBeenBuilt = true;
        }
    }

    public static async UniTask BuildChampionTable()
    { // a function to generate a new champion table based off of a csv file
        var connection = DatabaseBuilder.connection;
        List<List<string>> dataPackage = await ConvertCSVFileTo2DList(championCSVFile); // generate 2d list from csv file

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
    }

    public static async UniTask BuildShopOdds()
    {
        var connection = DatabaseBuilder.connection;
        List<List<string>> dataPackage = await ConvertCSVFileTo2DList(shopOddsCSVFile); // generate 2d list from csv file
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
    }
    public static async UniTask BuildDefaultBagSizes()
    {
        var connection = DatabaseBuilder.connection;
        List<List<string>> dataPackage = await ConvertCSVFileTo2DList(defaultBagSizesCSVFile); // generate 2d list from csv file
        foreach (var defaultBagSizeCSVData in dataPackage)
        {
            var defaultBagSizeEntity = new DefaultBagSizesDatabaseEntity
            {
                ChampionCost = int.TryParse(defaultBagSizeCSVData[0], out var championCost) ? championCost : throw new Exception($"Failed to parse {defaultBagSizeCSVData[0]} as ChampionCost."),
                BagSize = int.TryParse(defaultBagSizeCSVData[1], out var bagSize) ? bagSize : throw new Exception($"Failed to parse {defaultBagSizeCSVData[1]} as BagSize."),
            };
            connection.Insert(defaultBagSizeEntity);
        }
    }

    public static async UniTask BuildTraitLevels()
    {
        var connection = DatabaseBuilder.connection;
        List<List<string>> dataPackage = await ConvertCSVFileTo2DList(traitLevelsCSVFile); // generate 2d list from csv file
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
    }

    public static async UniTask BuildTraitColors()
    {
        var connection = DatabaseBuilder.connection;
        List<List<string>> dataPackage = await ConvertCSVFileTo2DList(traitColorsCSVFile);
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
    }

    public static async UniTask BuildXPLevels()
    {
        var connection = DatabaseBuilder.connection;
        List<List<string>> dataPackage = await ConvertCSVFileTo2DList(XPLevelsCSVFile); // generate 2d list from csv file
        foreach (var XPLevelsCSVData in dataPackage)
        {
            var XPLevelsEntity = new XPLevelsDatabaseEntity
            {
                Level = int.TryParse(XPLevelsCSVData[0], out var level) ? level : throw new Exception($"Failed to parse {XPLevelsCSVData[0]} as Level"),
                XPRequirement = int.TryParse(XPLevelsCSVData[1], out var xpRequirement) ? xpRequirement : throw new Exception($"Failed to parse {XPLevelsCSVData[1]} as XPRequirement")
            };
            connection.Insert(XPLevelsEntity);
        }
    }

    public static async UniTask BuildItems()
    {
        var connection = DatabaseBuilder.connection;
        List<List<string>> dataPackage = await ConvertCSVFileTo2DList(itemsFile); // generate 2d list from csv file
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
    }

    private static async UniTask<List<List<string>>> ConvertCSVFileTo2DList(string csvFileLocation)
    {
        List<List<string>> dataPackage = new List<List<string>>();
        using (var streamReader = new StringReader(await LoadCsvFile(csvFileLocation)))
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

    private static async UniTask<string> LoadCsvFile(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        UnityWebRequest request = UnityWebRequest.Get(filePath);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load CSV: " + request.error);
        }

        string csvData = request.downloadHandler.text;
        return csvData;
    }
}
