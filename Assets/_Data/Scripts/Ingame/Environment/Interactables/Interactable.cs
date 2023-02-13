using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class because the class isn't useful on it's own without the specific functionality of the base class.
// We need this abstract class though because it will keep the functionality consistent across all interactables.
public abstract class Interactable : MonoBehaviour
{
    public ItemPickup RequiredItem { get { return requiredItem; } }

    [Header("Settings")]
    [SerializeField] ItemPickup requiredItem;

    // All interactables will be usable, but what this means will be unique to each interactable class
    protected abstract void Use();
}