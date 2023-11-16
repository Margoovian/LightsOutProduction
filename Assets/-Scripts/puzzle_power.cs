using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzle_power : MonoBehaviour
{
    public AudioSource puzzle_powerSound;

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            puzzle_powerSound.enabled = true;
        }
        else
        {
            puzzle_powerSound.enabled = false;
        }
    }
}