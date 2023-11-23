using TMPro;
using UnityEngine;

public class TrackLightCount : MonoBehaviour
{
    public static TrackLightCount Instance { get; internal set; }
    private TMP_Text TextLabel;

    public void Modify(int current, int max) => TextLabel.text = current.ToString() + " / " + max.ToString();

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    private void Start()
    {
        TextLabel = GetComponent<TMP_Text>();
    }
}
