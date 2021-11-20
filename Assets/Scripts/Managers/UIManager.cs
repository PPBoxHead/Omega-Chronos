using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Variables
    #region Slowmo UI
    [SerializeField] private Image[] slowmoBar;
    [SerializeField] private Sprite[] slowmoSprites;
    private int maxCeiling = 100;
    private int slowmoStep = 20;
    private int slowmoCeiling;
    private int maxValue = 3;
    private int test = -1;
    private int currentValue;
    #endregion
    #region Hitpoints UI
    private TMP_Text hitPointsText;
    #endregion
    #endregion

    #region Methods

    private void Awake()
    {
        // Hitpoints UI
        hitPointsText = GameObject.Find("UI/HitpointsTxt").GetComponent<TMP_Text>();
        slowmoCeiling = maxCeiling;
        currentValue = maxValue;
    }

    public void UpdateBoostValue(float slowdownTime, float value)
    {
        // valueBar.sizeDelta = new Vector2(value / slowdownTime * maxBarValue, valueBar.sizeDelta.y);
        float slowmoValue = value / slowdownTime * 100;

        if (slowmoValue > 20) slowmoBar[0].gameObject.SetActive(true);

        if (slowmoValue * -test < -test * (slowmoCeiling - slowmoStep))
        {
            currentValue += test;
            slowmoCeiling += slowmoStep * test;

            if (currentValue < 0)
            {
                slowmoBar[0].gameObject.SetActive(false);
                test *= -1;
                currentValue = 0;
                slowmoCeiling = 40;
                return;
            }

            Debug.Log(currentValue);

            slowmoBar[0].sprite = slowmoSprites[currentValue];
            return;
        }

        // if (slowmoValue >= slowmoCeiling + slowmoStep)
        // {
        //     test = 1;
        //     slowmoBar[0].gameObject.SetActive(true);

        //     slowmoCeiling += slowmoStep;
        //     currentValue += test;

        //     slowmoBar[0].sprite = slowmoSprites[currentValue];
        //     return;
        // }
    }

    public void UpdateHitPoints(int currentHitPoints)
    {
        hitPointsText.text = "Health: " + currentHitPoints.ToString();
    }
    #endregion
}
