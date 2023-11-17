using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class SlideManager : MonoBehaviour
{
    [Header("Inputs")]
    public Slider slider;
    public TMP_InputField inputField;
    
    [Header("Rounding")]
    public bool useRounding;
    public int roundToSig;

    [Header("Miscellaneous")]
    public bool usePercentage;
    public bool skipFormatting;

    private void SliderUpdated(float newValue)
    {
        string endStr = "%";
        if (!usePercentage)
            endStr = string.Empty;

        if (useRounding)
            newValue = (float)Math.Round(newValue, roundToSig);

        inputField.text = newValue.ToString() + endStr;
    }

    private void FieldUpdated(string newValue)
    {
        string endStr = "%";
        if (!usePercentage)
            endStr = string.Empty;

        inputField.text = newValue + endStr;
        float.TryParse(newValue, out float result);

        if (result > slider.maxValue)
            result = slider.maxValue;

        if (result < slider.minValue)
            result = slider.minValue;

        if (useRounding)
            result = (float)Math.Round(result, roundToSig);

        slider.value = result;
    }

    private void Start()
    {
        if (!skipFormatting)
        {
            string endStr = "%";
            if (!usePercentage)
                endStr = string.Empty;

            inputField.text = slider.minValue.ToString() + endStr;
        }

        slider.onValueChanged.AddListener(SliderUpdated);
        inputField.onEndEdit.AddListener(FieldUpdated);
    }
}
