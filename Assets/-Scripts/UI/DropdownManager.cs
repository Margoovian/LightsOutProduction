using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownManager : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public bool ignoreListables;
    public List<TMP_Dropdown.OptionData> listables = new();

    void Start()
    {
        if (ignoreListables)
            return;

        if (dropdown.options.Count > 0)
            dropdown.ClearOptions();

        dropdown.AddOptions(listables);
    }
}
