using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Variables
    #region Setup
    [SerializeField] private bool dontLoadHere = false;
    private SavesManager savesManager;
    #endregion
    #endregion

    #region Methods
    void Start()
    {
        // ver de sacar esto en el menu
        if (!dontLoadHere)
        {
            savesManager = GameManager.GetInstance.GetSavesManager;
        }
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
