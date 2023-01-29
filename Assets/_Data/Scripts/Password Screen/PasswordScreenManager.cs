using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PasswordScreenManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] [Min(1)] string password = "Sputnik"; // The password the player must get from the previous puzzle (Dan's UI puzzle)
    [SerializeField] Color wrongPasswordColor = Color.red; // Colour to flash all text on the screen after wrong password input
    [SerializeField] string[] successTexts; // Lines of text to output after the player has successfully entered the correct password
    [SerializeField] string nextSceneName = "Game"; // Scene to load after succesfull password

    [Header("Sound")]
    [SerializeField] AudioClip wrongAttemptSound; // Sound effect to play after wrong password entered
    [SerializeField] AudioClip correctAttemptSound; // Sound effect to play after correct password entered

    [Header("Cache")]
    [SerializeField] Text flavourText; // Text before the login prompt to set the narrative.
    [SerializeField] InputField passwordInputField;
    [SerializeField] Text successfulLoginText; // Empty text to output "successTexts" into, one line at a time
    [SerializeField] AudioSource audioSrc;

    Color textStartingColor; // Keep the initial text colour so text temporarily flashed wrong can be reset

    void Start()
    {
        passwordInputField.ActivateInputField(); // Re-focus on the input field
        passwordInputField.Select(); // Re-focus on the input field

        // Get the UI's starting text colour so we can change it back to this if the password is entered wrong
        textStartingColor = flavourText.color;
    }

    void Update()
    {
        // Re-focus on input field (Unity automatically deselects it after enter is pressed)
        passwordInputField.ActivateInputField();    

        // Has the player pressed enter?
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Has the player entered at least one character?
            // Prevent spamming the enter key
            if (passwordInputField.text.Length > 0)
            {
                // Has the player entered the correct password?
                if (CheckPassword(passwordInputField.text))
                {
                    // Correct

                    // Disable input
                    passwordInputField.enabled = false;

                    // Play correct sound
                    audioSrc.PlayOneShot(correctAttemptSound);

                    // Show login text and go to the game
                    StartCoroutine(LoginCoroutine());
                }
                else
                {
                    // Incorrect

                    // Play incorrect sound
                    audioSrc.PlayOneShot(wrongAttemptSound);

                    // Briefly flash the text a different colour so the player knows they got it wrong
                    StartCoroutine(FlashText());
                }

                // Empty text field
                passwordInputField.text = "";
            }
        }
    }

    bool CheckPassword(string input)
    {
        bool isCorrect = false;

        if (input.ToLower() == password.ToLower()) // Prevent the player needing to worry about capitalisation
        {
            isCorrect = true;
        }

        // Return result
        return isCorrect;
    }

    IEnumerator FlashText()
    {
        // Turn to wrong colour
        flavourText.color = wrongPasswordColor;

        yield return new WaitForSeconds(0.2f);

        // Back to starting colour
        flavourText.color = textStartingColor;
    }

    IEnumerator LoginCoroutine()
    {
        // Display each line of success login message one at a time (like a real command line would)
        foreach (string newText in successTexts)
        {
            successfulLoginText.text += newText + "\n"; // Add line break to each piece of text
            yield return new WaitForSeconds(2f); // Simulate the computer performing an action
        }

        // Load next scene
        SceneManager.LoadScene(nextSceneName);
    }
}