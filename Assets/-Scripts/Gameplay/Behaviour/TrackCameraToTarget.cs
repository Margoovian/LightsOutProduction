using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCameraToTarget : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.Camera.LookAt = transform;
        GameManager.Instance.Camera.Follow = transform;
    }
}
