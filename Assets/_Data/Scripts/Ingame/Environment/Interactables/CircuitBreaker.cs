using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CircuitBreaker : Interactable
{
    [Header("Cache")]
    [SerializeField] LaserBeamGrid laserBeamGrid;
    [SerializeField] Animator anim;

    bool isActivated;

    // Events
    public UnityEvent onActivated;
    public UnityEvent onDeactivated;

    protected override void Use()
    {
        Activate();
    }

    public void Activate()
    {
        // Prevent the player trying to pull the lever when it's already activated
        if (isActivated)
            return;

        // Prevent the player pulling the lever until the laser beam grid resets it
        isActivated = true;

        // Move the lever to the downward position
        anim.SetTrigger("Activate");

        // Disable laser beam grid
        onActivated?.Invoke();

        // Disable laser beam grid
        // Keep this outside of activate otherwise you will get an infinite loop of the laser beam grid activating all the circuit breakers
        //laserBeamGrid.Deactivate();
    }

    public void Deactivate()
    {
        if (!isActivated)
            return;

        // Allow the player to use this circuit breaker again
        isActivated = false;

        // Move the lever to the upward position
        anim.SetTrigger("Deactivate");

        // Enable laser beam grid
        onDeactivated?.Invoke();
        //laserBeamGrid.Activate();
    }
}