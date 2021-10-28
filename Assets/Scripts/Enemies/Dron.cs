using UnityEngine;
using System.Collections;

public class Dron : Enemy
{
    #region Variables
    [Range(1, 10)] [SerializeField] private float presicion = 10;
    [Range(1, 10)] [SerializeField] private int visionRange = 3;
    [Range(10, 20)] [SerializeField] private float speed = 10;
    [Range(1, 10)] [SerializeField] private int health = 3;
    private bool chasing;
    private Rigidbody2D rb;
    #endregion

    #region Methods
    private void Awake()
    {
        range = visionRange;
        initialHitPoints = health;
        hitPoints = initialHitPoints;

        patrolCicle = GetComponent<SinMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerDetection();

        if (target != null && !chasing)
        {
            Chasing();
            chasing = true;
        }

        if (target != null && chasing)
        {
            Vector2 direction = (target.position + targetOff - transform.position).normalized;

            rb.velocity = Vector2.Lerp(rb.velocity, direction * speed, Time.deltaTime);
        }

        if (target == null)
        {
            Patrol();
            rb.velocity = Vector2.zero;
            chasing = false;
        }
    }
    #endregion
}
