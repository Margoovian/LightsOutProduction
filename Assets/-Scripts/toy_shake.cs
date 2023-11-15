using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toy_shake : MonoBehaviour
{
    public AudioSource glowtoySound;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            glowtoySound.enabled = true;
        }
        else
        {
            glowtoySound.enabled = false;
        }
    }
}