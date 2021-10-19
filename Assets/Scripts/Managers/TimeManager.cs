using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    #region Delegates
    public delegate void OnSlowMotion(bool paused);
    public event OnSlowMotion onSlowMotion;
    #endregion

    #region Variables

    #region Setup
    private KeyCode slowmoBtn;

    private float gravityScale;
    private float originalTime;
    private float originalDeltaTime;

    private bool isTimeSlow = false;
    private float timer;

    private GameObject slowmoBar;
    private RectTransform valueBar;
    private float maxBarValue;
    #endregion

    #region SlowdownParameters
    [Range(0.5f, 0.01f)] [SerializeField] private float slowdownFactor = 0.05f; // controls how low it's gonna be the time
    [Range(2f, 8f)] [SerializeField] private float slowdownTime = 5; // how long is gona be the slow
    #endregion
    #endregion

    #region Methods
    void Start()
    {
        slowmoBar = GameObject.Find("UI/SlowMotionIndicator");
        valueBar = GameObject.Find("UI/SlowMotionIndicator/Value").GetComponent<RectTransform>();

        maxBarValue = valueBar.rect.width;

        originalDeltaTime = Time.fixedDeltaTime;
        originalTime = Time.timeScale;

        slowmoBtn = KeybindingsManager.GetInstance.GetSlowmoButton;

        timer = slowdownTime;
    }

    void Update()
    {
        //arreglar aca
        if (Input.GetKeyDown(slowmoBtn) && !isTimeSlow)
        {
            StopAllCoroutines();
            SlowMotion();
        }
        else if (Input.GetKeyDown(slowmoBtn) && isTimeSlow)
        {
            StopAllCoroutines();
            StartCoroutine("Co_Recharge");
        }
    }

    void SlowMotion()
    {
        //arreglar aca
        isTimeSlow = true;
        //Time.timeScale = slowdownFactor;
        // always deltaTime = timeScale * 0.02
        //Time.fixedDeltaTime = Time.timeScale * 0.02f;
        StartCoroutine("Co_SlowTime");
        ManageSlowMotion();
    }

    IEnumerator Co_SlowTime()
    {
        //arreglar aca
        // pasar numeros a variables
        while (timer > 0)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, slowdownFactor, 10f * Time.unscaledDeltaTime);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            timer -= Time.unscaledDeltaTime;
            UpdateBoostValue(timer);
            yield return null;
        }

        StartCoroutine("Co_Recharge");
    }

    IEnumerator Co_Recharge()
    {
        //arreglar aca
        // pasar numeros a variables
        isTimeSlow = false;
        while (timer <= slowdownTime)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, 10f * Time.unscaledDeltaTime);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            timer += Time.unscaledDeltaTime;
            UpdateBoostValue(timer);
            yield return null;
        }
        // para dejar los valores exactos y no luego de un lerp
        timer = slowdownTime;
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    void UpdateBoostValue(float value)
    {
        valueBar.sizeDelta = new Vector2(value / slowdownTime * maxBarValue, valueBar.sizeDelta.y);
    }

    void ManageSlowMotion()
    {
        if (onSlowMotion != null)
        {
            onSlowMotion(isTimeSlow);
        }
    }
    #endregion
}
