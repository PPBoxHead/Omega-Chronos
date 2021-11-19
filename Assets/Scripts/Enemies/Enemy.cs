using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    #region Variables
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] protected Vector3 visionOff;
    protected int initialHitPoints;
    protected int hitPoints;
    protected Transform target;
    protected float range;
    protected SinMovement patrolCicle;
    protected Vector3 targetOff = new Vector3(0, 1.25f, 0);
    Coroutine co;
    #endregion

    #region Methods
    protected void PlayerDetection()
    {
        Collider2D[] circleHit = Physics2D.OverlapCircleAll(transform.position, range, targetLayer);

        if (circleHit.Length > 0 && circleHit[0].CompareTag("Player"))
        {
            Debug.DrawRay(transform.position + visionOff, circleHit[0].transform.position + targetOff - transform.position - visionOff, Color.yellow);

            RaycastHit2D playerOnSight = Physics2D.Raycast(transform.position + visionOff, circleHit[0].transform.position + targetOff - transform.position - visionOff, range, ~ignoreLayer);
            bool isPlayerOnSight = playerOnSight.collider && playerOnSight.collider.CompareTag("Player");

            if (isPlayerOnSight)
            {
                // aca tengo que hacer lo de frenar solo 1 corrutina
                // en vez de todas
                // StopCoroutine(co);
                target = playerOnSight.collider.transform;
            }
        }
        else
        {
            // co = StartCoroutine(TargetOOS());
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

    IEnumerator TargetOOS()
    {
        yield return new WaitForSeconds(2);
        target = null;
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
