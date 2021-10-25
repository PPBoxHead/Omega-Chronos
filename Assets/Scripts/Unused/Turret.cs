using UnityEngine;

public class Turret : MonoBehaviour
{
    public float Agro;
    public Transform Target;
    bool Detected = false;
    Vector2 Direction;
    //public GameObject AlarmLight; Si quieren pueden meterle luces a la torre
    public GameObject GunBarrel;
    public GameObject Bullet;
    public float FireRate;
    float ThisStartsBeingZeroTurret = 0;//voy a ser sincero no se para que sirve esto pero se que es importante
    public Transform ShootPoint;
    public float Force;
    void Update()
    {
        Vector2 targetPos = Target.position;
        Direction = targetPos - (Vector2)transform.position;
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Agro);
        if (rayInfo)
        {
            if (rayInfo.collider.gameObject.CompareTag("Player"))
            {
                if (Detected == false)
                {
                    Detected = true;
                    //aca ponen las luces
                }
            }
            else
            {
                if (Detected == true)
                {
                    Detected = false;
                    //y aca
                }

            }
        }
        if (Detected)
        {
            GunBarrel.transform.right = Direction;
            if (Time.time > ThisStartsBeingZeroTurret)
            {
                ThisStartsBeingZeroTurret = Time.time + 1 / FireRate;
                Shoot();
            }
        }
    }
    public void Shoot()
    {
        GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, Quaternion.identity);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(Direction * Force); // Cuando te acercas dispara mas lento, no quiero que haga eso
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Agro);
    }
}