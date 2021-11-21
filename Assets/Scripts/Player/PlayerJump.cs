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
    [Range(5, 30)] [SerializeField] private float vJumpForce = 22f;
    [Range(5, 30)] [SerializeField] private float hJumpForce = 11f;
    // llegue a este valor probando
    // es para que cuando estes en chronotime
    // el salto en la pared tenga la misma fuerza
    // que en tiempo normal
    private float vJumpForceCT = 20;
    private float hJumpForceCT = 8;
    private float currentVJumpForce;
    private float currentHJumpForce;

    [Header("Jump Config")]
    [Range(0, 0.5f)] [SerializeField] private float bufferTime = 0.15f;

    [Range(0, 1)] [SerializeField] private float jumpTimer = 0.2f;

    private bool stopMovement;
    private KeyCode jumpBtn;

    private SpriteRenderer spriteRenderer;
    private TimeManager timeManager;
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
        spriteRenderer = player.GetSpriteRenderer;
        timeManager = GameManager.GetInstance.GetTimeManager;

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
        // releaseJump = true;
        if (rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (!checkingForWall) checkingForWall = true;
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
            coyoteTimer += Time.deltaTime / timeManager.TimeScale;
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
    }

    void VerticalJump()
    {
        // algunas lineas estan repetidas con WallJump()
        // se podria mejorar un poco mas
        // pero creo que esta bien
        player.CurrentState = Player.State.Jumping;
        float vForce = timeManager.TimeScale == 1 ? vJumpForce : vJumpForceCT;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.drag = 1;
        rb.velocity = Vector2.up * (vForce / timeManager.TimeScale);
    }

    void WallJump()
    {
        Debug.Log("Aqui");
        int jumpDir = player.IsOnWallR ? -1 : 1;

        player.CurrentState = Player.State.WallJumping;
        player.WallJumped = true;

        float hForce = timeManager.TimeScale == 1 ? hJumpForce : hJumpForceCT;
        // por algun motivo le sumas 1 y queda igual 😂
        float vForce = timeManager.TimeScale == 1 ? vJumpForce : vJumpForceCT + 1;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity = new Vector2(jumpDir * hForce, vForce) / timeManager.TimeScale;

        checkingForWall = false;
    }

    void PauseResume(bool gamePaused)
    {
        stopMovement = gamePaused;
    }

    void RemoveAction()
    {
        inputTest = 0;
    }

    void OnDestroy()
    {
        GameManager.GetInstance.onGamePaused -= PauseResume;
    }
    #endregion
}
