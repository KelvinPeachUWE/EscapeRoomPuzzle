using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoopDelayBetween : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] AudioClip audioToPlay;
    [SerializeField] float timeBetweenIterations;

    [Header("Cache")]
    [SerializeField] AudioSource audioSrc;

    void Start()
    {
        StartCoroutine(LoopCoroutine());
    }

    IEnumerator LoopCoroutine()
    {
        while (true)
        {
            audioSrc.PlayOneShot(audioToPlay);

            yield return new WaitForSeconds(timeBetweenIterations);
        }
    }
}

