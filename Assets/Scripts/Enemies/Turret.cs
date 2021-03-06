using System.Collections;
using UnityEngine;

public class Turret : Enemy
{
    [SerializeField] public bool turretStoping;
    [Range(1, 10)] [SerializeField] protected int health = 3;
    [Range(1, 30)] [SerializeField] protected int visionRange = 3;
    protected bool aiming;
    protected Vector2 direction;
    [SerializeField] protected GameObject gunBarrel;
    [SerializeField] protected float fireRate = 1;
    protected bool isOnCooldown = false;
    protected BulletPoolManager bulletPoolManager;
    [SerializeField] protected Transform shootPoint;
    [Range(1, 15)] [SerializeField] protected int bulletSpeed = 15;
    protected AudioSource audioSource;
    protected ParticleSystem shootParticles;
    private void Awake()
    {
        range = visionRange;
        initialHitPoints = health;
        hitPoints = initialHitPoints;
        patrolCicle = GetComponent<SinMovement>();
    }
    private void Start()
    {
        bulletPoolManager = BulletPoolManager.GetInstance;
        audioSource = GetComponent<AudioSource>();
        shootParticles = GetComponentInChildren<ParticleSystem>();
        GameManager.GetInstance.onGamePaused += Pause;

        // este valor es para que salga el rayo de vision
        // (y la direccion de apuntado) desde el centro del cañon
        // y no desde las "Ruedas"
        // visionOff = new Vector3(0, 0.8f, 0);
    }
    private void Update()
    {
        if (onPause) return;

        PlayerDetection();

        if (target != null && !aiming)
        {
            if (turretStoping)
            {
                Chasing();
            }
            aiming = true;
        }

        if (aiming && target != null)
        {
            Aim();
            if (!isOnCooldown)
            {
                StartCoroutine("Shooting");
            }
        }

        if (target == null)
        {
            Patrol();
            aiming = false;
        }
    }
    protected virtual void Aim()
    {
        direction = target.position + targetOff - transform.position - visionOff;
        gunBarrel.transform.right = direction;
    }
    protected virtual void Shoot()
    {
        audioSource.Play();
        GameObject bullet = bulletPoolManager.GetPooledObject();
        shootParticles.Play();
        bullet.transform.position = shootPoint.position;
        bullet.transform.right = direction;
        bullet.GetComponent<Bullet>().direction = direction;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed;
    }
    protected IEnumerator Shooting()
    {
        isOnCooldown = true;
        Shoot();
        yield return new WaitForSeconds(fireRate);
        isOnCooldown = false;
    }

    protected void Pause(bool gamePaused)
    {
        onPause = gamePaused;
    }

    private void OnDestroy()
    {
        GameManager.GetInstance.onGamePaused -= Pause;
    }
}
