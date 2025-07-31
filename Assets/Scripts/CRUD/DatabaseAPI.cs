using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using System;
/// <summary>
/// A class that reads from the BaseGameInformation.db to provide basic game data (levels, shop odds, etc.)
/// Provides them in model format.
/// </summary>
public static class DatabaseAPI
{
    // class that reads from databases to provide data as needed.
    // generates data objects (units, traits, etc) 
    private static string dbName = @"Data Source=Assets\Scripts\CRUD\BaseGameInformation.db";

    public static UnitData getUnitData(string championName)
    {
        // given a champion name (MUST MATCH THAT FOUND INSIDE OF SQL DB STRICTLY), return a UnitData object of that champion.
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Champions WHERE ChampionName=" + "\'" + championName + "\'" + ";";
                using (IDataReader reader = command.ExecuteReader())
                { // only 1 possible result given database structure, so just read once
                    reader.Read();
                    List<string> traitList = new List<string>();
                    // compile all traits into a list, ignore if empty
                    for (int i = 1; i <= 3; i++)
                    {
                        string currentHeader = "Trait" + i.ToString(); // sweep the row from Trait1 to Trait3
                        if ((string)reader[currentHeader] != "")
                        { // if the trait spot is non-null
                            traitList.Add((string)reader[currentHeader]);
                        }
                    } // shitty typecasting

                    ChampionEnum championEnum;
                    if (!Enum.TryParse((string)reader["ChampionEnum"], out championEnum))
                        Debug.LogError("Could not parse champion enum from the CSV file.");
                    
                    return new UnitData(int.Parse((string)reader["DatabaseID"]),
                                        championEnum,
                                        (string)reader["ChampionName"],
                                        traitList,
                                        int.Parse((string)reader["Cost"]),
                                        (string)reader["ShopIconName"],
                                        (string)reader["ChampionIconName"]);
                }
            }
        }
    }

    public static List<UnitData> getAllUnitData()
    {
        /*
        Return a list of all possible UnitDatas found in the database. Open a connection to the database, execute SQL query,
        and create new instances of unitdata.
        */
        List<UnitData> unitDataList = new List<UnitData>();
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Champions;";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    List<string> traitList = new List<string>();
                    // compile all traits into a list, ignore if empty
                    for (int i = 1; i <= 3; i++)
                    {
                        string currentHeader = "Trait" + i.ToString(); // sweep the row from Trait1 to Trait3
                        if ((string)reader[currentHeader] != "")
                        { // if the trait spot is non-null
                            traitList.Add((string)reader[currentHeader]);
                        }
                    } // shitty typecasting
                    ChampionEnum championEnum;
                    if (!Enum.TryParse((string)reader["ChampionEnum"], out championEnum))
                        Debug.LogError("Could not parse champion enum from the CSV file.");
                    unitDataList.Add(new UnitData(int.Parse((string)reader["DatabaseID"]),
                                                    championEnum,
                                                    (string)reader["ChampionName"],
                                                    traitList,
                                                    int.Parse((string)reader["Cost"]),
                                                    (string)reader["ShopIconName"],
                                                    (string)reader["ChampionIconName"]));
                }
                reader.Close();
            }
            connection.Close();
        }
        return unitDataList;
    }

    public static int getBagSize(UnitData champion)
    {
        /*
        Given a UnitData object, extract the cost of the unit and check its shop odds.
        */
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT BagSize FROM DefaultBagSizes WHERE Cost={champion.Cost}";
                using (SqliteDataReader reader = command.ExecuteReader())
                { // only 1 possible result given database structure, so just read once
                    reader.Read();
                    return int.Parse((string)reader["BagSize"]);
                }
            }
        }
    }

    public static Dictionary<int, List<int>> getShopOdds()
    {
        /*
        Get all shop odds for all levels. Return a dictionary mapping from the level to a list of the odds.
        */
        Dictionary<int, List<int>> shopOddsDictionary = new Dictionary<int, List<int>>();
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM ShopOdds";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    List<int> levelOdds = new List<int>();
                    for (int i = 1; i <= 5; i++)
                    {
                        string currentHeader = i.ToString() + "Cost";
                        levelOdds.Add(int.Parse((string)reader[currentHeader]));
                    }
                    shopOddsDictionary[int.Parse((string)reader["Levels"])] = levelOdds;
                }
                reader.Close();
            }
            return shopOddsDictionary;
        }
    }

    /// <summary>
    /// From the database, grab the mapping of the current level to the required amount of XP to beat the level.
    /// </summary>
    /// <returns>
    /// A dictionary mapping levels to XP requirement for that level.
    /// </returns>
    public static Dictionary<int, int> getLevelMapping()
    {
        Dictionary<int, int> levelMapping = new Dictionary<int, int>();
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM XPLevels";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    levelMapping.Add(int.Parse(reader.GetString(0)), int.Parse(reader.GetString(1)));
                }
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
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM TraitLevels";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    List<int> levels = new List<int>();
                    for (int i = 1; i <= 7; i++) // TODO: MAKE MORE ROBUST FOR FUTURE VERSIONS?
                    {
                        int traitLevel = getTraitLevel(reader, $"Tier{i}");
                        if (traitLevel != 0)
                        {
                            levels.Add(traitLevel);
                        }
                    }
                    string traitName = reader.GetString(reader.GetOrdinal("TraitName"));
                    traitToLevels[traitName] = levels;
                }
            }
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM TraitColors";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    List<TraitRarities> rarities = new List<TraitRarities>();
                    for (int i = 1; i <= 7; i++) // TODO: MAKE MORE ROBUST FOR FUTURE VERSIONS?
                    {
                        int traitRarity = getTraitLevel(reader, $"Tier{i}");
                        if (traitRarity != 0)
                        {
                            TraitRarities enumValue = (TraitRarities)traitRarity;
                            rarities.Add(enumValue);
                        }
                    }
                    string traitName = reader.GetString(reader.GetOrdinal("TraitName"));
                    traitToRarities[traitName] = rarities;
                }
            }
        }
        foreach (string traitName in traitToLevels.Keys)
        {
            traitMapping[traitName] = (traitToLevels[traitName], traitToRarities[traitName]);
        }
        return traitMapping;
    }

    private static int getTraitLevel(SqliteDataReader reader, string traitTier)
    {
        // current assumption is that all values in the table are nonnull; "null" values are just empty strings

        string traitLevelString = reader.GetString(reader.GetOrdinal(traitTier));
        if (traitLevelString != "")
        {
            return int.Parse(traitLevelString);
        }
        else
        {
            return 0;
        }
    }

    public static Dictionary<(Component, Component), CompletedItem> getItemMapping()
    {
        Dictionary<(Component, Component), CompletedItem> itemMapping = new Dictionary<(Component, Component), CompletedItem>();
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Items";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Component component1 = (Component)Enum.Parse(typeof(Component), reader.GetString(reader.GetOrdinal("Component1")).Replace(".", "").Replace(" ", ""));
                    Component component2 = (Component)Enum.Parse(typeof(Component), reader.GetString(reader.GetOrdinal("Component2")).Replace(".", "").Replace(" ", ""));
                    CompletedItem item = (CompletedItem)Enum.Parse(typeof(CompletedItem), reader.GetString(reader.GetOrdinal("CompletedItem")).Replace(".", "").Replace(" ", ""));
                    itemMapping[(component1, component2)] = item;
                }
            }
        }
        return itemMapping;
    }
    
}