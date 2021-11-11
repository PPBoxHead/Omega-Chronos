using UnityEngine;

public class Laser : Turret
{
    // lo correcto seria tener una clase turret y diferenciarlas de bullet turret
    // con laser turret pero bueno

    private void Update()
    {
        PlayerDetection();

        if (target != null)
        {
            Aim();
        }
    }

    override protected void Aim()
    {
        // ajustar un poquito esto
        float angle = Mathf.Atan2(target.position.y + targetOff.y - transform.position.y, target.position.x + targetOff.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        // direction = target.position + targetOff - transform.position;
        // gunBarrel.transform.right = direction;
        // gunBarrel.transform.eulerAngles = new Vector3(0, 0, 0);
        gunBarrel.transform.rotation = Quaternion.RotateTowards(gunBarrel.transform.rotation, targetRotation, 100 * Time.deltaTime);
    }
}
