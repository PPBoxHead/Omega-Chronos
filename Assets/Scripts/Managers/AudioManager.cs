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
        Walking,
    }
    public enum SFX
    {
        Dialogue,
    }
    #endregion
    #region Variables
    float volumeRange = 0.1f;
    float pitchRange = 0.2f;
    #region AudioSources
    #region BackgroundClips
    [Header("Background Clips")]
    [SerializeField] private AudioSource backgroundSource;
    [SerializeField] private AudioClip[] backgroundClips;
    #endregion
    #region CharacterSFXClips
    [Header("Character SFX Clips")]
    [SerializeField] private AudioSource characterSFXSource;
    [SerializeField] private AudioClip[] characterSFXClips;
    #endregion
    #region SFXClips
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] sfxClips;
    #endregion
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
        PlayMusic(BackgroundMusic.MainMenu);
    }

    #region BackgroundMusic
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

    #region CharacterSFX
    public void PlayCharacterSFX(CharacterSFX characterSFXClip)
    {
        if (!characterSFXSource.isPlaying)
        {
            switch (characterSFXClip)
            {
                case CharacterSFX.Walking:
                    RandomizeSound(characterSFXClips[(int)CharacterSFX.Walking], characterSFXSource);
                    break;
            }
        }
    }
    #endregion

    // se podrian hacer las 2 juntas (playcharactersfx y playsfx)
    // pero... bueno
    #region SFX
    public void PlaySFX(SFX sfxClip)
    {
        if (!sfxSource.isPlaying)
        {
            switch (sfxClip)
            {
                case SFX.Dialogue:
                    RandomizeSound(sfxClips[(int)SFX.Dialogue], sfxSource);
                    break;
            }
        }
    }
    #endregion

    #region RandomizeSounds
    /// <Summary>
    /// randomizes sound pitch and volume
    /// </Summary>
    void RandomizeSound(AudioClip audioClip, AudioSource audioSource)
    {
        float startingVolume = 0.4f;
        float startingPitch = 0.9f;

        audioSource.clip = audioClip;
        audioSource.volume = GetRandom(startingVolume, volumeRange);
        audioSource.pitch = GetRandom(startingPitch, pitchRange);
        audioSource.Play();
    }

    /// <summary>
    /// Random.Range a little smaller to make it easier to read
    /// </summary>
    float GetRandom(float value, float range)
    {
        return Random.Range(value - range, value + range);
    }
    #endregion
    #endregion

    #region Getter/Setter
    public static AudioManager Getinstance
    {
        get { return instance; }
    }
    #endregion
}
