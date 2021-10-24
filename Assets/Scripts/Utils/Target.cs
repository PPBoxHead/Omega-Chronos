using UnityEngine;

public class Target : MonoBehaviour
{
    #region Variables
    [SerializeField] private float range = 2f;
    private Enemy enemy;
    #endregion

    #region Methods
    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        PlayerDetection();
    }

    private void PlayerDetection()
    {
        Collider2D circleHit = Physics2D.OverlapCircle(transform.position, range);
        if (circleHit && circleHit.CompareTag("Player"))
        {
            Debug.DrawRay(transform.position, circleHit.transform.position - transform.position, Color.yellow);

            RaycastHit2D playerOnSight = Physics2D.Raycast(transform.position, circleHit.transform.position - transform.position, range);
            bool isPlayerOnSight = playerOnSight.collider && playerOnSight.collider.CompareTag("Player");

            if (isPlayerOnSight)
            {
                enemy.Target = playerOnSight.collider.transform;
            }
            else
            {
                enemy.Target = null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
    #endregion
}
