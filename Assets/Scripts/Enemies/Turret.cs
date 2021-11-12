using System.Collections;
using UnityEngine;

public class Turret : Enemy
{
    [Range(1, 10)] [SerializeField] private int health = 3;
    [Range(1, 30)] [SerializeField] private int visionRange = 3;
    protected bool aiming;
    protected Vector2 direction;
    [SerializeField] protected GameObject gunBarrel;
    [SerializeField] protected float fireRate = 1;
    protected bool isOnCooldown = false;
    protected BulletPoolManager bulletPoolManager;
    [SerializeField] protected Transform shootPoint;
    [Range(1, 15)] [SerializeField] protected int bulletSpeed = 15;
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
        direction = target.position + targetOff - transform.position;
        gunBarrel.transform.right = direction;
    }
    protected virtual void Shoot()
    {
        GameObject bullet = bulletPoolManager.GetPooledObject();
        bullet.transform.position = shootPoint.position;
        bullet.transform.right = direction;
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
}
