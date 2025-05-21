
using System;
using Unity.VisualScripting.ReorderableList;
using UnityEngine.Rendering.Universal.Internal;

/// <summary>
/// A class representing the player's bench. Contains references to all of the champions inside of it
/// (agnostic of position), and handles logic related to validating buying and selling.
/// 
/// Bench does NOT need to know what kind of units are on it, only the count. 
/// </summary>
public class Bench : UnitManager
{
    public override void AddUnit()
    {
        currentUnitCount++;
    }

    public override void RemoveUnit()
    {
        currentUnitCount--;
    }

    public override bool CanUnitBePlaced()
    {
        return currentUnitCount <= maxUnitCount;
    }
}