using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Variables
    #region Slowmo UI
    private GameObject slowmoBar;
    private RectTransform valueBar;
    private float maxBarValue;
    #endregion
    #region Hitpoints UI
    private TMP_Text hitPointsText;
    #endregion
    #endregion

    #region Methods

    private void Awake()
    {
        // Slowmo UI
        slowmoBar = GameObject.Find("UI/SlowMotionIndicator");
        valueBar = GameObject.Find("UI/SlowMotionIndicator/Value").GetComponent<RectTransform>();
        maxBarValue = valueBar.rect.width;

        // Hitpoints UI
        hitPointsText = GameObject.Find("UI/HitpointsTxt").GetComponent<TMP_Text>();
    }

    public void UpdateBoostValue(float slowdownTime, float value)
    {
        valueBar.sizeDelta = new Vector2(value / slowdownTime * maxBarValue, valueBar.sizeDelta.y);
    }

    public void UpdateHitPoints(int currentHitPoints)
    {
        hitPointsText.text = "Health: " + currentHitPoints.ToString();
    }
    #endregion
}
