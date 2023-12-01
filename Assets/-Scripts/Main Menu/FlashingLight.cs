using UnityEngine;

public class FlashingLight: MonoBehaviour
{
    public float minFlashInterval = 0.2f;
    public float maxFlashInterval = 0.8f;

    private Light lightFlash;
    private float timer;

    private void Start()
    {
        lightFlash = GetComponent<Light>();
        timer = Random.Range(minFlashInterval, maxFlashInterval);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ToggleLight();
            timer = Random.Range(minFlashInterval, maxFlashInterval);
        }
    }

    private void ToggleLight()
    {
        if (lightFlash != null)
        {
            lightFlash.enabled = !lightFlash.enabled;
        }
    }
}