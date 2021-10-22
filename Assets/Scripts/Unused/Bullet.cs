using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float delay = 2;

    void OnCollisionEnter2D(Collision2D collision) //Si colisiona con cualquier box collider va a desaparecer, ojito
    {
        Destroy(this.gameObject);
    }
    void Update()
    {
        Destroy(this.gameObject, delay);
    }
}