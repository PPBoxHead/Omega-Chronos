using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Player))]

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("LookAhead Setup")]
    [Range(1, 5)] [SerializeField] private float aheadSpeed = 3;
    [Range(1, 10)] [SerializeField] private float aheadAmount = 5;
    [Header("WallJump Smoothing")]
    [Range(1, 10)] [SerializeField] private float walljumpLerp = 3;
    #region Setup
    private bool stopMovement;
    private Rigidbody2D rb;

    private Player player;

    private List<Player.State> movingState = new List<Player.State>() { Player.State.Walking, Player.State.Jumping, Player.State.Falling, Player.State.WallGrabing };
    #endregion
    #region LookAhead
    Transform lookAhead;
    #endregion
    #endregion

    #region Methods
    void Start()
    {
        lookAhead = GameObject.Find("/Player/LookAheadPoint").transform;

        player = GetComponent<Player>();
        rb = player.GetRb;

        GameManager.GetInstance.onGamePaused += PauseResume;
    }

    void FixedUpdate()
    {
        if (!movingState.Contains(player.CurrentState) || stopMovement) return;

        // moves lookAhead
        lookAhead.localPosition = new Vector2(Mathf.Lerp(lookAhead.localPosition.x, aheadAmount * player.HMovement, aheadSpeed * Time.deltaTime / Time.timeScale), lookAhead.localPosition.y);

        if (!player.WallJumped)
        {
            // transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);
            rb.velocity = new Vector2(player.HMovement * player.CurrentMovementSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(player.HMovement * player.CurrentMovementSpeed, rb.velocity.y)), walljumpLerp * Time.deltaTime / Time.timeScale);
            // used for smooth movement after walljump
        }

    }

    void PauseResume(bool gamePaused)
    {
        stopMovement = gamePaused;
    }

    void OnDestroy()
    {
        GameManager.GetInstance.onGamePaused -= PauseResume;
    }
    #endregion
}
