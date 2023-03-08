using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public string HintTitle { get { return hintTitle; } }
    public string HintMessage { get { return hintMessage; } }

    [SerializeField] string hintTitle;
    [SerializeField] string hintMessage;
}