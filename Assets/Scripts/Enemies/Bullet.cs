using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float duration = 2;

    void OnTriggerEnter2D(Collider2D other) //Si colisiona con cualquier box collider va a desaparecer, ojito
    {
        if (other.CompareTag("Player") || other.CompareTag("Floor") || other.CompareTag("WallGrab") || other.CompareTag("Enemy"))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        StartCoroutine("Despawn");
        gameObject.GetComponent<TrailRenderer>().Clear();
        gameObject.GetComponent<AudioSource>().Play();
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(duration);
        this.gameObject.SetActive(false);
    }
}