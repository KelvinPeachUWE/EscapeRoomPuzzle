using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserBeamGrid : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float disableTime = 3f; // How long should the lasers stay deactivated for?
    [SerializeField] GameObject[] lasers; // All the laser game objects to affect

    // Events
    public UnityEvent onActivated;
    public UnityEvent onDeactivated;

    bool isActivated = true;

    public void Activate()
    {
        // Prevent being activated twice
        if (isActivated)
            return;

        print("Laser beam grid activated.");

        isActivated = true;

        // Let all connected circuit breakers know they need to be activated
        onActivated?.Invoke();

        // Enable all the lasers so the player can't get through
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(true);
        }
    }

    public void Deactivate()
    {
        // Prevent being deactivated twice
        if (!isActivated)
            return;

        print("Laser beam grid deactivated.");

        isActivated = false;

        // Let all connected circuit breakers know they need to be deactivated
        onDeactivated?.Invoke();

        // Disable all the lasers so the player can get through
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(false);
        }

        // Start a timer to disable the lasers in a while
        StartCoroutine(ResetCoroutine());
    }

    IEnumerator ResetCoroutine()
    {
        // Stay disabled for just long enough for a player to get through
        yield return new WaitForSeconds(disableTime);

        Activate();
    }
}