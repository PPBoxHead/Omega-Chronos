using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LightsController : MonoBehaviour
{
    [SerializeField] private bool activating = false;
    [SerializeField] private float speed = 3;
    public UnityEvent[] turnOnLights;

    public void Activate()
    {
        if (!activating)
        {
            activating = true;
            StartCoroutine("Activating");
        }
    }

    IEnumerator Activating()
    {
        foreach (UnityEvent item in turnOnLights)
        {
            item?.Invoke();
            yield return new WaitForSeconds(speed);
        }
    }
}
