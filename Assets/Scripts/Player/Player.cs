using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    // que los demas referencien a player.cs
    // pero que player no referencie a los demas (linea 87 aprox)
    #region enum
    public enum State
    {
        Idle,
        Walking,
        Jumping,
        WallJumping,
        Falling,
        WallGrabing,
    }
    #endregion
    #region Variables
    #region Setup
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private float gravityScale;
    private TimeManager timeManager;

    private bool wallJumped = false;
    private bool isOnGround;
    private bool isOnWallR;
    private bool isOnWallL;

    private Transform checkpoint;
    private State currentState = State.Idle;
    #region Animations
    private string[] animations = { "playerIdle", "playerRun", "playerJump", "playerJump", "playerFall", "playerWallgrab" };
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    #endregion
    #endregion
    #region Movement
    [Range(5, 15)] [SerializeField] private float movementSpeed = 10;
    private float currentMovementSpeed;
    private float movTreshold = 0.05f;
    private bool onSlowmo = false;
    private float hMovement;
    private float vMovement;
    #endregion
    #region Pause
    private Vector2 playerMomentum;
    private bool stopMovement;
    private float playerVel;
    #endregion
    #region Raycast
    private Vector3 vRayOff = new Vector3(0.18f, 0, 0);
    private Vector3 hRayOffLow = new Vector3(0, 0.2f, 0);
    private Vector3 hRayOffHigh = new Vector3(0, 1.2f, 0);
    private float vRayLength = 0.05f;
    private float hRayLength = 0.37f;
    #endregion
    #endregion

    #region Methods
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        gravityScale = rb.gravityScale;
    }

    void Start()
    {
        timeManager = GameManager.GetInstance.GetTimeManager;

        GameManager.GetInstance.onGamePaused += PauseResume;
        GameManager.GetInstance.onDeath += OnDeath;
        timeManager.onSlowMotion += OnSlowMotion;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        currentMovementSpeed = movementSpeed;

        checkpoint = GameObject.Find("Checkpoint").transform;
        this.transform.position = checkpoint.position;
    }

    public void OnMove(InputValue input)
    {
        hMovement = input.Get<float>() / Time.timeScale;
    }

    void Update()
    {
        if (stopMovement) return;

        if (onSlowmo)
        {
            // with hMovement = input.Get<float>() / Time.timeScale; hmovement can be > 1
            // but without it it will move reaaaaally slow
            if (hMovement >= 1) hMovement = 1;
            else if (hMovement <= -1) hMovement = -1;
        }

        currentMovementSpeed = movementSpeed / Time.timeScale;
        animator.speed = 1 / Time.timeScale;
        // rb.gravityScale = gravityScale / Mathf.Pow(Time.timeScale, 2);

        isOnWallR = isWallCollidingR();
        isOnWallL = isWallCollidingL();

        isOnGround = isGroundColliding();

        if (isOnGround) wallJumped = false;

        if ((isOnWallR || isOnWallL) && currentState != State.WallJumping)
        {
            currentState = State.WallGrabing;
            wallJumped = false;
        }

        DrawRay();
        FlipSprite();
        ManageStates();
        PlayAnimation();
    }

    void PauseResume(bool gamePaused)
    {
        if (gamePaused)
        {
            playerMomentum = rb.velocity;
            rb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = playerMomentum;
        }
        stopMovement = gamePaused;
    }

    // instantly moves player
    public void MovePlayer()
    {
        this.transform.position = checkpoint.position;
    }

    void OnDeath(float duration)
    {
        MovePlayer();
        // tambien frenar la velocidad que tenia anteriormente
    }

    void ManageStates()
    {
        if (currentState == State.WallGrabing && !isWallCollidingL() && !isWallCollidingR())
        {
            // cuando te soltas
            currentState = State.Falling;
            return;
        }

        if (rb.velocity.y < -movTreshold && currentState != State.WallGrabing)
        {
            currentState = State.Falling;
        }
        else
        {
            if (isOnGround && Mathf.Abs(rb.velocity.y) <= movTreshold)
            {
                if (hMovement != 0)
                {
                    currentState = State.Walking;
                }
                else
                {
                    currentState = State.Idle;
                }
            }
        }
    }

    public void PlayAnimation()
    {
        switch (currentState)
        {
            case State.Idle:
                animator.Play(animations[(int)currentState]);
                break;
            case State.Walking:
                animator.Play(animations[(int)currentState]);
                break;
            case State.Jumping:
                animator.Play(animations[(int)currentState]);
                break;
            case State.Falling:
                animator.Play(animations[(int)currentState]);
                break;
            case State.WallGrabing:
                animator.Play(animations[(int)currentState]);
                break;
            case State.WallJumping:
                animator.Play(animations[(int)currentState]);
                break;
        }
    }

    void FlipSprite()
    {
        if (isOnWallR)
        {
            spriteRenderer.flipX = true;
            return;
        }
        else
        {
            if (isOnWallL)
            {
                spriteRenderer.flipX = false;
                return;
            }
        }
        if (hMovement < 0) spriteRenderer.flipX = true;
        else if (hMovement > 0) spriteRenderer.flipX = false;
    }

    bool isGroundColliding()
    {
        RaycastHit2D rayhit1 = Physics2D.Raycast(transform.position, Vector3.down, vRayLength);
        RaycastHit2D rayhit2 = Physics2D.Raycast(transform.position + vRayOff, Vector3.down, vRayLength);
        RaycastHit2D rayhit3 = Physics2D.Raycast(transform.position - vRayOff, Vector3.down, vRayLength);

        bool colCenter = rayhit1.collider && (rayhit1.collider.CompareTag("Floor"));
        bool colRight = rayhit2.collider && (rayhit2.collider.CompareTag("Floor"));
        bool colLeft = rayhit3.collider && (rayhit3.collider.CompareTag("Floor"));

        return colCenter || colRight || colLeft;
    }

    bool isWallCollidingR()
    {
        // right raycast
        RaycastHit2D rayhitRL = Physics2D.Raycast(transform.position + hRayOffLow, Vector3.right, hRayLength);
        RaycastHit2D rayhitRH = Physics2D.Raycast(transform.position + hRayOffHigh, Vector3.right, hRayLength);

        // right
        bool colRL = rayhitRL.collider && rayhitRL.collider.CompareTag("WallGrab");
        bool colRH = rayhitRH.collider && rayhitRH.collider.CompareTag("WallGrab");

        return (colRL && colRH);
    }

    bool isWallCollidingL()
    {
        // left raycast
        RaycastHit2D rayhitLL = Physics2D.Raycast(transform.position + hRayOffLow, Vector3.left, hRayLength);
        RaycastHit2D rayhitLH = Physics2D.Raycast(transform.position + hRayOffHigh, Vector3.left, hRayLength);

        //left
        bool colLL = rayhitLL.collider && rayhitLL.collider.CompareTag("WallGrab");
        bool colLH = rayhitLH.collider && rayhitLH.collider.CompareTag("WallGrab");

        return (colLL && colLH);
    }

    void DrawRay()
    {
        // vertical ray
        Debug.DrawRay(transform.position, Vector3.down * vRayLength, Color.red);
        Debug.DrawRay(transform.position + vRayOff, Vector3.down * vRayLength, Color.red);
        Debug.DrawRay(transform.position - vRayOff, Vector3.down * vRayLength, Color.red);

        // horizontal ray
        Debug.DrawRay(transform.position + hRayOffLow, Vector3.right * hRayLength, Color.green);
        Debug.DrawRay(transform.position + hRayOffHigh, Vector3.right * hRayLength, Color.green);
        Debug.DrawRay(transform.position + hRayOffLow, Vector3.left * hRayLength, Color.green);
        Debug.DrawRay(transform.position + hRayOffHigh, Vector3.left * hRayLength, Color.green);
    }

    void OnSlowMotion(bool isTimeSlow)
    {
        onSlowmo = isTimeSlow;
        // no paso los cambios de velocidades aca
        // porque como es gradual entonces tienen que
        // ser en el update
    }

    void OnDestroy()
    {
        GameManager.GetInstance.onGamePaused -= PauseResume;
        GameManager.GetInstance.onDeath -= OnDeath;
    }
    #endregion

    #region Getter/Setter
    public Rigidbody2D GetRb
    {
        get { return rb; }
    }

    public bool IsOnGround
    {
        get { return isOnGround; }
    }

    public bool IsOnWallR
    {
        get { return isOnWallR; }
    }

    public bool IsOnWallL
    {
        get { return isOnWallL; }
    }

    public float HMovement
    {
        get { return hMovement; }
    }

    public bool WallJumped
    {
        get { return wallJumped; }
        set { wallJumped = value; }
    }

    public State CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    public float CurrentMovementSpeed
    {
        get { return currentMovementSpeed; }
    }

    public bool IsOnSlowMo
    {
        get { return onSlowmo; }
    }
    #endregion
}
