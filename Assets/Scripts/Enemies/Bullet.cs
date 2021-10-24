using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform target;
    [Range(1, 100)] [SerializeField] private float speed = 10;
    float duration = 2;

    void OnCollisionEnter2D(Collision2D collision) //Si colisiona con cualquier box collider va a desaparecer, ojito
    {
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        // esto lo correcto seria hacerlo en el start con un velocity o algo de eso, pero para probar rinde
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed);
    }
}