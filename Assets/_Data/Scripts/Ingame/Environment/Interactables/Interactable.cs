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
    [SerializeField] AudioClip useSound;

    [Header("Cache")]
    [SerializeField] AudioSource audioSrc;

    // Events
    public delegate void OnUse(Interactable interactableUsed);
    public event OnUse onUse;

    public bool TryUse(GameObject user, ItemPickup heldItem)
    {
        // Does the player have the required item to use this?
        if (!RequiredItem || heldItem && heldItem.name == requiredItem.name)
        {
            Use(user);

            // Play sound (if there is one)
            if (audioSrc && useSound)
                audioSrc.PlayOneShot(useSound);

            if (onUse != null)
                onUse(this);

            return true;
        }
        else
        {
            return false;
        }
    }

    // All interactables will be usable, but what this means will be unique to each interactable class
    protected abstract void Use(GameObject usedBy);
}