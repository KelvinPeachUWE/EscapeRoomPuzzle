using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] [Min(1)] string password = "1337";
    [SerializeField] Color wrongInputColour = Color.red;

    [Header("Cache")]
    [SerializeField] InputField inputField;
    [SerializeField] Image inputFieldImage;
    [SerializeField] Canvas victoryCanvas;

    Color inputBackgroundStartingColor; // Keep initial colour so when background is flashed on wrong input it can be changed back

    // Events
    public delegate void OnKeypadUiEnabled();
    public static event OnKeypadUiEnabled onKeypadUiEnabled;
    public delegate void OnKeypadUiDisabled();
    public static event OnKeypadUiDisabled onKeypadUiDisabled;

    void Start()
    {
        // Get the text field's initial background colour so we can change it back if a wrong code is entered
        inputBackgroundStartingColor = inputFieldImage.color;
    }

    void OnEnable()
    {
        if (onKeypadUiEnabled != null)
            onKeypadUiEnabled();

    }

    void Update()
    {
        // Is the player trying to exit the keypad and return to the main game?
        if (Input.GetKeyDown(KeyCode.X))
        {
            Exit();
        }
    }

    public void InputKey(string input)
    {
        // Has the cancel button been pressed?
        if (input == "c")
        {
            Reset();
        }
        // Number button pressed
        else
        {
            // Add new number
            inputField.text += input;

            // Have enough characters been inputted
            if (inputField.text.Length >= password.Length)
            {
                // Is it correct
                // Yes
                if (inputField.text == password)
                {
                    Victory();
                }
                // No
                else
                {
                    StartCoroutine(Flash());
                    Reset();
                }
            }
        }
    }

    void Reset()
    {
        inputField.text = "";
    }

    void Victory()
    {
        // Game complete!
        victoryCanvas.gameObject.SetActive(true);
    }

    void Exit()
    {
        // Hide this keypad UI
        gameObject.SetActive(false);

        if (onKeypadUiDisabled != null)
            onKeypadUiDisabled();
    }

    IEnumerator Flash()
    {
        // Turn to wrong colour
        inputFieldImage.color = wrongInputColour;

        // Wait briefly
        yield return new WaitForSeconds(0.1f);

        // Back to starting colour
        inputFieldImage.color = inputBackgroundStartingColor;
    }
}