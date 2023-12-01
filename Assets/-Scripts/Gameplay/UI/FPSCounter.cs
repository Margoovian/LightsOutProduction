using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [field: SerializeField] public TMP_Text TextLabel { get; set; }

    public void SetValue(bool value) => TextLabel.enabled = value;
    private void Update() => TextLabel.text = "FPS: " + (int)(1.0f / Time.smoothDeltaTime);
}
