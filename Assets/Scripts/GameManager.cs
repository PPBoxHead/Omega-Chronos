using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    #region Variabes
    #region Setup
    [SerializeField] private Tilemap tilemap;
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
    // super cutre pero it works (❁´◡`❁)
    [SerializeField] private GameObject deathAnim;
    [SerializeField] private Animator deathAnimText;
    [SerializeField] private GameObject DeathTest;
    public delegate void OnDeath(float duration);
    public event OnDeath onDeath;
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

        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
    }

    private void Start()
    {
        AudioManager.Getinstance.MusicSelector();
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
        // if (SceneManager.GetActiveScene().name == "Lvl04")
        // {
        //     // como esta solo para este caso lo hice asi nomas
        //     // se puede dejar mas lindo
        //     timeManager.ResetTime();
        //     SceneManager.LoadScene("Lvl04");
        //     return;
        // }

        if (onDeath != null)
        {
            onDeath(duration);
            StartCoroutine("DeathMessage", duration);
        }
    }

    IEnumerator DeathMessage(float duration)
    {
        deathAnim.SetActive(true);
        deathAnimText.Play("Text");

        if (SceneManager.GetActiveScene().name == "Lvl04")
        {
            DeathTest.SetActive(true);
            yield return new WaitForSeconds(duration);
            timeManager.ResetTime();
            SceneManager.LoadScene("Lvl04");
        }

        yield return new WaitForSeconds(duration);
        deathAnim.SetActive(false);
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

    public Tilemap GetTilemap
    {
        get { return tilemap; }
    }

    public static GameManager GetInstance
    {
        get { return instance; }
    }
    #endregion

}
