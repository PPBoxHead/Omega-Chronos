using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private bool repeat;
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private Transform camTransform;

    private Vector3 lastCameraPosition;
    private float bgLength;

    private void Start()
    {
        lastCameraPosition = camTransform.position;
        bgLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        // Esto hace que se mueva
        Vector3 deltaMovement = camTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y, transform.position.z);
        lastCameraPosition = camTransform.position;

        // esto hace que lo repita
        // es un fix para los cables (el bool repeat)
        if (repeat)
        {
            if (Mathf.Abs(camTransform.position.x - transform.position.x) >= bgLength)
            {
                float offsetPositionX = (camTransform.position.x - transform.position.x) % bgLength;
                transform.position = new Vector3(camTransform.position.x + offsetPositionX, transform.position.y);
            }
        }
    }
}
