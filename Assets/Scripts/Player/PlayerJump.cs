using UnityEngine;
using UnityEngine.InputSystem;
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
    private float currentVJumpForce;
    private float currentHJumpForce;

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
    private int inputTest = 0;
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

        inputBuffer = new Queue<KeyCode>();

        GameManager.GetInstance.onGamePaused += PauseResume;
    }

    public void OnJump()
    {
        inputTest += 1;
        Invoke("RemoveAction", bufferTime);
    }

    public void OnReleaseJump()
    {
        releaseJump = true;
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
            coyoteTimer += Time.deltaTime / Time.timeScale;
        }

        if ((player.IsOnGround || player.IsOnWallL || coyoteTimer < coyoteFrames) && inputTest > 0)
        {
            if (player.CurrentState != Player.State.Jumping)
            {
                inputTest = 0;
                player.JumpParticles.Play();
                if (player.IsOnWallL || player.IsOnWallR) WallJump();
                else VerticalJump();
            }
        }

        if (startTimer)
        {
            timer -= Time.deltaTime / Time.timeScale;
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
        rb.velocity = Vector2.up * (vJumpForce / Time.timeScale);

        startTimer = true;
    }

    void WallJump()
    {
        int jumpDir = player.IsOnWallR ? -1 : 1;

        player.CurrentState = Player.State.WallJumping;
        player.WallJumped = true;

        rb.gravityScale = 0;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity = new Vector2(jumpDir * hJumpForce, vJumpForce) / Time.timeScale;

        startTimer = true;
        checkingForWall = false;
    }

    void StopJump()
    {
        player.CurrentState = Player.State.Falling;

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
        // if (inputBuffer.Count > 0) inputBuffer.Dequeue();
        inputTest = 0;
    }

    void OnDestroy()
    {
        GameManager.GetInstance.onGamePaused -= PauseResume;
    }
    #endregion
}
