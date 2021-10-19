using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button playBtn;
    private string startingLevel = "SampleScene";
    [SerializeField] private SavesManager savesManager;

    public void LoadScene(string value)
    {
        SceneManager.LoadScene(value);
    }

    public void GoToNextLevel(string value)
    {
        if (savesManager != null) savesManager.DeleteSaves();
        PlayerPrefs.SetString("OCscene", value);
        LoadScene(value);
    }

    public void PlayGame()
    {
        if (PlayerPrefs.HasKey("OCscene"))
        {
            startingLevel = PlayerPrefs.GetString("OCscene");
        }
        LoadScene(startingLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
