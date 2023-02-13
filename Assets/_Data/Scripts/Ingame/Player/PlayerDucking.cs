using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerDucking : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float standHeight = 2f;
    [SerializeField] float duckHeight = 1f;

    [Header("Cache")]
    [SerializeField] CharacterController controller;

    public void Duck()
    {
        controller.height = duckHeight;
    }

    public void Stand()
    {
        controller.height = standHeight;
    }
}
