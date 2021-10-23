using UnityEngine;

public class MoveObject : MonoBehaviour
{
    #region Variables
    private bool isMoving = false;
    [SerializeField] private Transform objective;
    [SerializeField] private Transform finalPos;
    [Range(1, 20)] [SerializeField] private int speed = 5;
    #endregion

    #region Methods
    private void Update()
    {
        if (isMoving)
        {
            MoveObjective();
        }
    }

    /// <summary>
    /// changes isMoving state
    /// </summary>
    public void IsMoving()
    {
        isMoving = !isMoving;
    }

    public void StopMoving()
    {
        if (isMoving) isMoving = false;
    }

    /// <summary>
    /// moves objective to final pos
    /// </summary>
    public void MoveObjective()
    {
        Vector3 vector3 = Vector3.MoveTowards(objective.position, finalPos.position, speed * Time.deltaTime);
        objective.position = vector3;

        // stops excecuting if objective is on place
        if (objective.position == finalPos.position)
        {
            isMoving = false;
        }
    }

    /// <summary>
    /// instantly moves objective to final position
    /// </summary>
    public void InstantMove()
    {
        objective.position = finalPos.position;
    }
    #endregion
}