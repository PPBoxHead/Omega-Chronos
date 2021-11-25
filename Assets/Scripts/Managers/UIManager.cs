using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Variables
    #region Slowmo UI
    [SerializeField] private Image[] slowmoBar;
    #endregion
    #region Hitpoints UI
    private TMP_Text hitPointsText;
    private static UIManager instance;
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

        // Hitpoints UI
        hitPointsText = GameObject.Find("UI/HitpointsTxt").GetComponent<TMP_Text>();
    }

    public void UpdateBoostValue(float slowdownTime, float value)
    {
        float slowmoValue = value / slowdownTime;
        int bars = Mathf.RoundToInt(slowmoValue * slowmoBar.Length);

        // activates sprites
        for (int i = 0; i < Mathf.RoundToInt(slowmoValue * slowmoBar.Length); i++)
        {
            slowmoBar[i].gameObject.SetActive(true);
        }

        // turns off sprites
        for (int i = bars; i < slowmoBar.Length; i++)
        {
            slowmoBar[i].gameObject.SetActive(false);
        }
    }

    public void UpdateHitPoints(int currentHitPoints)
    {
        hitPointsText.text = "Health: " + currentHitPoints.ToString();
    }
    #endregion

    #region Setters/Getters
    public static UIManager GetInstance
    {
        get { return instance; }
    }
    #endregion
}
