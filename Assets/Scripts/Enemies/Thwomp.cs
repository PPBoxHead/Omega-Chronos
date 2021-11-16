using UnityEngine;
using System.Collections;

public class Thwomp : Enemy
{
    #region Variables
    [Range(1, 30)] [SerializeField] private int visionRange = 3;
    #region Eyes
    [Header("Eyes")]
    [Range(50, 150), SerializeField] private int accuracy = 20;
    [SerializeField] private Transform eyesPivot;
    private Quaternion targetRotation;
    private float angle;
    private Vector2 direction;
    #endregion
    #region Positions
    [Header("Position")]
    [Range(0.1f, 10), SerializeField] private float actionTime = 0.5f;
    [Range(1, 20), SerializeField] private int speed = 5;
    [SerializeField] private Vector3 finalPos;
    private Vector3 initPos;
    #endregion
    #region State
    private bool isReturning = false;
    #endregion
    #endregion

    #region Methods
    private void Awake()
    {
        range = visionRange;
        initPos = transform.position;
    }

    private void Update()
    {
        PlayerDetection();
        if (target != null) Look();


        if (isReturning)
        {
            Move(initPos);

            if (transform.position.y >= initPos.y)
            {
                isReturning = false;
            }
            return;
        }
        else
        {

            if (transform.position.y <= initPos.y + finalPos.y)
            {
                StartCoroutine("Return");
            }
            else
            {
                Move(transform.position + finalPos);
            }

            return;
        }
    }

    #region Position
    void Move(Vector2 objective)
    {
        transform.position = Vector2.MoveTowards(transform.position, objective, speed * Time.deltaTime);
    }

    IEnumerator Return()
    {
        yield return new WaitForSeconds(actionTime);
        isReturning = true;
    }
    #endregion

    #region Eyes
    void Look()
    {
        direction = target.position + targetOff - transform.position;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        eyesPivot.transform.rotation = Quaternion.RotateTowards(eyesPivot.transform.rotation, targetRotation, accuracy * Time.deltaTime);
    }
    #endregion
    #endregion
}
