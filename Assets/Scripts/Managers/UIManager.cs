using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Variables
    #region Slowmo UI
    private GameObject slowmoBar;
    private RectTransform valueBar;
    private float maxBarValue;
    #endregion
    #endregion

    #region Methods
    private void Start()
    {
        slowmoBar = GameObject.Find("UI/SlowMotionIndicator");
        valueBar = GameObject.Find("UI/SlowMotionIndicator/Value").GetComponent<RectTransform>();
        maxBarValue = valueBar.rect.width;

    }
    public void UpdateBoostValue(float slowdownTime, float value)
    {
        valueBar.sizeDelta = new Vector2(value / slowdownTime * maxBarValue, valueBar.sizeDelta.y);
    }
    #endregion
}
