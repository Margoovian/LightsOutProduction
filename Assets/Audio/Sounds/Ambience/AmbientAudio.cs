using System.Collections;
using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    [field: SerializeField] private AudioClip[] Clips { get; set; }
    [field: SerializeField] private float FrequencyMin { get; set; }
    [field: SerializeField] private float FrequencyMax { get; set; }
    [field: SerializeField] private float PitchMin { get; set; }
    [field: SerializeField] private float PitchMax { get; set; }
    [field: SerializeField] private string AmbienceMusic { get; set; }

    private void Start()
    {
        RandomPlayer();
    }

    private void Update()
    {
        if (!AudioManager.Instance.IsPlaying(AmbienceMusic) && !GameManager.Instance.PlayerData.InMenu)
            AudioManager.Instance.Play(AmbienceMusic);
        else if (AudioManager.Instance.IsPlaying(AmbienceMusic) && GameManager.Instance.PlayerData.InMenu)
            AudioManager.Instance.Stop(AmbienceMusic);
    }

    private IEnumerator RandomPlayer()
    {
        float waitTime;
        while (true)
        {
            waitTime = Random.Range(FrequencyMin, FrequencyMax);
            yield return new WaitForSeconds(waitTime);
            PlayRandomSound();
        }
    }

    private void PlayRandomSound()
    {
        int soundIdx = Random.Range(0, Clips.Length);
        float randomPitch = Random.Range(PitchMin, PitchMax);

        //AudioSource.pitch = randomPitch;
        //AudioManager.Instance.PlaySFX(Clips[soundIdx].name);
    }
}
