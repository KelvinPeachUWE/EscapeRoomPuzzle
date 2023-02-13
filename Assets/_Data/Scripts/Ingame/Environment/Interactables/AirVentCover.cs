using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AirVentCover : Interactable
{
    [Header("Cache")]
    [SerializeField] Animator anim;

    protected override void Use()
    {
        // Play animation
        anim.SetTrigger("Open");

        // Prevent being interacted with again
        GetComponent<Collider>().enabled = false;
        Destroy(this);
    }
}