using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    #region Variables
    #region Setup
    [SerializeField] private Button playBtn;
    #endregion
    #endregion

    #region Methods
    void Start()
    {
        playBtn.Select();
    }

    public void PauseMenu()
    {
        GameManager.GetInstance.PauseGame();
    }
    #endregion
}
