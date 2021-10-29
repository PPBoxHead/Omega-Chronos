using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float duration = 2;

    void OnCollisionEnter2D(Collision2D collision) //Si colisiona con cualquier box collider va a desaparecer, ojito
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine("Despawn");
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(duration);
        this.gameObject.SetActive(false);
    }
}