using UnityEngine;
[RequireComponent(typeof(SinMovement))]

public abstract class Enemy : MonoBehaviour
{
    #region Variables
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask ignoreLayer;
    protected int initialHitPoints;
    protected int hitPoints;
    protected Transform target;
    protected float range;
    protected SinMovement patrolCicle;
    protected Vector3 targetOff = new Vector3(0, 1.25f, 0);
    #endregion

    #region Methods
    protected void PlayerDetection()
    {
        Collider2D[] circleHit = Physics2D.OverlapCircleAll(transform.position, range, targetLayer);

        if (circleHit.Length > 0 && circleHit[0].CompareTag("Player"))
        {
            Debug.DrawRay(transform.position, circleHit[0].transform.position + targetOff - transform.position, Color.yellow);

            RaycastHit2D playerOnSight = Physics2D.Raycast(transform.position, circleHit[0].transform.position + targetOff - transform.position, range, ~ignoreLayer);
            bool isPlayerOnSight = playerOnSight.collider && playerOnSight.collider.CompareTag("Player");

            if (isPlayerOnSight)
            {
                target = playerOnSight.collider.transform;
            }
            else
            {
                target = null;
            }
        }
        else
        {
            target = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // abstract si la clase superior no tiene nada
    // virtual es que se puede cambiar
    // ver de pasar a state machine if necsesary
    protected virtual void Chasing()
    {
        patrolCicle.enabled = false;
    }

    protected virtual void ReturnToPos()
    {
        // aca hay que pasar de donde esta hasta su pos inicial
    }

    protected virtual void Patrol()
    {
        patrolCicle.enabled = true;
    }

    public virtual void TakeDamage(int value)
    {
        hitPoints -= value;

        if (hitPoints <= 0)
        {
            Death();
        }
    }
    protected virtual void Death()
    {
        this.gameObject.SetActive(false);
        hitPoints = initialHitPoints;
    }
    #endregion

    #region Getter/Setter
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }
    #endregion
}
