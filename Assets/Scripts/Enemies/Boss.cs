using UnityEngine;
using UnityEngine.Events;

public class Boss : Turret
{
    // lo correcto seria tener una clase turret y diferenciarlas de bullet turret
    // con laser turret pero bueno
    #region Variables
    [SerializeField] private GameObject laserBeam;
    [SerializeField] private float accuracy = 100;
    [SerializeField] private Transform eye;
    public UnityEvent onLaserCharge;
    #endregion

    #region Methods
    private void Update()
    {
        if (onPause) return;

        PlayerDetection();

        if (target != null)
        {
            laserBeam.SetActive(true);
            Aim();
            LaserCharge();
        }
        else
        {
            laserBeam.SetActive(false);
        }
    }

    override protected void Aim()
    {
        direction = target.position + targetOff - transform.position;

        // laser rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        gunBarrel.transform.rotation = Quaternion.RotateTowards(gunBarrel.transform.rotation, targetRotation, accuracy * Time.deltaTime);

        // por que es + 90? y el otro -90? no lo se, no recuerdo
        // como se calculaba el angle, lo saque de internet y funciona asi
        // seguro se puede ajustar pero whatever
        eye.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
    }

    void LaserCharge()
    {
        RaycastHit2D laserRay = Physics2D.Raycast(transform.position, shootPoint.transform.up);

        if (laserRay)
        {
            if (laserRay.collider.CompareTag("EnergyCell"))
            {
                laserRay.collider.gameObject.SetActive(false);
                onLaserCharge?.Invoke();
            }
            else if (laserRay.collider.CompareTag("Player"))
            {
                laserRay.collider.gameObject.GetComponent<Player>().TakeDamage(1);
            }
        }
    }
    #endregion
}
