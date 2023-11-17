using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Stem", fileName = "Stem")]
public class Stem : ScriptableObject
{
    internal AudioSource source;
    public float CurrentVolume { get { return source.volume; }  private set { if (source) source.volume = value; } }
    [field: SerializeField, Tooltip("The Audio Clip")] public AudioClip Clip { get; set; }
    [field: SerializeField, Tooltip("Name to Refer Back to when calling the AudioManager [Warning! if blank or duplicated uses audio clips name]")] public string FriendlyName { get; set; }
    [field: SerializeField, Tooltip("The Type of Audio Clip")] public StemType ClipType { get; set; }
    [field: SerializeField, Range(0, 1), Tooltip("Audios Natural Volume")] public float GlobalVolume { get; set; } = 1;  
    [field: SerializeField, Range(-3, 3), Tooltip("If Negative Audio is Reversed")] public float Pitch { get; set; } = 1;
    [field: SerializeField, Range(-3, 1), Tooltip("Audio Left to Right")] public float Pan { get; set; } = 0;
    [field: SerializeField, Tooltip("Start Offset")] public float StartPosition { get; set; }
    [field: SerializeField, Tooltip("Does The Audio Loop?")] public bool IsLoop { get; set; }

    public bool EnableRandomPitch { get; set; }
    public bool EnableRandomPan { get; set; }
    public bool EnableRandomVolume { get; set; }
    public Vector2 RandomPitchRange { get; set; } = new Vector2(1,1);
    public Vector2 RandomPanRange { get; set; } = new Vector2(0, 0);
    public Vector2 RandomVolumeRange { get; set; } = new Vector2(1, 1);

    internal void Play(bool overrideVolume = true)
    {
        
        if (source == null) return;
        SetSettings(overrideVolume);

        source.Play();
    }

    internal void PlayOneShot(bool overrideVolume = true)
    {
        if (source == null) return;

        SetSettings(overrideVolume);
        source.loop = false;

        source.Play();
    }

    internal void PlayAsSFX(bool overrideVolume = true)
    {
        if (source == null) return;

        SetSettings(overrideVolume);
        source.loop = false;

        source.PlayOneShot(Clip);
    }

    internal void Stop()
    {
        if (source == null) return;
        source.Stop();
    }
    internal void Pause()
    {
        if (source == null) return;
        source.Pause();
    }
    internal void UnPause()
    {
        if (source == null) return;
        source.UnPause();
    }
    internal async void FadeIn(float fadeTime = 1f, float resolution = 0.01f) // Seconds
    {
        if (source == null) return;
        SetSettings(false);
        CurrentVolume = 0;
        source.Play();
        await Fade(fadeTime, resolution);
    }
    internal async void FadeOut(float fadeTime = 1f, float resolution = 0.01f) // Seconds
    {
        if (source == null) return;
        await Fade(fadeTime, resolution, false);
    }
    private void SetSettings(bool overrideCurrentVolume = true)
    {
        if (overrideCurrentVolume)
        {
            CurrentVolume = GlobalVolume;
            if (EnableRandomVolume)
                CurrentVolume = UnityEngine.Random.Range(RandomVolumeRange.x, RandomVolumeRange.y);
        }

        source.pitch = Pitch;
        if (EnableRandomPitch)
            source.pitch = UnityEngine.Random.Range(RandomPitchRange.x, RandomPitchRange.y);

        source.panStereo = Pan;
        if (EnableRandomPan)
            source.panStereo = UnityEngine.Random.Range(RandomPanRange.x, RandomPanRange.y);

        source.time = StartPosition;
        source.loop = IsLoop;

    }  

    private async Task Fade(float fadeTime, float resolution, bool inFade = true)
    {

        float startVolume = CurrentVolume;
        float time = 0;
        float step = 0;

        if (!inFade) goto OutFade;

        while (step < 1)
        {
            if (time >= resolution)
            {      
                step +=  resolution / fadeTime;
                CurrentVolume = Mathf.Lerp(startVolume, GlobalVolume, step);
                time = 0;
            }
            time += Time.deltaTime;
            await Task.Yield();
        }
        return;

        OutFade:
        while (step < 1)
        {
            if (time >= resolution)
            {
                step += resolution / fadeTime;
                CurrentVolume = Mathf.Lerp(startVolume, 0, step);
                time = 0;
            }
            time += Time.deltaTime;
            await Task.Yield();
        }
        Stop();
        return;
    }
}
