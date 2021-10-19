using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Player))]

public class PlayerJump : MonoBehaviour
{
    #region Variables
    #region Setup
    // pasar a Player.cs
    [Header("Jump force")]
    [Range(5, 20)] [SerializeField] private float vJumpForce = 11f;
    [Range(5, 20)] [SerializeField] private float hJumpForce = 11f;

    [Header("Jump Config")]
    [Range(0, 0.5f)] [SerializeField] private float bufferTime = 0.1f;

    [Range(0, 1)] [SerializeField] private float jumpTimer = 0.2f;

    private bool stopMovement;
    private KeyCode jumpBtn;

    private Rigidbody2D rb;

    private Player player;

    private List<Player.State> jumpingState = new List<Player.State>() { Player.State.Walking, Player.State.Idle, Player.State.Jumping, Player.State.Falling, Player.State.WallGrabing, Player.State.WallJumping };
    #endregion
    #region Buffer
    Queue<KeyCode> inputBuffer;
    #endregion
    #region VariableJump

    private bool checkingForWall = true;
    private bool releaseJump = false;
    private bool startTimer = false;
    private float gravityScale;
    private float timer;
    #endregion
    #region CoyoteTime
    [Header("Coyote Time")]
    [Range(0, 0.5f)] [SerializeField] private float coyoteFrames = 0.1f;
    [SerializeField] private float coyoteTimer;
    #endregion
    #endregion

    #region Methods
    void Start()
    {
        player = GetComponent<Player>();

        rb = player.GetRb;
        gravityScale = rb.gravityScale;

        timer = jumpTimer;

        jumpBtn = KeybindingsManager.GetInstance.GetJumpButton;

        inputBuffer = new Queue<KeyCode>();

        GameManager.GetInstance.onGamePaused += PauseResume;
    }

    void Update()
    {
        if (stopMovement || !jumpingState.Contains(player.CurrentState)) return;

        if ((player.IsOnGround || player.IsOnWallL || player.IsOnWallR) && checkingForWall)
        {
            coyoteTimer = 0;
        }
        else
        {
            coyoteTimer += Time.deltaTime;
        }

        if (Input.GetKeyDown(jumpBtn))
        {
            inputBuffer.Enqueue(jumpBtn);
            Invoke("RemoveAction", bufferTime);
        }

        if ((player.IsOnGround || player.IsOnWallL || coyoteTimer < coyoteFrames) && inputBuffer.Count > 0)
        {
            if (inputBuffer.Peek() == jumpBtn && player.CurrentState != Player.State.Jumping)
            {
                inputBuffer.Clear();
                if (player.IsOnWallL || player.IsOnWallR) WallJump();
                else VerticalJump();
            }
        }

        if (Input.GetKeyUp(jumpBtn))
        {
            releaseJump = true;
        }

        if (startTimer)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                releaseJump = true;
            }
        }

        if (releaseJump)
        {
            StopJump();
        }
    }

    void VerticalJump()
    {
        // algunas lineas estan repetidas con WallJump()
        // se podria mejorar un poco mas
        // pero creo que esta bien
        player.CurrentState = Player.State.Jumping;

        rb.gravityScale = 0;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity = Vector2.up * vJumpForce;

        startTimer = true;
    }

    void WallJump()
    {
        int jumpDir = player.IsOnWallR ? -1 : 1;

        player.CurrentState = Player.State.WallJumping;
        player.WallJumped = true;

        rb.gravityScale = 0;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity = new Vector2(jumpDir * hJumpForce, vJumpForce);

        startTimer = true;
        checkingForWall = false;
    }

    void StopJump()
    {
        rb.gravityScale = gravityScale;

        releaseJump = false;

        timer = jumpTimer;
        startTimer = false;

        if (!checkingForWall) checkingForWall = true;
    }

    void PauseResume(bool gamePaused)
    {
        stopMovement = gamePaused;
    }

    void RemoveAction()
    {
        if (inputBuffer.Count > 0) inputBuffer.Dequeue();
    }

    void OnDestroy()
    {
        GameManager.GetInstance.onGamePaused -= PauseResume;
    }
    #endregion
}
