using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownManager : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public bool ignoreListables;
    public bool carryPreviousValue;

    public List<TMP_Dropdown.OptionData> listables = new();

    private int previousValue;

    void Start()
    {
        if (ignoreListables)
            return;

        if (carryPreviousValue)
            previousValue = dropdown.value;

        if (dropdown.options.Count > 0)
            dropdown.ClearOptions();

        dropdown.AddOptions(listables);

        if (carryPreviousValue)
            dropdown.value = previousValue;
        
        dropdown.RefreshShownValue();
    }
}
