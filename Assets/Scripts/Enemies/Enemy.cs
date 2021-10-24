using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    protected Transform target;
    #endregion

    #region Methods
    private void Update()
    {
        if (target != null) Debug.Log(target.transform.position);
    }

    // abstract si la clase superior no tiene nada
    // virtual es que se puede cambiar
    // protected abstract void Alerted();
    // protected abstract void TakeDamage(int value);
    // protected abstract void Death();
    #endregion

    #region Getter/Setter
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }
    #endregion
}
