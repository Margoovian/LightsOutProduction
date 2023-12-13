using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class LightCountLabel : MonoBehaviour
{
    [field: Header("Assets")]
    [field: SerializeField] public TMP_Text TextLabel { get; set; }
    [field: SerializeField] public Image Icon { get; set; }

    [field: Header("Icon Sprites")]
    [field: SerializeField] public Sprite OnSprite { get; set; }
    [field: SerializeField] public Sprite OffSprite { get; set; }

    [field: Header("Configuration")]
    [field: SerializeField] public bool AdjustTextColor { get; set; }

    private string previousText;

    public void UpdateLabel(int current, int max)
    {
        if (AdjustTextColor)
        {
            Color color = new(1, 1, 1, 1);

            if (current <= 0)
                color = new(0, 1, 0, 1);

            TextLabel.color = color;
        }

        TextLabel.text = current.ToString() + " / " + max.ToString();
    }

    public void UpdateIcon(int current)
    {
        if (TextLabel.text == previousText)
            return;

        Sprite sprite = OnSprite;

        if (current <= 0)
        {
            sprite = OffSprite;
            AudioManager.Instance.PlaySFX("DoorOpen");
        }

        Icon.sprite = sprite;
        previousText = TextLabel.text;
    }

    private void Update()
    {
        if (!LevelController.Instance)
            return;

        int current = LevelController.Instance.GetCurrentValue();
        
        UpdateLabel(current, LevelController.Instance.MaxLights);
        UpdateIcon(current);
    }
}
