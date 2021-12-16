using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Boss : Turret
{
    // lo correcto seria tener una clase turret y diferenciarlas de bullet turret
    // con laser turret pero bueno
    #region Variables
    [SerializeField] private SpriteRenderer eyeSprite;
    [SerializeField] private float damageDuration = 2;
    [SerializeField] private float laserCooldown;
    [SerializeField] private float laserSpeed;
    [SerializeField] private Vector3 finalRot;
    [SerializeField] private Transform eyeRotationPivot;
    [SerializeField] private Gun laser;
    [SerializeField] private Animator animator;
    [Header("Arms")]
    [SerializeField] private SpriteRenderer[] armsRenderer;
    [SerializeField] private Sprite[] armSprite;
    [Header("Body")]
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private Sprite[] bodySprites;
    private bool isDamaged, damaging = false;
    private bool laserActivating = true;
    private bool returning = false;
    private bool shooting = false;
    private Vector3 initialRot;
    private int phase = 0;
    public UnityEvent[] onDamage;
    public UnityEvent onDeath;
    #endregion

    #region Methods
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialRot = gunBarrel.transform.rotation.eulerAngles;
    }

    private void Update()
    {
        if (isDamaged)
        {
            if (!damaging) StartCoroutine("OnDamage");
            laser.UpdateLaser();
            DamageAnim();
            return;
        }

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
        eyeRotationPivot.rotation = Quaternion.RotateTowards(eyeRotationPivot.rotation, Quaternion.Euler(new Vector3(0, 0, angle + 90)), laserSpeed * Time.deltaTime);
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
        eyeSprite.color = Color.red;
        animator.Play("Charging");
        audioSource.Play();
        yield return new WaitForSeconds(laserCooldown / 3);
        laser.UpdateLaser();
        laser.EnableLaser();
        shooting = true;
    }

    protected override void Shoot()
    {
        laser.UpdateLaser();
        gunBarrel.transform.rotation = Quaternion.RotateTowards(gunBarrel.transform.rotation, Quaternion.Euler(finalRot), laserSpeed * Time.deltaTime);
        LaserDamage();

        if (gunBarrel.transform.rotation == Quaternion.Euler(finalRot))
        {
            audioSource.Stop();
            shooting = false;
            returning = true;
            laser.DisableLaser();
            animator.Play("Idle");
        }
    }

    public void ShootTest()
    {
        StartCoroutine("LaserCharging");
    }

    void ReturnToInit()
    {
        gunBarrel.transform.rotation = Quaternion.RotateTowards(gunBarrel.transform.rotation, Quaternion.Euler(initialRot), laserSpeed * Time.deltaTime);

        if (gunBarrel.transform.rotation == Quaternion.Euler(initialRot))
        {
            returning = false;
            laserActivating = true;
            eyeSprite.color = Color.white;
        }
    }

    public override void TakeDamage(int value)
    {
        isDamaged = true;
        hitPoints -= value;

        // ESTA HORRIBLE 👍
        if (hitPoints >= 1) bodyRenderer.sprite = bodySprites[hitPoints - 1];
        foreach (SpriteRenderer arm in armsRenderer)
        {
            if (hitPoints <= 1)
            {
                arm.gameObject.SetActive(false);
            }
            else
            {
                arm.sprite = armSprite[hitPoints - 2];
            }
        }

        if (hitPoints <= 0)
        {
            StopAllCoroutines();
            Death();
        }

        if (phase < onDamage.Length)
        {
            onDamage[phase]?.Invoke();
            phase += 1;
        }
    }

    protected override void Death()
    {
        onDeath?.Invoke();
    }

    IEnumerator OnDamage()
    {
        damaging = true;
        yield return new WaitForSeconds(damageDuration);
        isDamaged = false;
        damaging = false;
    }

    void DamageAnim()
    {
        eyeRotationPivot.rotation *= Quaternion.Euler(new Vector3(0, 0, 2f));
    }
    #endregion
}
