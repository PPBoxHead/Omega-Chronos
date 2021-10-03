using UnityEngine;

public class KeybindingsManager : MonoBehaviour
{
    // manages keybindings
    #region Variables
    #region KeyCodes
    private KeyCode pauseButton = KeyCode.Escape;
    private KeyCode jumpButton = KeyCode.Space;
    private KeyCode dashButton = KeyCode.LeftShift;
    private KeyCode slowmoButton = KeyCode.L;
    #endregion
    private static KeybindingsManager instance;
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
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    // set & get
    public static KeybindingsManager GetInstance
    {
        get { return instance; }
    }

    public KeyCode GetJumpButton
    {
        get { return jumpButton; }
    }

    public KeyCode GetPauseButton
    {
        get { return pauseButton; }
    }

    public KeyCode GetDashButton
    {
        get { return dashButton; }
    }

    public KeyCode GetSlowmoButton
    {
        get { return slowmoButton; }
    }
    #endregion
}
