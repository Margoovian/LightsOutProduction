using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Eyes
{
    Gesture1,
    Gesture2,
    Gesture3,
    Gesture4,
    Gesture5,
}
public class EyeGestures : MonoBehaviour
{
    [Header("Array of Eyes")]
    public Transform EyeGesture1;
    public Transform EyeGesture2;
    public Transform EyeGesture3;
    public Transform EyeGesture4;
    public Transform EyeGesture5;
}