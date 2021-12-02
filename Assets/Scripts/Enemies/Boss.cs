using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Boss : Turret
{
    // lo correcto seria tener una clase turret y diferenciarlas de bullet turret
    // con laser turret pero bueno
    #region Variables
    [SerializeField] private GameObject laserBeam;
    [SerializeField] private float laserCooldown;
    [SerializeField] private float laserSpeed;
    [SerializeField] private Vector3 finalRot;
    [SerializeField] private Transform eye;
    private bool laserActivating = true;
    private bool returning = false;
    private bool shooting = false;
    private Vector3 initialRot;
    public UnityEvent onLaserCharge;
    #endregion

    #region Methods
    private void Start()
    {
        initialRot = gunBarrel.transform.rotation.eulerAngles;
    }

    private void Update()
    {
        if (onPause) return;
        PlayerDetection();

        if (target != null)
        {
            Aim();
            if (laserActivating)
            {
                StartCoroutine("LaserCharging");
                return;
            }
        }

        if (shooting)
        {
            Shoot();
            return;
        }

        if (returning)
        {
            ReturnToInit();
            return;
        }
    }

    override protected void Aim()
    {
        direction = target.position + targetOff - transform.position;
        // laser rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // por que es + 90? y el otro -90? no lo se, no recuerdo
        // como se calculaba el angle, lo saque de internet y funciona asi
        // seguro se puede ajustar pero whatever
        eye.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
    }

    void LaserDamage()
    {
        RaycastHit2D laserRay = Physics2D.Raycast(transform.position, shootPoint.transform.up);

        if (laserRay)
        {
            if (laserRay.collider.CompareTag("Player"))
            {
                laserRay.collider.gameObject.GetComponent<Player>().TakeDamage(1);
            }
        }
    }

    IEnumerator LaserCharging()
    {
        laserActivating = false;
        yield return new WaitForSeconds(laserCooldown);
        laserBeam.SetActive(true);
        shooting = true;
    }

    protected override void Shoot()
    {
        gunBarrel.transform.rotation = Quaternion.RotateTowards(gunBarrel.transform.rotation, Quaternion.Euler(finalRot), laserSpeed * Time.deltaTime);
        LaserDamage();

        if (gunBarrel.transform.rotation == Quaternion.Euler(finalRot))
        {
            shooting = false;
            returning = true;
            laserBeam.SetActive(false);
        }
    }

    void ReturnToInit()
    {
        gunBarrel.transform.rotation = Quaternion.RotateTowards(gunBarrel.transform.rotation, Quaternion.Euler(initialRot), laserSpeed * Time.deltaTime);

        if (gunBarrel.transform.rotation == Quaternion.Euler(initialRot))
        {
            returning = false;
            laserActivating = true;
        }
    }

    #endregion
}
