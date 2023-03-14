using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// If the player is behind schedule, then some big hints will be shown
public class HintManager : MonoBehaviour
{
    [Header("Vent")]
    [SerializeField] float ventHintTime = 120f;
    [SerializeField] AirVentCover airVent;

    [Header("Laser")]
    [SerializeField] float laserHintTime = 120f;
    [SerializeField] LaserBeamGrid laserBeamGrid;

    [Header("Poster")]
    [SerializeField] float posterHintTime = 120f;
    [SerializeField] Text[] posterScreenOverrideTexts; // All of the screen text to change to the hint

    [Header("Password")]
    [SerializeField] float displayPasswordTime = 60f;
    [SerializeField] Text[] passwordScreenOverrideTexts; // All of the screen text to change to the password

    void Start()
    {
        // Start the hint countdown as soon as the main game begins
        StartCoroutine(GiveHintCoroutine());
    }

    IEnumerator GiveHintCoroutine()
    {
        // *** VENT ***
        // Wait for vent timer
        yield return new WaitForSeconds(ventHintTime);

        // Check if the air vent has been opened (if the script hasn't been deleted yet)
        if (airVent)
        {
            // Make air vent fall over
            airVent.TryUse(GameObject.FindGameObjectWithTag("Player"), airVent.RequiredItem);

            // Give hint to go through air vent

        }




        // *** LASER ***
        // Wait for laser timer
        yield return new WaitForSeconds(laserHintTime);

        // Disable the laser grid
        laserBeamGrid.Disable();




        // *** POSTER ***
        // Wait for poster timer
        yield return new WaitForSeconds(posterHintTime);

        // Change terminal screen text to look at poster hint
        foreach(Text text in posterScreenOverrideTexts)
        {
            text.text = "Don't type any poster numbers into the keypad :)";
        }




        // *** PASSWORD ***
        // Wait for just give the password timer
        yield return new WaitForSeconds(displayPasswordTime);

        // Change terminal screen text to look at poster hint
        foreach (Text text in passwordScreenOverrideTexts)
        {
            text.text = "Don't type 1961 into the keypad :)";
        }
    }
}