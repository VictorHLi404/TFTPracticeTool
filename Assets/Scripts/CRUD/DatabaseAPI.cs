using SQLite;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A class that reads from the BaseGameInformation.db to provide basic game data (levels, shop odds, etc.)
/// Provides them in model format.
/// </summary>
public static class DatabaseAPI
{
    private static string dbName = @"Assets\Scripts\CRUD\BaseGameInformation.db";

    public static UnitData GetUnitData(ChampionEnum champion)
    {
        using (var connection = new SQLiteConnection(dbName))
        {
            var championEntities = connection.Table<ChampionDatabaseEntity>().ToList();
            var championEnumString = champion.ToString();
            var championDatabaseEntity = connection.Table<ChampionDatabaseEntity>().Where(p => p.ChampionEnumString == championEnumString).FirstOrDefault();
            if (championDatabaseEntity == null)
                throw new Exception($"Failed to identify {champion} in the database.");

            return new UnitData(championDatabaseEntity.DatabaseId,
                championDatabaseEntity.ChampionEnum,
                championDatabaseEntity.ChampionName,
                GetTraitList(championDatabaseEntity),
                championDatabaseEntity.Cost,
                championDatabaseEntity.ShopIconName,
                championDatabaseEntity.ChampionIconName);
        }
    }

    public static List<UnitData> GetAllUnitData()
    {
        /*
        Return a list of all possible UnitDatas found in the database. Open a connection to the database, execute SQL query,
        and create new instances of unitdata.
        */
        List<UnitData> unitDataList = new List<UnitData>();
        using (var connection = new SQLiteConnection(dbName))
        {
            var allUnits = connection.Table<ChampionDatabaseEntity>().ToList();
            foreach (var unit in allUnits)
            {
                unitDataList.Add(new UnitData(unit.DatabaseId,
                unit.ChampionEnum,
                unit.ChampionName,
                GetTraitList(unit),
                unit.Cost,
                unit.ShopIconName,
                unit.ChampionIconName));
            }
            connection.Close();
        }
        return unitDataList;
    }

    private static List<string> GetTraitList(ChampionDatabaseEntity champion)
    {
        var traitList = new List<string>();

        if (!string.IsNullOrEmpty(champion.Trait1))
            traitList.Add(champion.Trait1);
        if (!string.IsNullOrEmpty(champion.Trait2))
            traitList.Add(champion.Trait2);
        if (!string.IsNullOrEmpty(champion.Trait3))
            traitList.Add(champion.Trait3);

        return traitList;
    }

    public static int GetBagSize(UnitData champion)
    {
        /*
        Given a UnitData object, extract the cost of the unit and check its shop odds.
        */
        using (var connection = new SQLiteConnection(dbName))
        {
            var bagSizeEntity = connection.Table<DefaultBagSizesDatabaseEntity>().Where(x => x.ChampionCost == champion.Cost).FirstOrDefault();
            if (bagSizeEntity == null)
                Debug.Log($"Failed to find bag size for cost {champion.Cost}");
            return bagSizeEntity.BagSize;
        }
    }

    public static Dictionary<int, List<int>> GetShopOdds()
    {
        /*
        Get all shop odds for all levels. Return a dictionary mapping from the level to a list of the odds.
        */
        Dictionary<int, List<int>> shopOddsDictionary = new Dictionary<int, List<int>>();

        using (var connection = new SQLiteConnection(dbName))
        {
            var shopOddsEntities = connection.Table<ShopOddsDatabaseEntity>().ToList();
            foreach (var shopOddsEntity in shopOddsEntities)
            {
                var levelOdds = new List<int> { shopOddsEntity.OneCostOdds, shopOddsEntity.TwoCostOdds, shopOddsEntity.ThreeCostOdds, shopOddsEntity.FourCostOdds, shopOddsEntity.FiveCostOdds };
                shopOddsDictionary[shopOddsEntity.Level] = levelOdds;
            }
            connection.Close();
        }
        return shopOddsDictionary;
    }

    /// <summary>
    /// From the database, grab the mapping of the current level to the required amount of XP to beat the level.
    /// </summary>
    /// <returns>
    /// A dictionary mapping levels to XP requirement for that level.
    /// </returns>
    public static Dictionary<int, int> GetLevelMapping()
    {
        Dictionary<int, int> levelMapping = new Dictionary<int, int>();

        using (var connection = new SQLiteConnection(dbName))
        {
            var XPLevelsEntities = connection.Table<XPLevelsDatabaseEntity>().ToList();
            foreach (var XPLevelEntity in XPLevelsEntities)
            {
                levelMapping.Add(XPLevelEntity.Level, XPLevelEntity.XPRequirement);
            }
        }
        return levelMapping;
    }

