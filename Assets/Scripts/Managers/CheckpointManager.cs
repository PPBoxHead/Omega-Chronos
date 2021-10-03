using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private Transform checkpoint;
    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.HasKey("checkpointX") && PlayerPrefs.HasKey("checkpointY"))
        {
            Vector2 initialPosition;
            initialPosition = new Vector2(PlayerPrefs.GetFloat("checkpointX"), PlayerPrefs.GetFloat("checkpointY"));
            checkpoint.position = initialPosition;
        }
    }

    public void SetCheckpoint(Transform destination)
    {
        if (PlayerPrefs.HasKey("checkpointX") && PlayerPrefs.HasKey("checkpointY"))
        {
            Vector3 currentCheckpoint = new Vector3(PlayerPrefs.GetFloat("checkpointX"), PlayerPrefs.GetFloat("checkpointY"), 0);

            if (currentCheckpoint != destination.position)
            {
                MoveCheckpoint(destination);
            }
        }
        else
        {
            MoveCheckpoint(destination);
        }
    }

    public void MoveCheckpoint(Transform destination)
    {
        PlayerPrefs.SetFloat("checkpointX", destination.position.x);
        PlayerPrefs.SetFloat("checkpointY", destination.position.y);
        Debug.Log("Checkpoint position: (" + destination.position.x + "), (" + checkpoint.position.y + ")");
        checkpoint.position = destination.position;
    }
}
