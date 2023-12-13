using System.Collections;
using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    [field: SerializeField] private string[] Clips { get; set; }
    [field: SerializeField] private string AmbienceMusic { get; set; }
    [field: SerializeField] private Vector2 Frequency { get; set; }

    private IEnumerator RandomPlayer()
    {
        float waitTime;
        while (true)
        {
            waitTime = Random.Range(Frequency.x, Frequency.y);
            yield return new WaitForSeconds(waitTime);
            PlayRandomSound();
        }
    }

    private void PlayRandomSound() => AudioManager.Instance.PlaySFX(Clips[Random.Range(0, Clips.Length)]);

    private void Start() => RandomPlayer();

    private void Update()
    {
        if (!AudioManager.Instance.IsPlaying(AmbienceMusic) && !GameManager.Instance.PlayerData.InMenu)
            AudioManager.Instance.Play(AmbienceMusic);
        else if (AudioManager.Instance.IsPlaying(AmbienceMusic) && GameManager.Instance.PlayerData.InMenu)
            AudioManager.Instance.Stop(AmbienceMusic);
    }
}
