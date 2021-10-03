using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float slowdownFactor = 0.05f; // controls how low it's gonna be the time
    [Range(2f, 8f)]
    [SerializeField] private float slowdownTime = 5; // how long is gona be the slow
    public float originalTime;
    public float originalDeltaTime;
    public bool isTimeSlow = false;
    [SerializeField] private Rigidbody2D player;
    private float gravityScale;

    private KeyCode slowmoButton;

    // Delegate to notice things about this change
    public delegate void OnSlowMotion(bool paused);
    public event OnSlowMotion onSlowMotion;

    // esto pasar a un script aparte
    // porque esta super desprolijo
    [SerializeField] GameObject slowmoBar;
    [SerializeField] RectTransform valueBar;
    private float maxBarValue;
    private float timer;

    void Awake()
    {
        maxBarValue = valueBar.rect.width;
    }

    void Start()
    {
        originalDeltaTime = Time.fixedDeltaTime;
        originalTime = Time.timeScale;

        slowmoButton = KeybindingsManager.GetInstance.GetSlowmoButton;
        gravityScale = player.gravityScale;
    }

    void Update()
    {
        /*Time.timeScale += (1f / slowdownTime) * Time.unscaledDeltaTime; back to the normal time
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);*/
        if (Input.GetKeyDown(slowmoButton) && !isTimeSlow)
        {
            SlowMotion();
        }

        if (isTimeSlow)
        {
            UpdateBoostValue(timer);
        }
    }

    void UpdateBoostValue(float value)
    {
        timer -= Time.unscaledDeltaTime;
        valueBar.sizeDelta = new Vector2(value / slowdownTime * maxBarValue, valueBar.sizeDelta.y);
    }

    public void SlowMotion()
    {
        isTimeSlow = true;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        timer = slowdownTime;
        player.gravityScale = gravityScale / Mathf.Pow(Time.timeScale, 2);
        StartCoroutine("NormalMotion");

        ManageSlowMotion();
    }

    // esta solo esta para cuando quiero hacer pruebas sin CD
    /*public void NormalMotionTest()
    {
        Time.timeScale = originalTime;
        Time.fixedDeltaTime = originalDeltaTime;
        isTimeSlow = false;

        ManageSlowMotion();
    }*/

    IEnumerator NormalMotion()
    {
        yield return new WaitForSeconds(slowdownTime * slowdownFactor);

        ReturnMotion();
    }

    public void ReturnMotion()
    {
        StopAllCoroutines(); // esto esta para que cuando lo llames del dash no se ejecute 2 veces
        player.velocity = new Vector2(player.velocity.x, player.velocity.y * Time.timeScale);
        Time.timeScale = originalTime;
        Time.fixedDeltaTime = originalDeltaTime;
        isTimeSlow = false;
        valueBar.sizeDelta = new Vector2(maxBarValue, valueBar.sizeDelta.y);
        player.gravityScale = gravityScale;

        ManageSlowMotion();

    }

    void ManageSlowMotion()
    {
        if (onSlowMotion != null)
        {
            onSlowMotion(isTimeSlow);
        }
    }

    public float GetSlowdownFactor
    {
        get { return slowdownFactor; }
    }
}
