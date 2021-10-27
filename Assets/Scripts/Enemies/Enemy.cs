using UnityEngine;
[RequireComponent(typeof(SinMovement))]

public abstract class Enemy : MonoBehaviour
{
    #region Variables
    protected int initialHitPoints;
    protected int hitPoints;
    protected Transform target;
    protected float range;
    protected SinMovement patrolCicle;
    #endregion

    #region Methods
    protected void PlayerDetection()
    {
        Collider2D circleHit = Physics2D.OverlapCircle(transform.position, range);
        if (circleHit && circleHit.CompareTag("Player"))
        {
            Debug.DrawRay(transform.position, circleHit.transform.position - transform.position, Color.yellow);

            RaycastHit2D playerOnSight = Physics2D.Raycast(transform.position, circleHit.transform.position - transform.position, range);
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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // abstract si la clase superior no tiene nada
    // virtual es que se puede cambiar
    // ver de pasar a state machine if necsesary
    protected virtual void Alerted()
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
