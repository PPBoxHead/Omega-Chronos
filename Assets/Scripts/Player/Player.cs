using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform checkpoint;
    // de aca pasarias los componentes del player al resto de los scripts

    void Start()
    {
        GameManager.GetInstance.onDeath += onDeath;
        this.transform.position = checkpoint.position;
    }

    // instantly moves player
    public void MovePlayer()
    {
        this.transform.position = checkpoint.position;
    }

    void onDeath(float duration)
    {
        MovePlayer();
        // tambien frenar la velocidad que tenia anteriormente
    }

    void OnDestroy()
    {
        GameManager.GetInstance.onDeath -= onDeath;
    }
}
