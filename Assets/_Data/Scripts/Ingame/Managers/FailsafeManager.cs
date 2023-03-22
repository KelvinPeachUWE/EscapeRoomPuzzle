using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class FailsafeManager : MonoBehaviour
{
    [Header("Dual Monitors")]
    [SerializeField] Camera player1Camera;
    [SerializeField] Camera player2Camera;

    [Header("Player 2 Controls")]
    [SerializeField] PlayerInput player2Input;

    [Header("Final Audio")]
    [SerializeField] GameObject victoryScreen;

    void Update()
    {
        // Has the escape room manager pressed any secret failsafe key combinations?

        // Single monitor debug key combination?
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.M))
        {
            SetToSingleMonitor();
        }
        // Instantly complete puzzle
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.V))
        {
            // Game complete!
            victoryScreen.SetActive(true);
        }
        // Reset puzzle
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.E))
        {
            SceneManager.LoadScene("Terminal");
        }
        // Make player 2 use the keyboard
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.T))
        {
            SetPlayerTwoControls();
        }
    }

    void SetToSingleMonitor()
    {
        // Set the player 2 camera to the first monitor
        player2Camera.targetDisplay = 0; // 0 = first monitor because it starts counting from zero

        // Set player 1's camera viewport to half the screen
        // Source - https://answers.unity.com/questions/8202/how-to-make-a-split-screen-2-cameras-rendering-at.html
        // Source - https://docs.unity3d.com/ScriptReference/Camera-rect.html
        player1Camera.rect = new Rect(0f, 0f, 0.5f, 1f);

        // Set player 2's camera to the second monitor
        player2Camera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
    }

    void SetPlayerTwoControls()
    {
        // Source - https://forum.unity.com/threads/multiple-players-on-keyboard-new-input-system.725834/#post-7476125
        PlayerInput.all[1].SwitchCurrentControlScheme("KeyboardMouse", Keyboard.current);
    }
}