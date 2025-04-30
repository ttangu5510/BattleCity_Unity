using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class OptionFunction : MonoBehaviour
{

    public TMP_Dropdown resolutionsDropdown;
    List<Resolution> resolutions = new List<Resolution>();
    public int resolutionNum;

    void Start()
    {
        resolutions.AddRange(Screen.resolutions);
        resolutionsDropdown.options.Clear();

        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRateRatio + "hz";
            resolutionsDropdown.options.Add(option);

            if(item.width == Screen.width && item.height == Screen.height)
                resolutionsDropdown.value = optionNum;
            optionNum++;

          
        }
        TMP_Dropdown.OptionData option1 = new TMP_Dropdown.OptionData();
        option1.text = " ";
        resolutionsDropdown.options.Add(option1);

        resolutionsDropdown.RefreshShownValue();
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }
}
