using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailsafeManager : MonoBehaviour
{
    [Header("Dual Monitors")]
    [SerializeField] Camera player1Camera;
    [SerializeField] Camera player2Camera;

    void Update()
    {
        // Has the escape room manager pressed the secret single monitor debug key combination?
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.M))
        {
            SetToSingleMonitor();
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
}