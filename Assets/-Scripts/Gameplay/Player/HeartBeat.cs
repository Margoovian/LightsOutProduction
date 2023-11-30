using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    // to be continued

    enum HeartBeatStates
    {
        HeartBeatStartle = 1,
        HeartBeatPanic,
    }

    void Beating(HeartBeatStates HeartStates)
    {
        switch (HeartStates)
        {
            case HeartBeatStates.HeartBeatStartle:
                AudioManager.Instance.PlaySFX("HeartBeat");
                break;

            case HeartBeatStates.HeartBeatPanic:
                AudioManager.Instance.PlaySFX("TestSound");
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        //AudioManager.Instance.("HeartBeat2");
    }

    // Update is called once per frame
    void Update()
    {
        // THIS IS A BAD IDEA TO PUT IN UPDATE. DONT KNOW WHERE TO MAKE/PUT IT

        float fear = GameManager.Instance.PlayerData.FearLevel / GameManager.Instance.GameSettings.MaxFear;

        if (fear < 0.50)
        {
            //Beating(HeartBeatStates.HeartBeatStartle);
        }

        else if (fear <= 0.80)
        {
            //Beating(HeartBeatStates.HeartBeatPanic);
        }
    }
}
