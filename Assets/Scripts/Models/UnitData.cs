using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class UnitData
{ // a data class that holds all the necessary information needed for a given unit
    // A reference for the row number it holds in the DB
    public int DatabaseID { get; set; }
    // Name of the unit
    public ChampionEnum? UnitName { get; set; }
    // The traits the champion holds
    public string DisplayName { get; set; }
    public List<string> UnitTraits { get; set; }
    // How much the champion costs
    public int Cost { get; set; }
    // Reference to shop icon image name
    public string ShopIconName { get; set; } = "";

    // Reference to champion icon image name
    public string ChampionIconName { get; set; } = "";

    public UnitData(int _databaseID, ChampionEnum _unitName, string _displayName, List<string> _unitTraits, int _cost, string _shopIconName, string _championIconName)
    {
        this.UnitName = _unitName;
        this.DatabaseID = _databaseID;
        this.DisplayName = DisplayName;
        this.UnitTraits = _unitTraits;
        this.Cost = _cost;
        this.ShopIconName = _shopIconName;
        this.ChampionIconName = _championIconName;
    }

    public UnitData()
    {
        // generate a dummy instance of unitdata
    }

    public UnitData(UnitData newUnitData)
    {
        this.UnitName = newUnitData.UnitName;
        this.DatabaseID = newUnitData.DatabaseID;
        this.UnitTraits = newUnitData.UnitTraits;
        this.Cost = newUnitData.Cost;
        this.ShopIconName = newUnitData.ShopIconName;
        this.ChampionIconName = newUnitData.ChampionIconName;
    }

    public bool isMatchingTrait(string trait)
    {
        /*
        Check if a unit contains the given trait.
        */
        foreach (string unitTrait in UnitTraits)
        {
            if (trait == unitTrait)
            {
                return true;
            }
        }
        return false;
    }


    public void setToDummy()
    {
        /*
        set the unit data to a blank value.
        */
        UnitName = null;
    }

    public bool isDummy()
    {
        /*
        If the unit data is a "dummy", e.g not an actual champion for the purposes of having an empty shop, return so
        */
        return UnitName == null;
    }

    public override string ToString()
    {
        return this.UnitName + " is a " + this.Cost.ToString() + " Unit.";
    }
}