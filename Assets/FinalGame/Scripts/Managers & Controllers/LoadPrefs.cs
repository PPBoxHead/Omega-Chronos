using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

    //this script lets have a default values in the options settings and load them when is build

public class LoadPrefs : MonoBehaviour
{
    
    [Header("General Setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;

    [Header("Brightness Setting")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;

    [Header("Quality Level Setting")]
    [SerializeField] private TMP_Dropdown qualityDrowpdown;

    [Header("Fullscreen Setting")]
    [SerializeField] private Toggle fullScreenToggle;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text musicTextValue = null;
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private TMP_Text sfxTextValue = null;
    [SerializeField] private Slider sfxSlider = null;

       void Awake()
    {
        if(canUse)
        {
            if(PlayerPrefs.HasKey("musicVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("musicVolume");

                musicTextValue.text = localVolume.ToString("0");
                musicSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            if(PlayerPrefs.HasKey("sfxVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("sfxVolume");

                sfxTextValue.text = localVolume.ToString("0");
                sfxSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetValues("Audio");
            }

            if(PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qualityDrowpdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if(PlayerPrefs.HasKey("masterFullscreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("masterFullscreen");

                if(localFullscreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    fullScreenToggle.isOn = false;
                }
            }

            if(PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                brightnessTextValue.text = localBrightness.ToString("0.0");
                brightnessSlider.value = localBrightness;

            }
        }
    }

}
