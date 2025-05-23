using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class UnitData
{ // a data class that holds all the necessary information needed for a given unit
    // A reference for the row number it holds in the DB
    public int databaseID { get; set; }
    // Name of the unit
    public string unitName { get; set; }
    // The traits the champion holds
    public List<string> unitTraits { get; set; }
    // How much the champion costs
    public int cost { get; set; }
    // Reference to shop icon image name
    public string shopIconName { get; set; } = "";

    // Reference to champion icon image name
    public string championIconName { get; set; } = "";

    public UnitData(int _databaseID, string _unitName, List<string> _unitTraits, int _cost, string _shopIconName, string _championIconName)
    {

        this.unitName = _unitName;
        this.databaseID = _databaseID;
        this.unitTraits = _unitTraits;
        this.cost = _cost;
        this.shopIconName = _shopIconName;
        this.championIconName = _championIconName;
    }

    public UnitData()
    {
        // generate a dummy instance of unitdata
    }

    public UnitData(UnitData newUnitData)
    {
        this.unitName = newUnitData.unitName;
        this.databaseID = newUnitData.databaseID;
        this.unitTraits = newUnitData.unitTraits;
        this.cost = newUnitData.cost;
        this.shopIconName = newUnitData.shopIconName;
        this.championIconName = newUnitData.championIconName;
    }

    public bool isMatchingTrait(string trait)
    {
        /*
        Check if a unit contains the given trait.
        */
        foreach (string unitTrait in unitTraits)
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
        unitName = null;
    }

    public bool isDummy()
    {
        /*
        If the unit data is a "dummy", e.g not an actual champion for the purposes of having an empty shop, return so
        */
        return unitName == null;
    }

    public override string ToString()
    {
        return this.unitName + " is a " + this.cost.ToString() + " Unit.";
    }

    // Getters and Setters
    public int DatabaseID
    {
        get { return databaseID; }
        set { databaseID = value; }
    }

    public string UnitName
    {
        get { return unitName; }
        set { unitName = value; }
    }

    public List<string> UnitTraits
    {
        get { return unitTraits; }
        set { unitTraits = value; }
    }

    public int Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    public string ShopIconName
    {
        get { return shopIconName; }
        set { shopIconName = value; }
    }

    public string ChampionIconName
    {
        get { return championIconName; }
        set { championIconName = value; }
    }
}