using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    #region Pause
    public delegate void OnGamePaused(bool paused);
    public event OnGamePaused onGamePaused;
    public bool gamePaused = false;
    private KeyCode pauseButton;
    #endregion
    #region Death
    public delegate void OnDeath(float duration);
    public event OnDeath onDeath;
    public GameObject deathText;
    private float duration = 2f;
    #endregion
    private static GameManager instance;

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
    }

    void Start()
    {
        pauseButton = KeybindingsManager.GetInstance.GetPauseButton;
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseButton))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        gamePaused = !gamePaused;
        LoadMenuScene();

        if (onGamePaused != null)
        {
            onGamePaused(gamePaused);
        }
    }

    void LoadMenuScene()
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

    public static GameManager GetInstance
    {
        get { return instance; }
    }
}
