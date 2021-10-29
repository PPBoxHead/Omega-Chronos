using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    [Range(1, 10)] [SerializeField] private int health = 3;
    [Range(1, 10)] [SerializeField] private int visionRange = 3;
    private bool aiming;
    Vector2 direction;
    [SerializeField] private GameObject gunBarrel;
    [SerializeField] private float fireRate = 1;
    private bool isOnCooldown = false;
    private BulletPoolManager bulletPoolManager;
    [SerializeField] private Transform shootPoint;
    [Range(1, 15)] [SerializeField] private int bulletSpeed = 15;
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
    void Aim()
    {
        direction = target.position + targetOff - transform.position;
        gunBarrel.transform.right = direction;
    }
    void Shoot()
    {
        GameObject bullet = bulletPoolManager.GetPooledObject();
        bullet.transform.position = shootPoint.position;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed;
    }
    IEnumerator Shooting()
    {
        isOnCooldown = true;
        Shoot();
        yield return new WaitForSeconds(fireRate);
        isOnCooldown = false;
    }
}
