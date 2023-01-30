using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class AutoDoor : MonoBehaviour
{
    static readonly Color accessGrantedColour = new Color(0.4980392f, 0.5882353f, 0.372549f); // Asparagus green
    static readonly Color accessDeniedColour = new Color(0.6705883f, 0.1058824f, 0.07058824f); // Tabasco red

    [Header("Settings")]
    [SerializeField] bool isAccessGranted = true; // Can this door be opened?
    [SerializeField] string roomName = "Generic Room"; // The room name text for the sign on the door
    [SerializeField] string canOpenTag = "Player"; // GameObjects with which tag can open this door?

    [Header("Sound")]
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;
    [SerializeField] AudioClip accessDeniedSound;

    [Header("Cache")]
    [SerializeField] Text[] roomNameTexts; // Array because there will be a sign on both sides
    [SerializeField] Text[] roomAccessTexts;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource audioSrc;

    void Start()
    {
        // Sign on door text
        foreach (Text roomNameText in roomNameTexts) // Loop through the signs on each side of the door
        {
            roomNameText.text = roomName;
        }

        // Setup door access panel colours and access text
        if (isAccessGranted)
        {
            // Unlocked door
            foreach (Text roomNameText in roomNameTexts)
            {
                roomNameText.color = accessGrantedColour;
            }

            foreach (Text roomAccessText in roomAccessTexts)
            {
                roomAccessText.color = accessGrantedColour;
                roomAccessText.text = "ACCESS GRANTED";
            }
        }
        else
        {
            // Locked door
            foreach (Text roomNameText in roomNameTexts)
            {
                roomNameText.color = accessDeniedColour;
            }

            foreach (Text roomAccessText in roomAccessTexts)
            {
                roomAccessText.color = accessDeniedColour;
                roomAccessText.text = "NO ROBOTS ALLOWED";
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Is this door enabled?
        if (!isAccessGranted)
        {
            // Show the player they can't use it
            audioSrc.PlayOneShot(accessDeniedSound);
            return;
        }

        // Does it have the correct tag? E.g. the player.
        if (other.CompareTag(canOpenTag))
        {
            Open();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Is this door enabled?
        if (!isAccessGranted)
            return;

        // Does it have the correct tag? E.g. the player.
        if (other.CompareTag(canOpenTag))
        {
            Close();
        }
    }

    void Open()
    {
        anim.SetTrigger("Open");

        audioSrc.PlayOneShot(openSound);
    }

    void Close()
    {
        anim.SetTrigger("Close");

        audioSrc.PlayOneShot(closeSound);
    }
}