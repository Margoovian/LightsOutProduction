using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    float delay = 0;
    public Stem heartBeat;

    enum HeartBeatStates
    {
        // Definitions (used later)
        HeartBeatStable = 0,
        HeartBeatStartle,
        HeartBeatPanic,
    }

    void Beating(HeartBeatStates HeartStates)
    {
        // cooldown delay between "loops"
        delay += Time.deltaTime;

        switch (HeartStates)
        {
            case HeartBeatStates.HeartBeatStable:

                if (delay > 1.5)
                {
                    //AudioManager.Instance.AudioMixer.SetFloat();
                    heartBeat.Pitch = 0.7f;
                    heartBeat.GlobalVolume = 0.4f;
                    AudioManager.Instance.PlaySFX("HeartBeat");

                    delay = 0;
                }
                break;

            case HeartBeatStates.HeartBeatStartle:

                if (delay > 0.8)
                {
                    heartBeat.Pitch = 0.8f;
                    heartBeat.GlobalVolume = 0.7f;
                    AudioManager.Instance.PlaySFX("HeartBeat");
                    delay = 0;
                }
                break;

            case HeartBeatStates.HeartBeatPanic:
                if (delay > 0.55)
                {
                    heartBeat.Pitch = 0.9f;
                    heartBeat.GlobalVolume = 1f;
                    AudioManager.Instance.PlaySFX("HeartBeat");
                    delay = 0;
                }
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        float fear = GameManager.Instance.PlayerData.FearLevel / GameManager.Instance.GameSettings.MaxFear;

        // Manually adjested to correspond to "EyeGestures" (more or less)

        // Worried - Scared
        if (fear >= 0.34 && fear <= 0.67)
        {
            Beating(HeartBeatStates.HeartBeatStable);
        }
        
        // Scared - Death
        else if (fear > 0.67 && fear < 0.88)
        {
            Beating(HeartBeatStates.HeartBeatStartle);
        }

        // Death - |||
        else if (fear >= 0.88)
        {
            Beating(HeartBeatStates.HeartBeatPanic);
        }
        
    }
}