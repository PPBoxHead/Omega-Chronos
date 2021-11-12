using UnityEngine;
using UnityEngine.Tilemaps;

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
    public Tilemap tilemap;
    Vector2 outDir;
    Vector2 crashPos;
    Vector2 inNormal;
    Vector2 crashVel;
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
        // TakeDamage(1);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            inNormal = other.contacts[0].normal;
            crashPos = transform.position;
            crashVel = (target.position + targetOff - transform.position).normalized;

            outDir = Vector2.Reflect((target.position + targetOff - transform.position).normalized, inNormal) * -1;

            TakeDamage(1);
        }
    }

    private void OnDrawGizmos()
    {
        if (outDir != null)
        {
            Debug.DrawRay(crashPos, outDir, Color.green);
            Debug.DrawRay(crashPos, inNormal, Color.blue);
            Debug.DrawRay(crashPos, crashVel, Color.red);
        }
    }

    public void EnvCrash()
    {
        // reflectedObject.position = Vector3.Reflect(originalObject.position, Vector3.right);
        //vector3.reflex
        Vector2 direction = -rb.velocity.normalized;
        rb.velocity = Vector2.zero;

        rb.velocity = -direction * crashForce;
        // TakeDamage(1);
    }
    #endregion
}
