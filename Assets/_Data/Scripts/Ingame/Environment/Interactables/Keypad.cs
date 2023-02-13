using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    [Header("Cache")]
    [SerializeField] KeypadUI keypadUI;

    protected override void Use()
    {
        // Enable the UI canvas with the keypad buttons
        keypadUI.gameObject.SetActive(true);
    }
}