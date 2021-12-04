using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Variables
    #region Slowmo UI
    [Header("Slowmo UI")]
    [SerializeField] private Sprite[] slowmoSprites;
    [SerializeField] private Image slowmoUI;
    #endregion
    #region Hitpoints UI
    [Header("Hitpoins UI")]
    [SerializeField] private Sprite[] lifeSprites;
    [SerializeField] private Image lifeUI;
    private static UIManager instance;
    #endregion
    #endregion

    #region Methods

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void UpdateBoostValue(float slowdownTime, float value)
    {
        float slowmoValue = value / slowdownTime;
        int bars = Mathf.RoundToInt(slowmoValue * slowmoSprites.Length);
        if (bars == 0) return;

        slowmoUI.sprite = slowmoSprites[bars - 1];
    }

    public void TurnOffUI()
    {
        slowmoUI.gameObject.SetActive(false);
    }

    public void TurnOnnUI()
    {
        slowmoUI.gameObject.SetActive(true);
    }
    public void UpdateHitPoints(int currentHitPoints)
    {
        if (currentHitPoints == 0) return; //no es lo mas lindo pero funciona porque cuando los hitpoints son 0 el juego recarga
        lifeUI.GetComponent<Image>().sprite = lifeSprites[currentHitPoints - 1];
    }
    #endregion

    #region Setters/Getters
    public static UIManager GetInstance
    {
        get { return instance; }
    }
    #endregion
}
