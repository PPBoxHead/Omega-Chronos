using UnityEngine;

public class SinMovement : MonoBehaviour
{
    private Vector2 initialPos;

    [SerializeField] private float vSpeed = 4f;
    [SerializeField] private float amplitude = 4f;
    [SerializeField] private float hSpeed = 2f;
    [SerializeField] private float limitX; // sets limit on X-axis (ex: if limitX = 4 then this will move between x = 4 and x = -4)

    void Start()
    {
        initialPos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector2(initialPos.x + Mathf.Sin(Time.time * hSpeed) * limitX, initialPos.y + Mathf.Sin(Time.time * vSpeed) * amplitude);
    }
}
