using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other) 
    {
        // Is it an item pickup?
        if (other.GetComponent<ItemPickup>())
        {
            // Reset it to its original position
            other.GetComponent<ItemPickup>().Reset();
        }
        // Is it the player?
        else if (other.transform.CompareTag("Player"))
        {
            // Disable character controller
            other.transform.GetComponent<CharacterController>().enabled = false;

            // Teleport player back to start
            other.transform.position = new Vector3(5.47f, -1.5f, -18.1f);

            // Enable character controller
            other.transform.GetComponent<CharacterController>().enabled = true;
        }
    }
}