using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    #region Enum
    public enum BackgroundMusic
    {
        Noise,
        // Menu Music
        StaticLife,
        // Menu Music
        // Lvl 00 music
        TalkWithMyMechanicalBrain,
        // Lvl 00 music
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
    private bool noiseTime = false;
    private bool fading = false;
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

    private void Update()
    {
        if (!fading)
        {
            if (noiseTime && !backgroundSource.isPlaying)
            {
                FadeMusic(BackgroundMusic.Noise);
                return;
            }

            if (!backgroundSource.isPlaying)
            {
                MusicSelector();
                return;
            }
        }
    }

    #region BackgroundMusic
    public void MusicSelector()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Menu":
                FadeMusic(BackgroundMusic.StaticLife);
                break;
            case "Lvl00":
                FadeMusic(BackgroundMusic.TalkWithMyMechanicalBrain);
                break;
        }
    }

    public void StopMusic()
    {
        FadeMusic(BackgroundMusic.Noise);
    }

    public void PlayMusic(BackgroundMusic backgroundClip)
    {
        switch (backgroundClip)
        {
            case BackgroundMusic.Noise:
                backgroundSource.clip = backgroundClips[(int)BackgroundMusic.Noise];
                break;
            case BackgroundMusic.StaticLife:
                backgroundSource.clip = backgroundClips[(int)BackgroundMusic.StaticLife];
                break;
            case BackgroundMusic.TalkWithMyMechanicalBrain:
                backgroundSource.clip = backgroundClips[(int)BackgroundMusic.TalkWithMyMechanicalBrain];
                break;
        }

        backgroundSource.Play();
    }

    /// <summary>
    /// fades out current music and plays the next one
    /// </summary>
    public void FadeMusic(BackgroundMusic musicClip)
    {
        fading = true;
        StopAllCoroutines(); // stops fade in/out, it helps when spamming this
        StartCoroutine(FadeOut(musicClip));
    }

    /// <summary>
    /// smoothly lowers music 
    /// </summary>
    IEnumerator FadeOut(BackgroundMusic musicClip)
    {
        float duration = 0.05f;
        float lowestVolume = 0.1f;

        while (backgroundSource.volume > lowestVolume)
        {
            backgroundSource.volume -= 0.01f;
            yield return new WaitForSeconds(duration);
        }

        PlayMusic(musicClip);
        noiseTime = !noiseTime;
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// smoothly turns music up
    /// </summary>
    IEnumerator FadeIn()
    {
        float duration = 0.05f;
        int highestVolume = 1;

        while (backgroundSource.volume < highestVolume)
        {
            backgroundSource.volume += 0.01f;
            yield return new WaitForSeconds(duration);
        }
        fading = false;
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
