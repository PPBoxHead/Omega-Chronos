using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LaserTurret : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject[] indicators;
    [SerializeField] private float shootDuration = 5;
    [SerializeField] private GameObject laserBeam;
    [SerializeField] private GameObject readySign;
    [SerializeField] private float cooldown = 20;
    private float timer = 0;
    private bool isFinished = true;
    private bool isStarted;
    private bool shooting;

    public UnityEvent onShoot;
    #endregion

    #region Methods
    private void Update()
    {
        if (isStarted)
        {
            timer += Time.deltaTime;

            float value = timer / cooldown;
            int bars = Mathf.RoundToInt(value * indicators.Length);

            for (int i = 0; i < bars; i++)
            {
                indicators[i].SetActive(true);
            }

            for (int i = bars; i < indicators.Length; i++)
            {
                indicators[i].SetActive(false);
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

    IEnumerator Shooting()
    {
        shooting = false;
        laserBeam.SetActive(true);
        onShoot?.Invoke();
        yield return new WaitForSeconds(shootDuration);
        laserBeam.SetActive(false);
        timer = 0;
        readySign.SetActive(true);
        isFinished = true;

        foreach (GameObject item in indicators)
        {
            item.SetActive(false);
        }
    }
    #endregion
}
