using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Dictionary<string, Stem> Stems = new();
    [field: SerializeField] public GameObject AudioObject { get; set; }
    [field: SerializeField] public string AudioPath { get; set; } // This has to be relative to the Resource folder (I:E in it) 
    [field: SerializeField] public AudioMixerGroup MusicMixer { get; set; } = null;
    [field: SerializeField] public AudioMixerGroup SFXMixer { get; set; } = null;

    private void Awake() => Instance = this;
    
    void Start()
    {
        if(!AudioObject) { Debug.LogWarning("No Audio Object! (There will be no audio!)"); }

        Stem[] stems = Resources.LoadAll<Stem>(AudioPath);

        foreach(Stem stem in stems)
        {
            AudioSource source = AudioObject.AddComponent<AudioSource>();
            source.clip = stem.Clip;
            stem.source = source;

            RouteStem(stem);

            if (stem.FriendlyName == "" || Stems.ContainsKey(stem.FriendlyName))
                Stems.Add(stem.Clip.name, stem);
            else
                Stems.Add(stem.FriendlyName, stem);
        }
    }

    private void RouteStem(Stem stem) // Call after assigning source to stem
    {
        if (!MusicMixer || !SFXMixer) return;
        switch (stem.ClipType)
        {
            case StemType.Music: stem.source.outputAudioMixerGroup = MusicMixer; break;
            case StemType.SFX: stem.source.outputAudioMixerGroup = SFXMixer; break;
            default: break;
        }
    }

    internal bool IsPlaying(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) return stem.source.isPlaying;
        else return false;

    }
    internal void Play(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.Play();
    }

    internal void PlaySFX(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.PlayAsSFX();
    }

    internal void Stop(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.Stop();
    }

    internal void Pause(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.Pause();
    }

    internal void UnPause(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.UnPause();
    }

    internal void FadeIn(string clip, float timeInSeconds = 2)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.FadeIn(timeInSeconds);
    }

    internal void FadeOut(string clip, float timeInSeconds = 2)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.FadeOut(timeInSeconds);
    }


    internal void Crossfade(string clipA, string clipB, float timeInSeconds = 2) // ClipA is fade in, ClipB is fadeout
    {
        Stem stemA;
        Stem stemB;
        bool existsA = Stems.TryGetValue(clipA, out stemA);
        bool existsB = Stems.TryGetValue(clipB, out stemB);
        if (existsA && existsB)
        {
            stemA.FadeIn(timeInSeconds);
            stemB.FadeOut(timeInSeconds);
        }
    }
}
