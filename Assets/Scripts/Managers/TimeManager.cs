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
    private float originalTime;
    private float originalDeltaTime;

    private bool isTimeSlow = false;
    private float timer;

    private UIManager uIManager;
    #endregion
    #region SlowdownParameters
    [Range(0.5f, 0.01f)] [SerializeField] private float slowdownFactor = 0.05f; // controls how low it's gonna be the time
    [Range(1f, 30f)] [SerializeField] private float slowdownSmooth = 20f;
    [Range(2f, 8f)] [SerializeField] private float slowdownTime = 5; // how long is gona be the slow
    private float startingTimeScale;
    #endregion
    #endregion

    #region Methods
    void Start()
    {
        uIManager = GameManager.GetInstance.GetUIManager;

        originalDeltaTime = Time.fixedDeltaTime;
        originalTime = Time.timeScale;

        timer = slowdownTime;
        startingTimeScale = Time.timeScale;
    }

    public void OnChronoTime()
    {
        if (!isTimeSlow)
        {
            StopAllCoroutines();
            SlowMotion();
        }
        else if (isTimeSlow)
        {
            StopAllCoroutines();
            StartCoroutine("Co_Recharge");
        }
    }

    void SlowMotion()
    {
        //arreglar aca
        isTimeSlow = true;
        StartCoroutine("Co_SlowTime");
        ManageSlowMotion();
    }

    IEnumerator Co_SlowTime()
    {
        //arreglar aca
        // pasar numeros a variables
        while (timer > 0)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, slowdownFactor, slowdownSmooth * Time.unscaledDeltaTime);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            timer -= Time.unscaledDeltaTime;
            uIManager.UpdateBoostValue(slowdownTime, timer);
            yield return null;
        }

        StartCoroutine("Co_Recharge");
    }

    IEnumerator Co_Recharge()
    {
        //arreglar aca
        // pasar numeros a variables
        isTimeSlow = false;
        ManageSlowMotion();
        while (timer <= slowdownTime)
        {
            if (Time.timeScale < startingTimeScale - 0.01f)
            // can change this number but it will work as is
            // it sets timescale to 1 instead of 0.99999.... when close to 1
            {
                Time.timeScale = Mathf.Lerp(Time.timeScale, startingTimeScale, slowdownSmooth * Time.unscaledDeltaTime);
            }
            else
            {
                Time.timeScale = startingTimeScale;
            }
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            timer += Time.unscaledDeltaTime;
            uIManager.UpdateBoostValue(slowdownTime, timer);
            yield return null;
        }
        // para dejar los valores exactos y no luego de un lerp
        timer = slowdownTime;
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    void ManageSlowMotion()
    {
        if (onSlowMotion != null)
        {
            onSlowMotion(isTimeSlow);
        }
    }
    #endregion

    #region Getters/Setters
    public float TimeScale
    {
        get { return Time.timeScale; }
    }
    #endregion
}
