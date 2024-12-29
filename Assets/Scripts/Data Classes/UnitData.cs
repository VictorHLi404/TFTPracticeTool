using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class UnitData { // a data class that holds all the necessary information needed for a given unit

    // NOTE: distinct from an actual unit trait / how a unit behaves on the field; read only data class 
    protected int databaseID; // a reference for the row number it holds in the db
    protected string unitName; // name of the unit
    protected List<string> unitTraits; // the traits the champion holds
    protected int cost; // how much the champion costs
    protected string shopIconName = ""; // reference to shop icon image name
    protected string championIconName = ""; // reference to champion icon image name

    public UnitData(int _databaseID, string _unitName, List<string> _unitTraits, int _cost, string _shopIconName, string _championIconName) {
    
        this.unitName = _unitName;
        this.databaseID = _databaseID;
        this.unitTraits = _unitTraits;
        this.cost = _cost;
        this.shopIconName = _shopIconName;
        this.championIconName = _championIconName;
    }

    public bool isMatchingTrait(string trait) { // check if a unit contains the necessary trait
        foreach (string unitTrait in unitTraits) {
            if (trait == unitTrait) {
                return true;
            }
        }
        return false;
    }

    public override string ToString() {
        return this.unitName + " is a " + this.cost.ToString() + " Unit.";
    }
 }