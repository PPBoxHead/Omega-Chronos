using UnityEngine;
using System.Collections;

public class Thwomp : Enemy
{
    #region Variables
    [Range(1, 30)] [SerializeField] private int visionRange = 3;
    #region Eyes
    [Header("Eyes")]
    [Range(1, 90), SerializeField] private float angleRange = 30;
    [Range(50, 150), SerializeField] private int accuracy = 20;
    [SerializeField] private float actionAngle = -90;
    [SerializeField] private Transform eyesPivot;
    private Quaternion targetRotation;
    private float angle;
    private Vector2 direction;
    #endregion
    #region Positions
    [Header("Position")]
    [Range(0.1f, 10), SerializeField] private float actionTime = 0.5f;
    [Range(1, 10), SerializeField] private int speed = 5;
    [SerializeField] private Vector3 finalPos;
    private Vector3 initPos;
    #endregion
    #region State
    private bool isReturning = false;
    private bool isMoving = false;
    private bool onRange = false;
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

        if (isReturning)
        {
            Move(initPos);

            if (transform.position.y >= initPos.y)
            {
                isReturning = false;
            }
            return;
        }

        if (target != null)
        {
            Look();
            if (Mathf.Abs(actionAngle - angle) <= angleRange && !onRange)
            {
                StartCoroutine("StartMoving");
            }
        }

        if (isMoving)
        {
            Move(transform.position + finalPos);

            if (transform.position.y <= initPos.y + finalPos.y)
            {
                StartCoroutine("Return");
            }
        }
    }

    #region Position
    void Move(Vector2 objective)
    {
        transform.position = Vector2.MoveTowards(transform.position, objective, speed * Time.deltaTime);
    }

    IEnumerator StartMoving()
    {
        onRange = true;
        yield return new WaitForSeconds(actionTime);
        isMoving = true;
    }

    IEnumerator Return()
    {
        isMoving = false;
        yield return new WaitForSeconds(actionTime);
        isReturning = true;
        onRange = false;
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
