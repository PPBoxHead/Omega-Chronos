using UnityEngine;

public class SavesManager : MonoBehaviour
{
    public void DeleteSaves()
    {
        PlayerPrefs.DeleteKey("scene");
        PlayerPrefs.DeleteKey("checkpointX");
        PlayerPrefs.DeleteKey("checkpointY");
        Debug.Log("PlayerPrefs deleted");
    }
}
