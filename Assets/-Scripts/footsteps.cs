using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footsteps : MonoBehaviour
{

    public AudioSource player_foot;

    void Update()
    {
        if (Input.GetKey("w")) || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d") ||{ 
            footsteps.enabled = true;
        
        else 
        {
            footsteps.enabled = false;
        }
}