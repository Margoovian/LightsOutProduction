using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//public class AudioManager : MonoBehaviour
//{
//    public static AudioManager Instance { get; internal set; }
//    private void Awake() { if (!Instance) Instance = this; }
//}

[System.Serializable]
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
        }

        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        PlayMusic("music");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(music, x => x.nameIt == name);
        if (s == null)
        {
            Debug.Log(name + " Not Found");
            return;
        }

        musicSource.clip = s.clip;

        if (s.isLoopable)
            musicSource.loop = true;

        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sounds, x => x.nameIt == name);
        if (s == null)
        {
            Debug.Log(name + " Not Found");
            return;
        }

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