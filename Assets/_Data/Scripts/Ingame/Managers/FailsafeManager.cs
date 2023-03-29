using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using StarterAssets;

public class FailsafeManager : MonoBehaviour
{
    public static bool isOneMonitorMode;

    [Header("Dual Monitors")]
    [SerializeField] Camera player1Camera;
    [SerializeField] Camera player2Camera;
    [SerializeField] RectTransform interfaceRect1;
    [SerializeField] Canvas interfaceCanvas2;
    [SerializeField] RectTransform interfaceRect2;
    [SerializeField] GameObject liveText1;
    [SerializeField] GameObject liveText2;

    [Header("Player 2 Controls")]
    [SerializeField] PlayerInput player2Input;

    [Header("Final Audio")]
    [SerializeField] GameObject victoryScreen;

    [Header("Mouse Sensitivity")]
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] float lowMouseSensitivity = 5f;
    [SerializeField] float highMouseSensitivity = 15f;

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
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.G))
        {
            SetPlayerTwoControls();
        }
        // Low mouse sensitivity
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.M))
        {
            firstPersonController.RotationSpeed = lowMouseSensitivity;
        }
        // High mouse sensitivity
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.H) && Input.GetKey(KeyCode.M))
        {
            firstPersonController.RotationSpeed = highMouseSensitivity;
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

        // Make sure looking at object notifications still show up
        // Source - https://answers.unity.com/questions/1577433/how-to-get-and-change-recttransform-position-left.html
        interfaceRect1.offsetMax = new Vector2(-960f, 250f); // First half of screen

        interfaceCanvas2.targetDisplay = 0; // Move player 2 notifications to monitor 1
        interfaceRect2.offsetMin = new Vector2(960f, 0f); // Second half of screen
        // Source - https://forum.unity.com/threads/modify-the-width-and-height-of-recttransform.270993/
        interfaceRect2.sizeDelta = new Vector2 (interfaceRect2.sizeDelta.x, 250f); 

        // Hide 'REMOTE 1' text
        liveText1.SetActive(false);
        liveText2.SetActive(false);

        // Set the keypad to use monitor 1
        isOneMonitorMode = true;
    }

    void SetPlayerTwoControls()
    {
        // Source - https://forum.unity.com/threads/multiple-players-on-keyboard-new-input-system.725834/#post-7476125
        PlayerInput.all[1].SwitchCurrentControlScheme("KeyboardMouse", Keyboard.current);
    }
}