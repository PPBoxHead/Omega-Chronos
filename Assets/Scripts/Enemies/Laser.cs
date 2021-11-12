using UnityEngine;

public class Laser : Turret
{
    // lo correcto seria tener una clase turret y diferenciarlas de bullet turret
    // con laser turret pero bueno
    #region Variables
    [SerializeField] private GameObject laserBeam;
    [SerializeField] private float accuracy = 100;
    #endregion

    #region Methods
    private void Update()
    {
        PlayerDetection();

        if (target != null)
        {
            laserBeam.SetActive(true);
            Aim();
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
    }
    #endregion
}
