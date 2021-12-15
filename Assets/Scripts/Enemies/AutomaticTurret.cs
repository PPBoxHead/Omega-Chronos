using UnityEngine;

public class AutomaticTurret : Turret
{
    #region Variables
    [SerializeField] private Vector2 bulletDirection;
    #endregion

    #region Methods
    private void Update()
    {
        if (onPause) return;

        if (!isOnCooldown)
        {
            StartCoroutine("Shooting");
        }
    }

    protected override void Shoot()
    {
        audioSource.Play();
        shootParticles.Play();

        GameObject bullet = bulletPoolManager.GetPooledObject();
        bullet.transform.position = shootPoint.position;
        bullet.transform.right = bulletDirection;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
    }
    #endregion
}
