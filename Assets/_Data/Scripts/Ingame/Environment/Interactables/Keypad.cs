using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    [SerializeField] KeypadUI keypadUI;

    public void Use()
    {
        // Enable the UI canvas with the keypad buttons
        keypadUI.gameObject.SetActive(true);
    }
}