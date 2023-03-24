using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementDetector : MonoBehaviour
{
    [SerializeField] ItemPickup itemPickup; // Needed so we can check if we are being held
    // Used to change the object's material red when blocked
    // Renderer so it can use both MeshRenderer and SpriteRenderer
    [SerializeField] Renderer[] renderers;
    [SerializeField] LayerMask layersToCheck; // Layers that aren't allowed to be placed on
    [HideInInspector] Color[] startingColours; // Starting colours of each renderers array element

    public bool IsBlocked { get; private set; }

    void Start()
    {
        // Make an array the same size of the renderers array
        startingColours = new Color[renderers.Length];

        // Fill it with each renderer material colour
        for (int i = 0; i < renderers.Length; i++)
        {
            startingColours[i] = renderers[i].material.color;
        }
    }

    // OnTriggerStay to prevent a bug where inside two triggers then exit one and the player is allowed to place (while still inside one)
    void OnTriggerStay(Collider other)
    {
        // Are we currently being held?
        if (itemPickup.heldBy)
        {
            IsBlocked = true;

            // Make materials red
            foreach (Renderer rend in renderers)
            {
                rend.material.color = Color.red;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Are we currently being held?
        if (itemPickup.heldBy)
        {
            IsBlocked = false;

            // Change materials back to normal
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = startingColours[i];
            }
        }
    }
}