    /// <summary>
    /// From the database, grab the mapping of the trait names to (their leveling scheme, their rarities/colors)
    /// </summary>
    /// <returns>A dictionary mapping trait naems to levelling scheme and rarities</returns>
    public static Dictionary<string, (List<int>, List<TraitRarities>)> getTraits()
    {
        Dictionary<string, (List<int>, List<TraitRarities>)> traitMapping = new Dictionary<string, (List<int>, List<TraitRarities>)>();
        Dictionary<string, List<int>> traitToLevels = new Dictionary<string, List<int>>();
        Dictionary<string, List<TraitRarities>> traitToRarities = new Dictionary<string, List<TraitRarities>>();

        using (var connection = new SQLiteConnection(dbName))
        {
            var traitLevelEntities = connection.Table<TraitLevelsDatabaseEntity>().ToList();
            foreach (var traitLevelEntity in traitLevelEntities)
            {
                var levels = new List<int>();

                if (traitLevelEntity.TierOne != null && traitLevelEntity.TierOne != 0)
                    levels.Add(traitLevelEntity.TierOne.Value);
                if (traitLevelEntity.TierTwo != null)
                    levels.Add(traitLevelEntity.TierTwo.Value);
                if (traitLevelEntity.TierThree != null)
                    levels.Add(traitLevelEntity.TierThree.Value);
                if (traitLevelEntity.TierFour != null)
                    levels.Add(traitLevelEntity.TierFour.Value);
                if (traitLevelEntity.TierFive != null)
                    levels.Add(traitLevelEntity.TierFive.Value);
                if (traitLevelEntity.TierSix != null)
                    levels.Add(traitLevelEntity.TierSix.Value);
                if (traitLevelEntity.TierSeven != null)
                    levels.Add(traitLevelEntity.TierSeven.Value);
                if (traitLevelEntity.TierEight != null)
                    levels.Add(traitLevelEntity.TierEight.Value);
                if (traitLevelEntity.TierNine != null)
                    levels.Add(traitLevelEntity.TierNine.Value);
                if (traitLevelEntity.TierTen != null)
                    levels.Add(traitLevelEntity.TierTen.Value);

                traitToLevels[traitLevelEntity.TraitName] = levels;
            }
            var traitColorsEntities = connection.Table<TraitColorsDatabaseEntity>().ToList();
            foreach (var traitColorEntity in traitColorsEntities)
            {
                var rarities = new List<TraitRarities>();

                if (traitColorEntity.TierOne != null && traitColorEntity.TierOne != 0)
                    rarities.Add((TraitRarities)traitColorEntity.TierOne.Value);
                if (traitColorEntity.TierTwo != null)
                    rarities.Add((TraitRarities)traitColorEntity.TierTwo.Value);
                if (traitColorEntity.TierThree != null)
                    rarities.Add((TraitRarities)traitColorEntity.TierThree.Value);
                if (traitColorEntity.TierFour != null)
                    rarities.Add((TraitRarities)traitColorEntity.TierFour.Value);
                if (traitColorEntity.TierFive != null)
                    rarities.Add((TraitRarities)traitColorEntity.TierFive.Value);
                if (traitColorEntity.TierSix != null)
                    rarities.Add((TraitRarities)traitColorEntity.TierSix.Value);
                if (traitColorEntity.TierSeven != null)
                    rarities.Add((TraitRarities)traitColorEntity.TierSeven.Value);
                if (traitColorEntity.TierEight != null)
                    rarities.Add((TraitRarities)traitColorEntity.TierEight.Value);
                if (traitColorEntity.TierNine != null)
                    rarities.Add((TraitRarities)traitColorEntity.TierNine.Value);
                if (traitColorEntity.TierTen != null)
                    rarities.Add((TraitRarities)traitColorEntity.TierTen.Value);

                traitToRarities[traitColorEntity.TraitName] = rarities;
            }
        }
        foreach (string traitName in traitToLevels.Keys)
        {
            traitMapping[traitName] = (traitToLevels[traitName], traitToRarities[traitName]);
        }
        return traitMapping;
    }

    public static Dictionary<(Component, Component), CompletedItem> getItemMapping()
    {
        Dictionary<(Component, Component), CompletedItem> itemMapping = new Dictionary<(Component, Component), CompletedItem>();

        using (var connection = new SQLiteConnection(dbName))
        {
            var itemDatabaseEntities = connection.Table<ItemDatabaseEntity>().ToList();
            foreach (var itemEntity in itemDatabaseEntities)
            {
                Component component1 = (Component)Enum.Parse(typeof(Component), itemEntity.ComponentOne.Replace(".", "").Replace(" ", ""));
                Component component2 = (Component)Enum.Parse(typeof(Component), itemEntity.ComponentTwo.Replace(".", "").Replace(" ", ""));
                CompletedItem item = (CompletedItem)Enum.Parse(typeof(CompletedItem), itemEntity.CompletedItem.Replace(".", "").Replace(" ", ""));
                itemMapping[(component1, component2)] = item;
            }
        }
        return itemMapping;
    }
    
}