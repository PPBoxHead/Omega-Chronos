using UnityEngine;

public class SavesManager : MonoBehaviour
{
    public void DeleteSaves()
    {
        PlayerPrefs.DeleteKey("OCscene");
        PlayerPrefs.DeleteKey("OCcheckpointX");
        PlayerPrefs.DeleteKey("OCcheckpointY");
        Debug.Log("PlayerPrefs deleted");
    }

    public void SaveScene(string value)
    {
        PlayerPrefs.SetString("OCscene", value);
    }
}
