using System;
using UnityEngine;

//public class AudioManager : MonoBehaviour
//{
//    public static AudioManager Instance { get; internal set; }
//    private void Awake() { if (!Instance) Instance = this; }
//}

[Serializable]
public class Sound
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

    public Sound[] music, sounds;
    public AudioSource musicSource, soundSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(this); // ...but why though?
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(music, x => x.nameIt == name);

        if (s == null)
        {
            Debug.Log(name + " not found in array: " + music.ToString() + "!", this);
            return;
        }

        musicSource.clip = s.clip;

        if (s.isLoopable)
            musicSource.loop = true;

        musicSource.Play();
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(music, x => x.nameIt == name);

        if (s == null)
        {
            Debug.Log(name + " not found in array: " + music.ToString() + "!", this);
            return;
        }

        if (s.clip != musicSource.clip)
        {
            Debug.Log(name = "'s clip isn't equal to " + musicSource.name + "'s clip!", this);
            return;
        }

        if (!musicSource.isPlaying)
        {
            Debug.Log(musicSource.name + " isn't playing any music!", this);
            return;
        }

        musicSource.Stop();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sounds, x => x.nameIt == name);

        if (s == null)
        {
            Debug.Log(name + " not found in array: " + sounds.ToString() + "!", this);
            return;
        }

        if (soundSource.isPlaying)
            return;

        soundSource.loop = s.isLoopable;
        soundSource.clip = s.clip;
        soundSource.Play();
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