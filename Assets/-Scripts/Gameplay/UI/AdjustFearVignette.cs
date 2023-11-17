using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustFearVignette : MonoBehaviour
{
    [field: SerializeField] public AnimationClip Animation { get; set; }
    [field: SerializeField] public GameObject Target { get; set; }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!Animation || !Target) return;
        
        if (GameManager.Instance)
        {
            Animation.SampleAnimation(Target, Mathf.LerpAngle(0, 1, GameManager.Instance.PlayerData.FearLevel / GameManager.Instance.GameSettings.MaxFear));
        }
    }
}
