using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; internal set; }
    private void Awake() { if (!Instance) Instance = this; }
}
