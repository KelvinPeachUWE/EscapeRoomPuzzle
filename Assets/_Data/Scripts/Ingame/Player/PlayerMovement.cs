using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    [Header("Cache")]
    [SerializeField] CharacterController controller;

    void Update()
    {
        // Get movement input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Apply movement input
        Vector3 move = new Vector3(horizontal, 0f, vertical);
        controller.SimpleMove(move * moveSpeed);
    }
}