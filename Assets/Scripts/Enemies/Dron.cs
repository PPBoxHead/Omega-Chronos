using UnityEngine;

public class Dron : Enemy
{
    #region Variables
    [Range(1, 10)] [SerializeField] private int health = 3;
    [Range(1, 10)] [SerializeField] private int visionRange = 3;
    private Rigidbody2D rb;
    #endregion

    #region Methods
    private void Awake()
    {
        range = visionRange;
        initialHitPoints = health;
        hitPoints = initialHitPoints;

        patrolCicle = GetComponent<SinMovement>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        PlayerDetection();
        Debug.Log(target);

        if (target != null)
        {
            Alerted();
            // ver de usar una mezcla  con move towards y luego slerp
            // transform.position += transform.forward * 10 * Time.deltaTime;
            // rb.velocity = Vector3.Slerp(transform.position, target.position, Time.deltaTime);
            // transform.position = Vector3.Slerp(transform.position, target.position, 2 * Time.deltaTime);
        }
        else
        {
            Patrol();
            rb.velocity = Vector2.zero;
        }
    }
    #endregion
}
