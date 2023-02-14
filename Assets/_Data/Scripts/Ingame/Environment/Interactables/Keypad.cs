using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    [Header("Cache")]
    [SerializeField] KeypadUI keypadUI;

    // Events
    public delegate void OnKeypadUsed(Keypad keypad);
    public static event OnKeypadUsed onKeypadUsed;

    protected override void Use()
    {
        // Enable the UI canvas with the keypad buttons
        keypadUI.gameObject.SetActive(true);

        if (onKeypadUsed != null)
            onKeypadUsed(this);
    }
}