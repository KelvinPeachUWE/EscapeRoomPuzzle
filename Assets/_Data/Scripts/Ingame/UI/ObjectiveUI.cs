using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Text objectiveText;
    [SerializeField] AudioClip objectiveChangedSound;

    [Header("Cache")]
    [SerializeField] AudioSource audioSrc;

    void Awake()
    {
        // Subscribe to events
        ObjectiveManager.onObjectiveChanged += UpdateText;
    }

    void Start()
    {
        // Set initial objective
        objectiveText.text = "OBJECTIVE: " + ObjectiveManager.CurrentObjective;
    }

    void UpdateText(string newText)
    {
        // Update text
        objectiveText.text = "OBJECTIVE: " + newText;
        
        // Play sound
        if (audioSrc && objectiveChangedSound)
            audioSrc.PlayOneShot(objectiveChangedSound);
    }
}