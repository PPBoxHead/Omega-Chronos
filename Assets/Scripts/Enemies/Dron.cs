using UnityEngine;
using System.Collections;

public class Dron : Enemy
{
    #region Variables
    [Range(1, 10)] [SerializeField] private int health = 3;
    [Range(1, 10)] [SerializeField] private int visionRange = 3;
    [Range(1, 10)] [SerializeField] private float chasingRange = 2;
    private Transform objective;
    private bool chasing;
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

        if (target != null && !chasing)
        {
            Debug.Log("Target adquired");
            Alerted();
            // ver de usar una mezcla  con move towards y luego slerp
            // transform.position += transform.forward * 10 * Time.deltaTime;
            // rb.velocity = Vector3.Slerp(transform.position, target.position, Time.deltaTime);
            // transform.position = Vector3.Slerp(transform.position, target.position, .5f * Time.deltaTime);
            objective = target;
            chasing = true;
            StartCoroutine("ChaseObjective");
        }

        // if (target == null)
        // {
        //     Patrol();
        //     rb.velocity = Vector2.zero;
        //     chasing = false;
        // }

        // if (chasing)
        // {
        //     Debug.Log("Chasing");
        //     // transform.position = Vector3.Slerp(transform.position, objective.position, 1f * Time.deltaTime);
        //     // transform.position = Vector2.MoveTowards(transform.position, objective.position, Time.deltaTime);
        //     rb.velocity = (objective.position - transform.position) * .5f;
        //     chasing = false;

        //     // if (Vector3.Distance(transform.position, objective.position) <= chasingRange)
        //     // {
        //     //     chasing = false;
        //     //     Debug.Log("Llegue");
        //     // }
        // }
    }

    // ojective a lo que tiene que perseguir, target el player
    IEnumerator ChaseObjective()
    {
        rb.velocity = (objective.position - transform.position) * .5f;

        while (Vector3.Distance(transform.position, objective.position) >= chasingRange)
        {
            yield return null;
        }

        Debug.Log("llegue a destino");
        chasing = false;
    }
    #endregion
}
