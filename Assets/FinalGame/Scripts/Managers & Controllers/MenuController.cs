using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public string newGame;
    private string loadedGame;
    [SerializeField] private GameObject noSavedGamePanel = null;
    [SerializeField] private GameObject noSavedGameButton;

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 0.1f;
    [SerializeField] private Image brightPanel;
    
    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDrowpdown;
    [SerializeField] private Toggle fullScreenToggle;
    private int qualityLevel;
    private bool isFullScreen;
    private float brightnessLevel;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [Header("Volume Settings")]
    //[SerializeField] private AudioMixerGroup masterMixer;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private TMP_Text masterTextValue = null;
    [SerializeField] private Slider masterSlider = null;
    [SerializeField] private TMP_Text musicTextValue = null;
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private TMP_Text sfxTextValue = null;
    [SerializeField] private Slider sfxSlider = null;
    [SerializeField] private float defaultVolume = 0.5f;
    [Space]
    [SerializeField] private GameObject confirmationPrompt = null;
    void Start() //resolutions list for the screen & cursor lock
    {
        // Esto bloquea la interacción del mouse y lo vuelve invisible
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    public void NewGame()
    {
        PlayerPrefs.DeleteKey("OCscene");
        PlayerPrefs.DeleteKey("OCcheckpointX");
        PlayerPrefs.DeleteKey("OCcheckpointY");
        Debug.Log("PlayerPrefs deleted");
        SceneManager.LoadScene("SampleScene");
    }
    public void LoadGame()
    {
        if(PlayerPrefs.HasKey("OCscene"))
        {
            loadedGame = PlayerPrefs.GetString("OCscene"); 
            SceneManager.LoadScene(loadedGame); 
        }
        else
        {
            noSavedGamePanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(noSavedGameButton);
        }
    }
    public void ExitGame()
    {
        Debug.Log("Goodbye pussy");
        Application.Quit();
    }

    public void SetBrightness(float brightness)
    {
        brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
        brightPanel.color = new Color(brightPanel.color.r, brightPanel.color.g, brightPanel.color.b, brightnessSlider.value);
    }
    public void SetFullScreen(bool fullScreen)
    {
        isFullScreen = fullScreen;
    }
    public void SetQuality(int qualityIndex)
    {
        qualityLevel = qualityIndex;
    }
    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", brightnessLevel);
        brightPanel.color = new Color(brightPanel.color.r, brightPanel.color.g, brightPanel.color.b, brightnessSlider.value);

        PlayerPrefs.SetInt("masterQuality", qualityLevel);
        QualitySettings.SetQualityLevel(qualityLevel);

        PlayerPrefs.SetInt("masterFullscreen", (isFullScreen ? 1 : 0));
        Screen.fullScreen = isFullScreen;

        StartCoroutine(Confirmationbox());
    }

    public void SetMasterVolume(float masterVolume)
    {
        AudioListener.volume = masterVolume;
        masterMixer.SetFloat("MasterVolumeExposed", Mathf.Log10(masterVolume) * 20);
        masterTextValue.text = masterVolume.ToString("0.0");
    }
    public void SetMusicVolume(float musicVolume) // poner el audio de la música 
    {
        //masterMixer.audioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolume) * 20); // esto aun se tiene que ver 
        AudioListener.volume = musicVolume;
        musicTextValue.text = musicVolume.ToString("0.0");
    }
    public void SetSFXVolume(float sfxVolume) // poner el audio de los sfx 
    {
        AudioListener.volume = sfxVolume;
        sfxTextValue.text = sfxVolume.ToString("0.0");
    }

    public void VolumeApply() // guarda los valores de audio
    {
        PlayerPrefs.SetFloat("musicVolume", AudioListener.volume);
        PlayerPrefs.SetFloat("sfxVolume", AudioListener.volume);
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        // Nos muestra que fue aplicado
        StartCoroutine(Confirmationbox());
    }

    public void ResetValues(string MenuType)
    {
        if(MenuType == "Graphics")
        {
            //reset Brightness value
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");
            brightPanel.color = new Color(brightPanel.color.r, brightPanel.color.g, brightPanel.color.b, brightnessSlider.value);

            //reset quality
            qualityDrowpdown.value = 3;
            QualitySettings.SetQualityLevel(3);
            //reset full screen
            fullScreenToggle.isOn = true;
            Screen.fullScreen = true;
            //reset resolution
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }

        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            masterSlider.value = defaultVolume;
            masterTextValue.text = defaultVolume.ToString("0.0");
            musicSlider.value = defaultVolume;
            musicTextValue.text = defaultVolume.ToString("0.0");
            sfxSlider.value = defaultVolume;
            sfxTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
    }
    
    public IEnumerator Confirmationbox()
    { 
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        confirmationPrompt.SetActive(false);
    }




    
}
