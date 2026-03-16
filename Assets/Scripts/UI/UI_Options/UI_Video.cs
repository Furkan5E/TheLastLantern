using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Video : UIScreen
{
    [Header("Video Settings UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    [SerializeField] private TMP_Dropdown frameRateDropdown;
    [SerializeField] private Toggle vSyncToggle;

    private Resolution[] resolutions;

    protected override void Awake()
    {
        base.Awake();
        
        SetupResolutionDropdown();
        
        displayModeDropdown.onValueChanged.AddListener(SetDisplayMode);
        frameRateDropdown.onValueChanged.AddListener(SetFrameRateLimit);
        vSyncToggle.onValueChanged.AddListener(SetVSync);
    }

    private void SetupResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height} @ {Mathf.RoundToInt((float)resolutions[i].refreshRateRatio.value)}hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    private void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }

    private void SetDisplayMode(int modeIndex)
    {
        FullScreenMode mode = FullScreenMode.ExclusiveFullScreen;
        switch (modeIndex)
        {
            case 0: mode = FullScreenMode.ExclusiveFullScreen; break;
            case 1: mode = FullScreenMode.FullScreenWindow; break; // Borderless
            case 2: mode = FullScreenMode.Windowed; break;
        }
        Screen.fullScreenMode = mode;
    }

    private void SetFrameRateLimit(int limitIndex)
    {
        int targetFPS = -1; //uncapped
        switch (limitIndex)
        {
            case 0: targetFPS = 30; break;
            case 1: targetFPS = 60; break;
            case 2: targetFPS = 120; break;
            case 3: targetFPS = -1; break;
        }
        Application.targetFrameRate = targetFPS;
    }

    private void SetVSync(bool isEnabled)
    {
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
    }
}