using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private Transform checkpoint;
    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.HasKey("OCcheckpointX") && PlayerPrefs.HasKey("OCcheckpointY"))
        {
            Vector2 initialPosition;
            initialPosition = new Vector2(PlayerPrefs.GetFloat("OCcheckpointX"), PlayerPrefs.GetFloat("OCcheckpointY"));
            checkpoint.position = initialPosition;
        }
    }

    public void SetCheckpoint(Transform destination)
    {
        if (PlayerPrefs.HasKey("OCcheckpointX") && PlayerPrefs.HasKey("OCcheckpointY"))
        {
            Vector3 currentCheckpoint = new Vector3(PlayerPrefs.GetFloat("OCcheckpointX"), PlayerPrefs.GetFloat("OCcheckpointY"), 0);

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
        PlayerPrefs.SetFloat("OCcheckpointX", destination.position.x);
        PlayerPrefs.SetFloat("OCcheckpointY", destination.position.y);
        Debug.Log("Checkpoint position: (" + destination.position.x + "), (" + checkpoint.position.y + ")");
        checkpoint.position = destination.position;
    }
}
