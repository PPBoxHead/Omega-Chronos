using UnityEngine;
using UnityEngine.Events;

public class BossManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject[] energyCells;
    private int energyCellsAct = 0;
    public UnityEvent onPowerUp;
    private int energyCellsReq;
    #endregion

    #region Methods
    private void Start()
    {
        energyCellsReq = energyCells.Length;
    }
    public void PowerEnergyCell()
    {
        energyCellsAct += 1;

        if (energyCellsAct >= energyCellsReq)
        {
            onPowerUp?.Invoke();
        }
    }
    #endregion
}
