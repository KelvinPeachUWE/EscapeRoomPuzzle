using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    static readonly float throwForce = 10f; // How far should items be thrown? (static so it's consistant for all players)

    [SerializeField] Transform holdPoint; // Where should a picked up item be held?
    
    // DEBUG
    [SerializeField] ItemPickup testItem;

    ItemPickup heldItem; // The object the player is currently holding

    public delegate void OnPickedUp(ItemPickup itemPickedUp);
    public event OnPickedUp onPickedUp;
    public delegate void OnDropped(ItemPickup itemDropped);
    public event OnDropped onDropped;

    void Update()
    {
        // Player input
        
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

                // DEBUG
                PickupItem(testItem);
                // DEBUG
            }
        }
        // Check if the player is pressing the throw button
        else if (Input.GetMouseButtonDown(0))
        {
            // Throw currently held item forward
            if (heldItem)
                ThrowItem(heldItem, transform.forward);
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
            if (onPickedUp != null)
                onPickedUp(itemToPickup);
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
        if (onDropped != null)
            onDropped(heldItem);

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