using UnityEngine;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    [SerializeField] private string otherTag = "Player";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(otherTag))
        {
            onTriggerEnter?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(otherTag))
        {
            onTriggerExit?.Invoke();
        }
    }
}
