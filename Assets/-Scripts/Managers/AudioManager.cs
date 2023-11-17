using System;
using UnityEngine;

//public class AudioManager : MonoBehaviour
//{
//    public static AudioManager Instance { get; internal set; }
//    private void Awake() { if (!Instance) Instance = this; }
//}

[Serializable]
public class Audio
{
    public void Play()
    {
        AudioManager.Instance.PlaySFX(nameIt);
    }

    public string nameIt;
    public AudioClip clip;
    public bool isLoopable;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Audio[] music, sounds;
    public AudioSource musicSource, soundSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Audio a = Array.Find(music, x => x.nameIt == name);

        if (a == null)
        {
            Debug.Log(name + " not found in array: " + music.ToString() + "!", this);
            return;
        }

        musicSource.clip = a.clip;
        musicSource.loop = a.isLoopable;
        musicSource.Play();
    }

    public void StopMusic(string name)
    {
        Audio a = Array.Find(music, x => x.nameIt == name);

        if (a == null)
        {
            Debug.Log(name + " not found in array: " + music.ToString() + "!", this);
            return;
        }

        if (a.clip != musicSource.clip)
            return;

        if (!musicSource.isPlaying)
            return;

        musicSource.Stop();
    }

    public void PlaySFX(string name)
    {
        Audio a = Array.Find(sounds, x => x.nameIt == name);

        if (a == null)
        {
            Debug.Log(name + " not found in array: " + sounds.ToString() + "!", this);
            return;
        }

        if (soundSource.isPlaying)
            return;

        soundSource.loop = a.isLoopable;
        soundSource.clip = a.clip;
        soundSource.Play();
    }

    public void StopSFX(string name)
    {
        Audio a = Array.Find(sounds, x => x.nameIt == name);

        if (a == null)
        {
            Debug.Log(name + " not found in array: " + sounds.ToString() + "!", this);
            return;
        }

        if (a.clip != soundSource.clip)
            return;

        if (!soundSource.isPlaying)
            return;

        soundSource.Stop();
    }

    public void ToggleLoopableAudio()
    {
        musicSource.mute = !musicSource.mute;

    }
    public void ToggleSFX()
    {
        soundSource.mute = !soundSource.mute;

    }
}

[CreateAssetMenu(menuName = "scriptableObjects/SFX", fileName = "SFX")]
public class SFX : ScriptableObject
{
    public virtual void Play()
    {
        AudioManager.Instance.PlaySFX(nameIt);
    }

    public string nameIt;
    public AudioClip clip;
    public bool isLoopable;
}

[CreateAssetMenu(menuName = "scriptableObjects/Music", fileName = "Music")]
public class Music : SFX
{
    public override void Play()
    {
        AudioManager.Instance.PlayMusic(nameIt);
    }
}