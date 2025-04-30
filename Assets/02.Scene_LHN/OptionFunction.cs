using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionFunction : MonoBehaviour
{

    public Dropdown resolutionsDropdown;
    List<Resolution> resolutions = new List<Resolution>();

    void Update()
    {
        resolutions.AddRange(Screen.resolutions);
        resolutionsDropdown.options.Clear();

        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRateRatio + "hz";
            resolutionsDropdown.options.Add(option);
        }
        resolutionsDropdown.RefreshShownValue();
    }
}
