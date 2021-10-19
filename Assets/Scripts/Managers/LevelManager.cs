using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Variables
    #region Setup
    private SavesManager savesManager;
    #endregion
    #endregion

    #region Methods
    void Start()
    {
        savesManager = GameManager.GetInstance.GetSavesManager;
    }

    public void LoadScene(string value)
    {
        SceneManager.LoadScene(value);
    }

    public void GoToNextLevel(string value)
    {
        if (savesManager != null)
        {
            savesManager.DeleteSaves();
            savesManager.SaveScene(value);
        }
        LoadScene(value);
    }
    #endregion
}
