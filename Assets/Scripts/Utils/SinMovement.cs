using UnityEngine;

public class SinMovement : MonoBehaviour
{
    private Vector2 initialPos;
    [SerializeField] private float vSpeed = 4f;
    [SerializeField] private float amplitude = 4f;
    [SerializeField] private float hSpeed = 2f;
    [SerializeField] private float limitX; // amplitude horizontal
    [SerializeField] private int hDir = 1;
    [SerializeField] private int vDir = 1;
    private bool onPause = false;

    void Start()
    {
        initialPos = transform.position;

        GameManager.GetInstance.onGamePaused += Pause;
    }

    void Update()
    {
        if (onPause) return;

        transform.position = new Vector2(initialPos.x + Mathf.Sin(Time.time * hSpeed * hDir) * limitX, initialPos.y + Mathf.Sin(Time.time * vSpeed * vDir) * amplitude);
    }

    protected void Pause(bool gamePaused)
    {
        onPause = gamePaused;
    }

    private void OnDestroy()
    {
        GameManager.GetInstance.onGamePaused -= Pause;
    }
}
