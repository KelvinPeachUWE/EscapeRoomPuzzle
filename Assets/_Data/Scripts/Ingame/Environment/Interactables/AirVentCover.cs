using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AirVentCover : Interactable
{
    [Header("Cache")]
    [SerializeField] Animator anim;
    [SerializeField] ParticleSystem openEffect;

    protected override void Use(GameObject usedBy)
    {
        // Play animation
        anim.SetTrigger("Open");

        // Play open impact particle effect
        openEffect.Play();

        // Prevent being interacted with again
        GetComponent<Collider>().enabled = false;
        Destroy(this);
    }
}