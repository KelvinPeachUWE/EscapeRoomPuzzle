using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeypadUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] [Min(1)] string password = "1337";
    [SerializeField] Color wrongInputColour = Color.red;
    // Which button is selected by default for player 2 (controller)
    // Without this, player 2 can't use the UI
    [SerializeField] GameObject firstSelectedButton;

    [Header("Cache")]
    [SerializeField] Canvas keypadUICanvas;
    [SerializeField] InputField inputField;
    [SerializeField] Image inputFieldImage;
    [SerializeField] Canvas victoryCanvas;

    // Which player game object enabled this game object?
    [HideInInspector] public GameObject enabledBy; // [HideInInspector] so people don't try and assign in inspector. It's set via the Keypad.cs script.

    Color inputBackgroundStartingColor; // Keep initial colour so when background is flashed on wrong input it can be changed back

    // Events
    public delegate void OnKeypadUiEnabled(GameObject enabledBy);
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
            onKeypadUiEnabled(enabledBy);

        // Based on the player that activated the keypad UI, choose which monitor to show the UI on
        // Player 1
        if (enabledBy.name == "Player1")
        {
            keypadUICanvas.targetDisplay = 0; // 0 = monitor 1 because it starts counting from 0
        }
        // Player 2
        else
        {
            keypadUICanvas.targetDisplay = 1; // 1 = 2 because it starts counting from 0

            // Make sure the first button is selected so the buttons can be traversed using a controller
            // Source - https://www.youtube.com/watch?v=SXBgBmUcTe0
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
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