using UnityEngine;

public class Dron : Enemy
{
    #region Variables
    [Range(10, 100), SerializeField] private int crashForce = 10;
    [Range(1, 10)] [SerializeField] private float precision = 10;
    [Range(1, 20)] [SerializeField] private int visionRange = 3;
    [Range(10, 20)] [SerializeField] private float speed = 10;
    [Range(1, 10)] [SerializeField] private int health = 2;
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

            rb.velocity = Vector2.Lerp(rb.velocity, direction * speed, Time.deltaTime * precision);
        }

        if (target == null)
        {
            Patrol();
            rb.velocity = Vector2.zero;
            chasing = false;
        }
    }

    public void Crash()
    {
        Vector2 direction = (target.position + targetOff - transform.position).normalized;
        rb.velocity = Vector2.zero;

        rb.velocity = -direction * crashForce;
        TakeDamage(1);
    }

    public void EnvCrash()
    {
        Vector2 direction = -rb.velocity.normalized * crashForce;
        rb.velocity = Vector2.zero;

        rb.velocity = -rb.velocity.normalized * crashForce;
        TakeDamage(1);
    }
    #endregion
}
