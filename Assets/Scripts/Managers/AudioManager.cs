using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Enum
    public enum BackgroundMusic
    {
        MainMenu,
    }
    public enum CharacterSFX
    {

    }
    public enum SFX
    {

    }
    #endregion
    #region Variables
    #region AudioSources
    [SerializeField] private AudioClip[] backgroundClips;
    private AudioSource backgroundSource;
    #endregion
    #region Singleton
    private static AudioManager instance;
    #endregion
    #endregion

    #region Methods
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        backgroundSource = GameObject.Find("/AudioManager/BackgroundMusic").GetComponent<AudioSource>();
        PlayMusic(BackgroundMusic.MainMenu);
    }

    public void PlayMusic(BackgroundMusic backgroundClip)
    {
        switch (backgroundClip)
        {
            case BackgroundMusic.MainMenu:
                backgroundSource.clip = backgroundClips[(int)BackgroundMusic.MainMenu];
                break;
        }

        backgroundSource.Play();
    }
    #endregion

    #region Getter/Setter
    public static AudioManager Getinstance
    {
        get { return instance; }
    }
    #endregion
}
