using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    // to be continued

    float delay = 0;

    public Stem heartBeat;

    enum HeartBeatStates
    {
        HeartBeatStable = 0,
        HeartBeatStartle,
        HeartBeatPanic,
    }

    void Beating(HeartBeatStates HeartStates)
    {
        delay += Time.deltaTime;

        switch (HeartStates)
        {
            case HeartBeatStates.HeartBeatStable:

                if (delay > 1.5)
                {
                    //AudioManager.Instance.AudioMixer.SetFloat();
                    heartBeat.Pitch = 0.7f;
                    heartBeat.GlobalVolume = 0.45f;
                    AudioManager.Instance.PlaySFX("HeartBeat");


                    delay = 0;
                }
                break;

            case HeartBeatStates.HeartBeatStartle:

                if (delay > 0.7)
                {
                    heartBeat.Pitch = 0.8f;
                    heartBeat.GlobalVolume = 0.7f;
                    AudioManager.Instance.PlaySFX("HeartBeat");
                    delay = 0;
                }
                break;

            case HeartBeatStates.HeartBeatPanic:
                if (delay > 0.4)
                {
                    heartBeat.Pitch = 0.8f;
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
        // THIS IS A BAD IDEA TO PUT IN UPDATE. DONT KNOW WHERE TO MAKE/PUT IT

        float fear = GameManager.Instance.PlayerData.FearLevel / GameManager.Instance.GameSettings.MaxFear;

        if (fear >= 0.34 && fear <= 0.67)
        {
            Beating(HeartBeatStates.HeartBeatStable);
        }

        else if (fear > 0.67 && fear < 0.88)
        {
            Beating(HeartBeatStates.HeartBeatStartle);
        }

        else if (fear >= 0.88)
        {
            Beating(HeartBeatStates.HeartBeatPanic);
        }
        
    }
}