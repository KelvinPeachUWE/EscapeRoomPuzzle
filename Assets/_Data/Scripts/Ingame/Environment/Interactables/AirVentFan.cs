using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class AirVentFan : MonoBehaviour
{
    [SerializeField] Transform[] waypoints; // Positions the player will automatically move to
    [SerializeField] float moveSpeed = 2f; // How fast should the player be moved through the air vent

    void OnTriggerEnter(Collider other)
    {
        // Is it the player?
        if (other.transform.CompareTag("Player"))
        {
            // Move the player through the air vent system
            StartCoroutine(MovePlayer(other.transform));
        }
    }

    IEnumerator MovePlayer(Transform player)
    {
        // Disable player movement
        player.GetComponent<FirstPersonController>().enabled = false;
        // Prevent the player falling to the ground
        player.GetComponent<CharacterController>().enabled = false;

        // Move the player through the waypoints one at a time
        foreach (Transform waypoint in waypoints)
        {
            // Source - https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html
            // Have we reached the waypoint?
            while (Vector3.Distance(player.position, waypoint.position) > 0.01f) // 0.001f because being exactly 0 may never happen
            {
                // Move the player towards the waypoint
                var step =  moveSpeed * Time.deltaTime; // calculate distance to move
                player.position = Vector3.MoveTowards(player.position, waypoint.position, step);

                // Wait for the next frame to prevent the game freezing until we reach the final waypoint
                yield return null;
            }
        }

        // Player has now moved through all the waypoints

        // Enable player movement
        player.GetComponent<FirstPersonController>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
    }
}