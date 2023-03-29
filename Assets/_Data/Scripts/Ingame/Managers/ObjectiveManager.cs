using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectiveManager
{
    // Set the initial objective
    // Other objectives will be set by a trigger collider with SetObjectiveOnTriggerEnter.cs
    public static string CurrentObjective { get; private set; } = "FIND AN ITEM TO UNSCREW THE AIR VENT";

    // Events
    public delegate void OnObjectiveChanged(string newObjective);
    public static event OnObjectiveChanged onObjectiveChanged;

    public static void SetCurrentObjective(string newObjective)
    {
        CurrentObjective = newObjective;

        // Update UI
        if (onObjectiveChanged != null)
            onObjectiveChanged(newObjective);
    }
}