using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    static readonly float throwForce = 10f; // How far should items be thrown? (static so it's consistant for all players)
    static readonly float interactRange = 5f;

    [SerializeField] Transform holdPoint; // Where should a picked up item be held?

    ItemPickup heldItem; // The object the player is currently holding
    GameObject currentlyLookingAt; // The object the player is currently looking at (if any)

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
    // Interactable events
    public delegate void OnInteractableStartedLookingAt(Interactable interactableStartedLookingAt);
    public event OnInteractableStartedLookingAt onInteractableStartedLookingAt;
    public delegate void OnInteractableStoppedLookingAt(Interactable interactableStoppedLookingAt);
    public event OnInteractableStoppedLookingAt onInteractableStoppedLookingAt;

    void Update()
    {
        PlayerInput();
        CheckIfLookingAtObject();
    }

    void PlayerInput()
    {        
        // Check if the player is pressing the item pickup button
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Is the player currently holding an item?
            if (heldItem)
            {
                // Drop current item
                DropHeldItem();
            }
            else
            {
                // Pickup new item
                //PickupItem();
            }
        }
        // Check if the player is pressing the throw button
        else if (Input.GetMouseButtonDown(0))
        {
            // Throw currently held item forward
            if (heldItem)
            {
                ThrowItem(heldItem, transform.forward);
            }
        }
    }

    void CheckIfLookingAtObject()
    {
        // Check if the player is looking at an object (item or interactable)
        // Source - https://stackoverflow.com/questions/73242434/raycast-from-camera-in-unity
        Transform camera = Camera.main.transform;
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

                    print("Player started looking at " + currentlyLookingAt.transform.name);
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
                        onInteractableStartedLookingAt(hit.transform.GetComponent<Interactable>());

                    print("Player started looking at " + currentlyLookingAt.name);
                }
            }
            else
            {
                // Has the player stopped looking at an object since the previous frame?
                if (currentlyLookingAt)
                {
                    print("Player stopped looking at " + currentlyLookingAt.transform.name);

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

                    currentlyLookingAt = null;
                }
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

            // Pickup the object
            heldItem.transform.position = holdPoint.position;
            heldItem.transform.rotation = holdPoint.rotation;
            heldItem.transform.parent = holdPoint;

            // Prevent held object interacting with world
            heldItem.GetComponent<Rigidbody>().isKinematic = true;
            heldItem.GetComponent<Collider>().enabled = false;

            // Let other objects that want to know know (e.g. display pickup message)
            if (onItemPickedUp != null)
                onItemPickedUp(itemToPickup);
        }
    }

    void DropHeldItem()
    {
        // Drop the held item
        heldItem.transform.parent = null;

        // Allow held item to interact with world
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        heldItem.GetComponent<Collider>().enabled = true;

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
        Destroy(heldItem);

        heldItem = null;
    }
}