using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    static readonly float throwForce = 10f; // How far should items be thrown? (static so it's consistant for all players)
    static readonly float interactRange = 2f;

    [SerializeField] Transform holdPoint; // Where should a picked up item be held?
    [SerializeField] Transform camera; // Camera used by this player
    [SerializeField] [Min(1)] int playerNumber = 1; // Which player is this? Used to make sure correct input is checked.
    [SerializeField] AudioSource audioSrc; // Where to play sound effects

    ItemPickup heldItem; // The object the player is currently holding
    GameObject currentlyLookingAt; // The object the player is currently looking at (if any)
    GameObject previouslyLookingAt; // The object the player was looking at the previous frame (if any)

    // Event
    // Item events
    public delegate void OnItemStartedLookingAt(ItemPickup itemStartedLookingAt);
    public event OnItemStartedLookingAt onItemStartedLookingAt;
    public delegate void OnItemStoppedLookingAt(ItemPickup itemStoppedLookingAt);
    public event OnItemStoppedLookingAt onItemStoppedLookingAt;
    public delegate void OnItemPickedUp(ItemPickup itemPickedUp);
    public event OnItemPickedUp onItemPickedUp;
    public delegate void OnItemDropped(ItemPickup itemDropped);
    public event OnItemDropped onItemDropped;
    public delegate void OnItemDestroyed(ItemPickup itemDestroyed);
    public event OnItemDestroyed onItemDestroyed;
    // Interactable events
    public delegate void OnInteractableStartedLookingAt(Interactable interactableStartedLookingAt, ItemPickup heldItem); // heldItem is needed so it can be determined if the player has the required item
    public event OnInteractableStartedLookingAt onInteractableStartedLookingAt;
    public delegate void OnInteractableStoppedLookingAt(Interactable interactableStoppedLookingAt);
    public event OnInteractableStoppedLookingAt onInteractableStoppedLookingAt;
    // Hint areas
    public delegate void OnHintStartedLookingAt(Hint hintStartedLookingAt);
    public event OnHintStartedLookingAt onHintStartedLookingAt;
    public delegate void OnHintStoppedLookingAt(Hint hintStoppedLookingAt);
    public event OnHintStoppedLookingAt onHintStoppedLookingAt;    

    void Update()
    {
        PlayerInput();
        CheckIfLookingAtObject();
    }

    void PlayerInput()
    {
        // Check for this player's input
        bool inputPressed = false;

        // Player 1
        if (playerNumber == 1)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                inputPressed = true;
            }
        }
        // Player 2
        else
        {
            if (Input.GetKeyDown("joystick button 2"))
            {
                inputPressed = true;
            }
        }

        // Has this player pressed any buttons?
        if (inputPressed)
        {
            // Is the player currently looking at an interactable?
            if (currentlyLookingAt && currentlyLookingAt.GetComponent<Interactable>())
            {
                // Try to use this interactable (we may not have the correct item)
                if (currentlyLookingAt.GetComponent<Interactable>().TryUse(gameObject, heldItem))
                {
                    // If we are succesful and a held item is one-time use, destroy it
                    if (heldItem && heldItem.IsDestroyOnUse)
                    {
                        DestroyHeldItem();
                    }
                }
            }
            // Is the player currently holding an item?
            else if (heldItem)
            {
                // Check if the held item is in a valid (empty) location
                // Prevent the held item being placed inside another item or wall
                if (!heldItem.IsPlacementBlocked())
                {
                    // Throw currently held item forward
                    ThrowItem(heldItem, transform.forward);
                }
            }
            // Is the player currently looking at an item?
            else if (currentlyLookingAt && currentlyLookingAt.GetComponent<ItemPickup>())
            {
                // Pickup looked at item
                PickupItem(currentlyLookingAt.GetComponent<ItemPickup>());
            }
        }
    }

    void CheckIfLookingAtObject()
    {
        // Check if the player is looking at an object (item or interactable)
        // Source - https://stackoverflow.com/questions/73242434/raycast-from-camera-in-unity
        RaycastHit hit;

        // Raycast forward from the camera's position
        if (Physics.Raycast(camera.position, camera.forward, out hit, interactRange))
        {
            // Is it an item (e.g. screwdriver) ?
            if (hit.transform.GetComponent<ItemPickup>())
            {
                // Is it a different item than the current one? (prevent staring at the same item triggering code)
                if (currentlyLookingAt != hit.transform.gameObject)
                {
                    // Player is looking at an item
                    currentlyLookingAt = hit.transform.gameObject;

                    if (onItemStartedLookingAt != null)
                        onItemStartedLookingAt(hit.transform.GetComponent<ItemPickup>());
                }
            }
            // Is it an interactable (e.g. air vent cover)?
            else if (hit.transform.GetComponent<Interactable>())
            {
                // Player is looking at an interactable

                // Is it a different interactable than the current one?
                if (currentlyLookingAt != hit.transform.gameObject)
                {
                    currentlyLookingAt = hit.transform.gameObject;

                    if (onInteractableStartedLookingAt != null)
                        onInteractableStartedLookingAt(hit.transform.GetComponent<Interactable>(), heldItem);
                }
            }
            // Is the player currently looking at a hint area?
            else if (hit.transform.GetComponent<Hint>())
            {
                // Is it a different hint than the current one?
                if (currentlyLookingAt != hit.transform.gameObject)
                {
                    currentlyLookingAt = hit.transform.gameObject;

                    // Let the UI system (and anyone else interested) know we are looking at a hint area
                    if (onHintStartedLookingAt != null)
                        onHintStartedLookingAt(hit.transform.GetComponent<Hint>());
                }
            }
            // Not looking at anything interactive
            else
            {
                // Has the player stopped looking at an object since the previous frame?
                if (currentlyLookingAt)
                {
                    // Let anyone who is interested know (e.g. UI) an item or interactable has stopped being looked at

                    // Determine whether it's an item or 
                    // Item?
                    if (currentlyLookingAt.GetComponent<ItemPickup>())
                    {
                        // Trigger event
                        if (onItemStoppedLookingAt != null)
                            onItemStoppedLookingAt(currentlyLookingAt.GetComponent<ItemPickup>());
                    }
                    // Interactable?
                    else if (currentlyLookingAt.GetComponent<Interactable>())
                    {
                        // Trigger event
                        if (onInteractableStoppedLookingAt != null)
                            onInteractableStoppedLookingAt(currentlyLookingAt.GetComponent<Interactable>());
                    }
                    // Hint area?
                    else if (currentlyLookingAt.GetComponent<Hint>())
                    {
                        // Trigger event
                        if (onHintStoppedLookingAt != null)
                            onHintStoppedLookingAt(currentlyLookingAt.GetComponent<Hint>());
                    }

                    currentlyLookingAt = null;
                }
            }
        }
        // Check if the player has stopped looking at an object?
        else
        {
                // Has the player stopped looking at an object since the previous frame?
                if (currentlyLookingAt)
                {
                    // Let anyone who is interested know (e.g. UI) an item or interactable has stopped being looked at

                    // Determine whether it's an item or 
                    // Item?
                    if (currentlyLookingAt.GetComponent<ItemPickup>())
                    {
                        // Trigger event
                        if (onItemStoppedLookingAt != null)
                            onItemStoppedLookingAt(currentlyLookingAt.GetComponent<ItemPickup>());
                    }
                    // Interactable?
                    else if (currentlyLookingAt.GetComponent<Interactable>())
                    {
                        // Trigger event
                        if (onInteractableStoppedLookingAt != null)
                            onInteractableStoppedLookingAt(currentlyLookingAt.GetComponent<Interactable>());
                    }
                    // Hint area?
                    else if (currentlyLookingAt.GetComponent<Hint>())
                    {
                        // Trigger event
                        if (onHintStoppedLookingAt != null)
                            onHintStoppedLookingAt(currentlyLookingAt.GetComponent<Hint>());
                    }

                    currentlyLookingAt = null;
                } 
        }
    }

    void PickupItem(ItemPickup itemToPickup)
    {
        // Make sure a valid item was passed in
        if (itemToPickup)
        {
            // Store a reference to the item picked up so it can be used later
            heldItem = itemToPickup;
            heldItem.heldBy = this;

            // Pickup the object
            Vector3 holdPosition = holdPoint.position + holdPoint.TransformDirection(Vector3.forward) * heldItem.ForwardHoldOffset;
            heldItem.transform.position = holdPosition;

            heldItem.transform.rotation = holdPoint.rotation;
            heldItem.transform.parent = holdPoint;

            // Prevent held object interacting with world (other than collisions)
            heldItem.GetComponent<Rigidbody>().isKinematic = true;

            // Disable all colliders (including sphere colliders used to make the area the player needs to look at bigger)
            // This prevents a held item being detected as being looked at
            // Source - https://answers.unity.com/questions/730070/how-to-disable-all-colliders-on-a-game-object.html
            foreach(Collider collider in heldItem.GetComponents<Collider>())
            {
                collider.enabled = false;
            }

            // Play pickup sound (if there is one)
            if (audioSrc && heldItem.PickupSound)
                audioSrc.PlayOneShot(heldItem.PickupSound);

            // Let other objects that want to know know (e.g. display pickup message)
            if (onItemPickedUp != null)
                onItemPickedUp(itemToPickup);
        }
    }

    void DropHeldItem()
    {
        // Drop the held item
        heldItem.transform.parent = null;
        heldItem.heldBy = null;

        // Allow held item to interact with world
        heldItem.GetComponent<Rigidbody>().isKinematic = false;

        // Enable all colliders (including sphere colliders)
        // Source - https://answers.unity.com/questions/730070/how-to-disable-all-colliders-on-a-game-object.html
        foreach(Collider collider in heldItem.GetComponents<Collider>())
        {
            collider.enabled = true;
        }

        // Play drop sound (if there is one)
        if (audioSrc && heldItem.DropSound)
            audioSrc.PlayOneShot(heldItem.DropSound);

        // Let other objects that want to know know 
        if (onItemDropped != null)
            onItemDropped(heldItem);

        heldItem = null;
    }

    void ThrowItem(ItemPickup itemToThrow, Vector3 throwDirection)
    {
        // First stop holding the item
        DropHeldItem();

        // Apply a force to the item's rigidbody in the throwing direction
        itemToThrow.GetComponent<Rigidbody>().AddForce(throwDirection * throwForce, ForceMode.Impulse);
    }

    void DestroyHeldItem()
    {
        // Remove from game
        Destroy(heldItem.gameObject);

        // Let other interested objects know
        if (onItemDestroyed != null)
            onItemDestroyed(heldItem);

        // Remove from memory
        heldItem = null;
    }
}