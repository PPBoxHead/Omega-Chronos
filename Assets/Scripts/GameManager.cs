using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    #region Variabes
    #region Setup
    private SavesManager savesManager;
    private LevelManager levelManager;
    private TimeManager timeManager;
    private UIManager uIManager;
    #endregion
    #region Pause
    public delegate void OnGamePaused(bool paused);
    public event OnGamePaused onGamePaused;

    private bool gamePaused = false;
    private KeyCode pauseBtn;
    #endregion
    #region Death
    public delegate void OnDeath(float duration);
    public event OnDeath onDeath;
    public GameObject deathText;
    private float duration = 2f;
    #endregion
    #region Singleton
    private static GameManager instance;
    #endregion
    #endregion

    #region Methods
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        savesManager = GetComponent<SavesManager>();
        levelManager = GetComponent<LevelManager>();
        timeManager = GetComponent<TimeManager>();
        uIManager = GetComponent<UIManager>();
    }

    void Start()
    {
        pauseBtn = KeybindingsManager.GetInstance.GetPauseButton;
    }

    public void OnPause()
    {
        PauseGame();
    }

    public void PauseGame()
    {
        gamePaused = !gamePaused;
        LoadInGameMenu();

        if (onGamePaused != null)
        {
            onGamePaused(gamePaused);
        }
    }

    void LoadInGameMenu()
    {
        if (SceneManager.GetSceneByName("PauseMenu").isLoaded)
        {
            SceneManager.UnloadSceneAsync("PauseMenu");
        }
        else
        {
            SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
        }
    }

    public void PlayerDeath()
    {
        if (onDeath != null)
        {
            onDeath(duration);
            StartCoroutine("DeathMessage", duration);
        }
    }

    IEnumerator DeathMessage(float duration)
    {
        deathText.SetActive(true);
        yield return new WaitForSeconds(duration);
        deathText.SetActive(false);
    }

    void OnDestroy()
    {
        if (instance != this)
        {
            instance = this;
        }
    }
    #endregion


    #region Getters/Setters
    public SavesManager GetSavesManager
    {
        get { return savesManager; }
    }

    public LevelManager GetLevelManager
    {
        get { return levelManager; }
    }

    public TimeManager GetTimeManager
    {
        get { return timeManager; }
    }

    public UIManager GetUIManager
    {
        get { return uIManager; }
    }
    public static GameManager GetInstance
    {
        get { return instance; }
    }
    #endregion

}
