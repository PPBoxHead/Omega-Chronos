using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LaserTurret : MonoBehaviour
{
    #region Variables
    [SerializeField] private Sprite[] indicators;
    [SerializeField] private GameObject endParticles;
    [SerializeField] private float shootDuration = 5;
    [SerializeField] private GameObject laserBeam;
    [SerializeField] private GameObject readySign;
    [SerializeField] private float cooldown = 20;
    private SpriteRenderer spriteRenderer;
    private ScreenShake screenShake;
    private bool isFinished = true;
    private float timer = 0;
    private bool isStarted;
    private bool shooting;
    public UnityEvent onShoot;
    #endregion

    #region Methods
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        screenShake = ScreenShake.Instance;
    }

    private void Update()
    {
        if (isStarted)
        {
            timer += Time.deltaTime;

            float value = timer / cooldown;
            int bars = Mathf.RoundToInt(value * indicators.Length);

            if (bars < indicators.Length)
            {
                spriteRenderer.sprite = indicators[bars];
            }

            if (timer >= cooldown)
            {
                isStarted = false;
                shooting = true;
            }
        }

        if (shooting)
        {
            StartCoroutine("Shooting");
        }
    }

    public void Started()
    {
        if (isFinished)
        {
            isStarted = true;
            readySign.SetActive(false);
            isFinished = false;
        }
    }

    public void LaserLength()
    {
        LineRenderer go;
        Vector3 offset = new Vector3(0, 29, 0);
        go = GetComponentInChildren<LineRenderer>();
        go.SetPosition(1, go.GetPosition(1) + offset);
        endParticles.SetActive(false);
    }

    IEnumerator Shooting()
    {
        shooting = false;
        laserBeam.SetActive(true);
        onShoot?.Invoke();
        screenShake.ShakeCamera(0, shootDuration, 10);
        yield return new WaitForSeconds(shootDuration);
        laserBeam.SetActive(false);
        timer = 0;
        readySign.SetActive(true);
        isFinished = true;
        spriteRenderer.sprite = indicators[0];
    }
    #endregion
}
