using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creak : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private float frequencyMin;
    [SerializeField] private float frequencyMax;
    [SerializeField] private float pitchMin;
    [SerializeField] private float pitchMax;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(RandomPlayer());
    }

    private IEnumerator RandomPlayer()
    {
        float waitTime;
        while (true)
        {
            waitTime = Random.Range(frequencyMin, frequencyMax);
            yield return new WaitForSeconds(waitTime);
            PlayRandomSound();
        }
    }

    private void PlayRandomSound()
    {
        int soundIdx = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(pitchMin, pitchMax);
        audioSource.pitch = randomPitch;
        audioSource.PlayOneShot(clips[soundIdx]);
    }
}
