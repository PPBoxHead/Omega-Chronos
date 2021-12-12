using UnityEngine;
using UnityEngine.Events;

public class FlyingTurret : Turret
{
    // realmente turret no deberia tener nada en el update
    // pero que le vamos a hacer 🤷‍♂️
    // iba a ser una torreta voladora, pero quedaba muy dificil
    // asi que le saque la capacidad de disparar
    // (y ya que funciona no voy a cambiar que herede de torreta)
    #region Variables
    [SerializeField, Range(1, 15)] private int playerDistance = 10;
    [SerializeField, Range(20, 60)] private int speed = 60;
    private ParticlePoolManager particlePoolManager;
    public UnityEvent onDeath;
    private Rigidbody2D rb;
    #endregion

    #region Methods
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        particlePoolManager = ParticlePoolManager.GetInstance;
    }
    private void Update()
    {
        PlayerDetection();

        if (target != null && !aiming)
        {
            Chasing();
            aiming = true;
        }

        if (aiming && target != null)
        {
            Aim();
        }

        if (target == null)
        {
            Patrol();
            aiming = false;
        }
        else
        {
            // esto seguro que se hace mejor pero bueh es lo que hay a estas alturas
            if (Vector2.Distance(transform.position, target.position) <= playerDistance)
            {
                rb.velocity = Vector2.Lerp(rb.velocity, -direction.normalized, Time.deltaTime * speed);
            }

            if (Vector2.Distance(transform.position, target.position) >= playerDistance + 2)
            {
                rb.velocity = Vector2.Lerp(rb.velocity, direction.normalized, Time.deltaTime * speed);
            }
        }
    }

    protected override void Death()
    {
        rb.gravityScale = 4;
        rb.freezeRotation = false;
        hitPoints = initialHitPoints;

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

        onDeath?.Invoke();
        GetComponent<Animator>().Play("deathEvilPedro");
        GetComponent<FlyingTurret>().enabled = false;
    }
    #endregion
}
