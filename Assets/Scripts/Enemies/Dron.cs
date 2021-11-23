using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Dron : Enemy
{
    #region Variables
    [Range(1, 10), SerializeField] private int crashForce = 5;
    [Range(1, 10)] [SerializeField] private float precision = 10;
    [Range(1, 20)] [SerializeField] private int visionRange = 3;
    [Range(10, 20)] [SerializeField] private float speed = 10;
    [Range(1, 10)] [SerializeField] private int health = 2;
    private bool chasing;
    private Rigidbody2D rb;
    #region Crashing
    private float crashDuration = 0.5f;
    private bool onDamage = false;
    private Vector2 crashVel;
    private Vector2 inNormal;
    private Vector2 crashPos;
    private Tilemap tilemap;
    private Vector2 outDir;
    #endregion
    #endregion

    #region Methods
    private void Start()
    {
        range = visionRange;
        initialHitPoints = health;
        hitPoints = initialHitPoints;

        patrolCicle = GetComponent<SinMovement>();
        rb = GetComponent<Rigidbody2D>();
        tilemap = GameManager.GetInstance.GetTilemap;
    }

    private void Update()
    {
        if (onDamage) return;

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
        StartCoroutine("Co_OnDamage");

        if (target != null)
        {
            Vector2 direction = (target.position + targetOff - transform.position).normalized;
            rb.velocity = Vector2.zero;

            rb.velocity = -direction * crashForce;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            inNormal = other.contacts[0].normal;
            crashPos = transform.position;
            crashVel = (target.position + targetOff - transform.position).normalized;

            outDir = Vector2.Reflect((target.position + targetOff - transform.position).normalized, inNormal) * -1;
            rb.velocity = outDir.normalized * crashForce;

            StartCoroutine("Co_OnDamage");
        }
    }

    IEnumerator Co_OnDamage()
    {
        TakeDamage(1);
        onDamage = true;
        yield return new WaitForSeconds(crashDuration);
        onDamage = false;
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
    #endregion
}
