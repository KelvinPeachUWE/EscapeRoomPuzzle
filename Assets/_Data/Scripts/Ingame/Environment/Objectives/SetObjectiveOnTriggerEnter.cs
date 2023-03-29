using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectiveOnTriggerEnter : MonoBehaviour
{
    [SerializeField] string newObjective;
    [SerializeField] string triggeredByTag = "Player";
    [SerializeField] bool shouldDestroyOnCollision = true;

    void OnTriggerEnter(Collider other)
    {
        // Make sure only the correct object triggers the new objective
        // E.g. a thrown crate won't trigger the next objective
        if (other.CompareTag(triggeredByTag))
        {
            // Set the objective string
            ObjectiveManager.SetCurrentObjective(newObjective);

            // Prevent objective being set again
            if (shouldDestroyOnCollision)
                Destroy(gameObject);
        }
    }
}