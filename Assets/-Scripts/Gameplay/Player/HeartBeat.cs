using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{   
    
    // to be continued
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlaySFX("HeartBeat1");
        AudioManager.Instance.Play("HeartBeat2");
    }

    void CreatingHeartBreats()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
