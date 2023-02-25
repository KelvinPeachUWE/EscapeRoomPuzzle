using UnityEngine;
using System.Collections;

// Source - https://docs.unity3d.com/Manual/MultiDisplay.html
public class ActivateAllDisplays : MonoBehaviour
{
    void Start ()
    {
        // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
        // Check if additional displays are available and activate each.
    
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }
}