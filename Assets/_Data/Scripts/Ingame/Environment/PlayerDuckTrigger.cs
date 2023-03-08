using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDuckTrigger : MonoBehaviour
{
    [SerializeField] bool shouldDuck = true;

    void OnTriggerEnter(Collider other)
    {
        // Is it the player (with ducking capability)?
        if (other.GetComponent<PlayerDucking>())
        {
            // Is this a duck or un-duck trigger
            if (shouldDuck)
            {
                print("Duck");
                other.GetComponent<PlayerDucking>().Duck();
            }
            else
            {
                print("Stand");
                other.GetComponent<PlayerDucking>().Stand();
            }
        }
    }
}