using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [HideInInspector] public PlayerInteract heldBy; // Public so PlayerInteract can set it

    public bool IsDestroyOnUse { get { return isDestroyOnUse; } }
    public float ForwardHoldOffset { get { return forwardHoldOffset; } }
    public AudioClip PickupSound { get { return pickupSound; } }

    [SerializeField] bool isDestroyOnUse; // Can this item only be used once?
    [SerializeField] float forwardHoldOffset; // Adjust where the item should be held
    // A child object with the PlacementDetector script. Will be toggled to check if an item can be dropped.
    // Separate object so the collider doesn't affect the item pickup's rigidbody
    [SerializeField] PlacementDetector placementDetector;
    [SerializeField] AudioClip pickupSound;

    // Store in case we need to reset the item (e.g. it goes out-of-bounds)
    Vector3 initialPosition;
    Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // Check if there is nothing at the items current location
    // This is used to prevent this item being placed inside another item or wall
    public bool IsPlacementBlocked()
    {
        return placementDetector.IsBlocked;
    }

    public void Reset()
    {
        // Reset position and rotation
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Reset velocity
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}