using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour
{
    public UnityEvent onCollisionEnter;
    public UnityEvent onCollisionExit;
    public bool isEnabled = true;
    [SerializeField] private string otherTag = "Player";

    void OnCollisionEnter2D(Collision2D other)
    {
        if (isEnabled)
        {
            if (other.gameObject.CompareTag(otherTag))
            {
                onCollisionEnter?.Invoke();
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (isEnabled)
        {
            if (other.gameObject.CompareTag(otherTag))
            {
                onCollisionExit?.Invoke();
            }
        }
    }
}
