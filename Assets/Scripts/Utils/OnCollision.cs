using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour
{
    public UnityEvent onCollisionEnter;
    public UnityEvent onCollisionExit;
    [SerializeField] private string otherTag = "Player";

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(otherTag))
        {
            onCollisionEnter?.Invoke();
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(otherTag))
        {
            onCollisionExit?.Invoke();
        }
    }
}
