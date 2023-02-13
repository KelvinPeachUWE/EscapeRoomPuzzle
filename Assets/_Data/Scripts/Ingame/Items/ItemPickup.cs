using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public bool IsDestroyOnUse { get { return isDestroyOnUse; } }

    [SerializeField] bool isDestroyOnUse; // Can this item only be used once?
}