using TMPro;
using UnityEngine;

public class LightCountLabel : MonoBehaviour
{
    private TMP_Text TextLabel;
    private bool CanRun;

    public void UpdateLabel(int current, int max) => TextLabel.text = current.ToString() + " / " + max.ToString();

    private void Update()
    {
        if (!CanRun)
            return;

        (int, int) values = LevelController.Instance.GetValues();
        UpdateLabel(values.Item1, values.Item2);
    }

    private void Start()
    {
        if (TextLabel == null)
            TextLabel = GetComponent<TMP_Text>();

        CanRun = LevelController.Instance;
    }
}
