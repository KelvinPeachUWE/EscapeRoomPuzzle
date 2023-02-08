using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AirVentCover : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] Animator anim;

    void Update()
    {
        // DEBUG
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Use();
        }
        // DEBUG
    }

    void Use()
    {
        // Play animation
        anim.SetTrigger("Open");

        // Prevent being interacted with again
        GetComponent<Collider>().enabled = false;
        Destroy(this);
    }
}