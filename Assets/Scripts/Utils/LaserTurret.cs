using UnityEngine;
using System.Collections;

public class LaserTurret : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject[] indicators;
    [SerializeField] private float shootDuration = 5;
    [SerializeField] private GameObject laserBeam;
    [SerializeField] private float cooldown = 20;
    private float timer = 0;
    private bool isStarted;
    private bool shooting;
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
        // TODO: hacer esto que se reinicie con el player y no automatico como esta ahora
        isStarted = true;
    }

    IEnumerator Shooting()
    {
        shooting = false;
        laserBeam.SetActive(true);
        yield return new WaitForSeconds(shootDuration);
        laserBeam.SetActive(false);
        isStarted = true;
        timer = 0;
    }
    #endregion
}
