using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MenuUtil : MonoBehaviour
{

    [Header("Settings")]
    public Dropdown qualityDropdown;
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;


    void Start()
    {

        // Quality
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
        qualityDropdown.value = QualitySettings.GetQualityLevel();

        // Resolutions
        Resolution[] screenResolutions = Screen.resolutions;
        Resolution currentRes = Screen.currentResolution;

        resolutionDropdown.ClearOptions();
        List<string> resolutions = new List<string>(screenResolutions.Length);
        int currentResolution = 0;

        for (int i = 0; i < screenResolutions.Length; i++)
        {
            string resolutionString = String.Format("{0} x {1}", screenResolutions[i].width, screenResolutions[i].height);

            if (resolutions.Contains(resolutionString))
                continue;

            resolutions.Add(resolutionString);

            if (currentRes.Equals(screenResolutions[i]))
                currentResolution = resolutions.Count - 1;
        }

        resolutionDropdown.AddOptions(resolutions);
        resolutionDropdown.value = currentResolution;

        // Fullscreen
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void OnQualitySelect(int index)
    {
        index = qualityDropdown.value;
        QualitySettings.SetQualityLevel(index);
        print(index);
    }

    public void OnResolutionSelect(int index)
    {
        index = resolutionDropdown.value;
        string[] resolutionStrings = resolutionDropdown.options[index].text.Split(new[] { " x " }, StringSplitOptions.RemoveEmptyEntries);

        Screen.SetResolution(int.Parse(resolutionStrings[0]), int.Parse(resolutionStrings[1]), Screen.fullScreen);
        print(index);
    }

    public void OnFullscreenToggle(bool value)
    {

        //make sure you have the current value of the toggle and take the inverse of that to toggle
        value = !fullscreenToggle.isOn;
        Screen.fullScreen = value;
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        Debug.Break();
#else
			Application.Quit();
#endif
    }

}
