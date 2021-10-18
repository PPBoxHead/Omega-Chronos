using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerJump : MonoBehaviour
{
    #region Variables
    #region Setup
    [SerializeField] private float jumpForce; // fuerza del salto
    [SerializeField] private float bufferTime = 0.1f; // el tiempo del buffer
    private float walkingTreshold = 0.05f;
    private KeyCode jumpButton;
    Queue<KeyCode> inputBuffer;
    private Rigidbody2D rb2D;
    #endregion
    #region VariableJump
    [SerializeField] private float jumpTimer = 0.2f; // cuanto tiempo esta "Subiendo"
    private bool releaseJump = false;
    private bool isOnGround = false;
    private bool startTimer = false;
    private float gravityScale;
    private float timer;
    #endregion
    #region CoyoteTime
    [SerializeField] private float coyoteFrames = 3f; // coyote time
    [SerializeField] private float coyoteTimer;
    [SerializeField] private Vector3 raycastOffset = new Vector3(0.18f, 0, 0);
    [SerializeField] private float raycastLenght = 0.7f;
    #endregion
    private bool stopMovement;
    public TimeManager timeManager;
    private Animator animator;
    private WallGrab wallGrab;
    [SerializeField] private ParticleSystem dustParticles;
    #endregion

    #region Methods
    private void Awake()
    {
        inputBuffer = new Queue<KeyCode>();
        rb2D = GetComponent<Rigidbody2D>();
        gravityScale = rb2D.gravityScale;
        timer = jumpTimer;
        animator = GetComponentInChildren<Animator>();
        wallGrab = GetComponent<WallGrab>();
    }

    private void Start()
    {
        GameManager.GetInstance.onGamePaused += PauseResume;

        jumpButton = KeybindingsManager.GetInstance.GetJumpButton;
    }

    private void Update()
    {
        if (stopMovement) return;

        ManageAnimations();

        if (isGroundColliding())
        {
            coyoteTimer = 0; // resets coyoteTimer
            isOnGround = true;
        }
        else
        {
            coyoteTimer += 1; // start adding to coyoteTimer
            isOnGround = false;
        }

        if (Input.GetKeyDown(jumpButton))
        {
            inputBuffer.Enqueue(jumpButton); // saves space to buffer
            Invoke("RemoveAction", bufferTime * Time.timeScale); // deletes action after 0.1f
        }

        // dynamic jump
        if ((isOnGround || coyoteTimer < coyoteFrames) && inputBuffer.Count > 0)
        {
            if (inputBuffer.Peek() == jumpButton)
            {
                // peeks into buffer to check for jumpButton
                inputBuffer.Clear(); // clears buffer when you jump to avoid double jump on the same frame
                Jump();
            }
        }

        if (Input.GetKeyUp(jumpButton))
        {
            releaseJump = true;
        }

        if (startTimer)
        {
            // stops jump
            timer -= Time.unscaledDeltaTime;
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

    private bool isGroundColliding()
    {
        // checks if player is colliding with floor
        // and returns bool

        RaycastHit2D rayHit1 = Physics2D.Raycast(transform.position, Vector3.down, raycastLenght);
        RaycastHit2D rayHit2 = Physics2D.Raycast(transform.position + raycastOffset, Vector3.down, raycastLenght);
        RaycastHit2D rayHit3 = Physics2D.Raycast(transform.position - raycastOffset, Vector3.down, raycastLenght);

        bool isCollidingCenter = rayHit1.collider && (rayHit1.collider.CompareTag("Floor"));
        bool isCollidingRight = rayHit2.collider && (rayHit2.collider.gameObject.CompareTag("Floor"));
        bool isCollidingLeft = rayHit3.collider && (rayHit3.collider.gameObject.CompareTag("Floor"));

        Debug.DrawRay(transform.position, Vector3.down * raycastLenght, Color.red);
        Debug.DrawRay(transform.position + raycastOffset, Vector3.down * raycastLenght, Color.red);
        Debug.DrawRay(transform.position - raycastOffset, Vector3.down * raycastLenght, Color.red);

        return isCollidingCenter || isCollidingLeft || isCollidingRight;
    }

    private void Jump()
    {
        dustParticles.Play();
        isOnGround = false;

        rb2D.gravityScale = 0;
        rb2D.velocity = Vector2.zero;
        rb2D.velocity = Vector2.up * jumpForce / Time.timeScale;
        //rb2D.AddForce(Vector2.up * jumpForce);
        startTimer = true;
    }

    private void StopJump()
    {
        rb2D.gravityScale = gravityScale / Mathf.Pow(Time.timeScale, 2);
        releaseJump = false;
        timer = jumpTimer;
        startTimer = false;
    }

    private void RemoveAction()
    {
        if (inputBuffer.Count > 0) inputBuffer.Dequeue();
    }

    private void PauseResume(bool gamePaused)
    {
        stopMovement = gamePaused;
    }

    void ManageAnimations()
    {
        if (wallGrab.isOnWall)
        {
            animator.Play("playerWallgrab");
        }
        else if (isOnGround && Mathf.Abs(rb2D.velocity.y) <= walkingTreshold)
        {
            if (Mathf.Abs(rb2D.velocity.x) >= walkingTreshold)
            {
                animator.Play("playerRun");
            }
            else animator.Play("playerIdle");
        }
        else
        {
            if (startTimer || wallGrab.startTimer) animator.Play("playerJump");
            else animator.Play("playerFall");
        }
    }

    public bool IsOnGround
    {
        get { return isOnGround; }
    }

    public float GetGravityScale
    {
        get { return gravityScale; }
    }

    public float GetJumpForce
    {
        get { return jumpForce; }
    }
    #endregion
}
