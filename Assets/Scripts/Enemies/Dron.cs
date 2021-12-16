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
    private ParticlePoolManager particlePoolManager;
    #region Crashing
    private float crashDuration = 1f;
    private AudioSource audioSource;
    private bool onDamage = false;
    private Vector2 crashVel;
    private Vector2 inNormal;
    private Vector2 crashPos;
    private Tilemap tilemap;
    private Vector2 outDir;
    #endregion
    private bool isDead = false;
    private Animator animator;
    #endregion

    #region Methods
    private void Start()
    {
        range = visionRange;
        initialHitPoints = health;
        hitPoints = initialHitPoints;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        patrolCicle = GetComponent<SinMovement>();
        rb = GetComponent<Rigidbody2D>();
        tilemap = GameManager.GetInstance.GetTilemap;
        particlePoolManager = ParticlePoolManager.GetInstance;

        GameManager.GetInstance.onGamePaused += Pause;
    }

    private void Update()
    {
        if (onDamage || onPause) return;

        PlayerDetection();

        if (target != null && !chasing)
        {
            animator.Play("droneAlert");
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
            animator.Play("droneIdle");
            rb.velocity = Vector2.zero;
            chasing = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isDead) return;
        if (other.gameObject.CompareTag("Floor"))
        {
            inNormal = other.contacts[0].normal;
            crashPos = transform.position;
            crashVel = (target.position + targetOff - transform.position).normalized;

            outDir = Vector2.Reflect((target.position + targetOff - transform.position).normalized, inNormal) * -1;
            rb.velocity = Vector2.zero;
            rb.velocity = outDir.normalized * crashForce;

            GameObject particles = particlePoolManager.GetPooledObject();
            particles.transform.position = transform.position;
            particles.GetComponent<ExplosionParticles>().direction = -outDir.normalized;
            particles.SetActive(true);

            StartCoroutine("Co_OnDamage");
            return;
        }

        // no era la idea de que esto termine asi
        if (other.gameObject.CompareTag("Player"))
        {
            Vector2 direction = (target.position + targetOff - transform.position).normalized;
            rb.velocity = Vector2.zero;

            GameObject particles = particlePoolManager.GetPooledObject();
            particles.transform.position = transform.position;
            particles.GetComponent<ExplosionParticles>().direction = direction.normalized;
            particles.SetActive(true);

            rb.velocity = Vector2.zero;
            rb.velocity = -direction * crashForce;
            if (hitPoints == 1) GetComponent<OnCollision>().onCollisionEnter?.Invoke(); // fue una solucion media rancia pero no se ejecutaba la ultima vez, asi lo forzamos a que lo haga
            StartCoroutine("Co_OnDamage");
        }
    }

    IEnumerator Co_OnDamage()
    {
        audioSource.Play();
        TakeDamage(1);
        onDamage = true;
        yield return new WaitForSeconds(crashDuration);
        onDamage = false;
    }

    protected override void Death()
    {
        if (isDead) return;
        isDead = true;
        rb.gravityScale = 4;
        rb.freezeRotation = false;

        GameObject explosion = particlePoolManager.GetExplosion();
        if (explosion != null)
        {
            explosion.transform.parent = transform;
            explosion.transform.position = transform.position;
            explosion.SetActive(true);
        }

        GameObject smoke = particlePoolManager.GetSmoke();

        if (smoke != null)
        {
            smoke.transform.position = transform.position;
            smoke.transform.parent = transform;
            smoke.SetActive(true);
        }

        animator.Play("droneDeath");
        GetComponent<OnCollision>().isEnabled = false;
        GetComponent<Dron>().enabled = false;
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

    protected void Pause(bool gamePaused)
    {
        if (gamePaused)
        {
            rb.velocity = Vector2.zero;
        }

        onPause = gamePaused;
    }

    private void OnDestroy()
    {
        GameManager.GetInstance.onGamePaused -= Pause;
    }
    #endregion
}
