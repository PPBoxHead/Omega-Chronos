using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Button playBtn;
    private string startingLevel = "SampleScene";
    [SerializeField] private SavesManager savesManager;

    void Start()
    {
        // esto pasar a un script aparte
        // y revisar si hay un eventsystem
        // si tiene -> eliminarlo
        if (playBtn != null)
        {
            playBtn.Select();
        }
    }

    public void LoadScene(string value)
    {
        SceneManager.LoadScene(value);
    }

    public void GoToNextLevel(string value)
    {
        if (savesManager != null) savesManager.DeleteSaves();
        PlayerPrefs.SetString("scene", value);
        LoadScene(value);
    }

    public void PlayGame()
    {
        if (PlayerPrefs.HasKey("scene"))
        {
            startingLevel = PlayerPrefs.GetString("scene");
        }
        LoadScene(startingLevel);
    }

    public void PauseMenu()
    {
        GameManager.GetInstance.PauseGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
