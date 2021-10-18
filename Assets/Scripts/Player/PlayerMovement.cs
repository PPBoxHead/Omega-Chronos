using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    #region Setup
    public float movementSpeed;
    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;
    #endregion
    #region Movement
    private Vector2 playerMomentum;
    private bool stopMovement;
    private float hMovement;
    private float vMovement;
    #endregion
    public Transform camTarget;
    [SerializeField] private float aheadAmount, aheadSpeed;
    public TimeManager timeManager; // esto despues puede estar como un singleton pero para el prototipo lo hago asi nomas
    public bool isDashing = false;
    [SerializeField] private ParticleSystem dustParticles;
    private PlayerJump playerJump;
    #endregion

    #region Methods
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerJump = GetComponent<PlayerJump>();
    }

    private void Start()
    {
        GameManager.GetInstance.onGamePaused += PauseResume;
        timeManager.onSlowMotion += OnSlowMotion;
    }

    private void Update()
    {
        if (stopMovement) return;

        // solucion super cutre cuando pones el tiempo mas lento
        // arreglar luego
        hMovement = Input.GetAxis("Horizontal") / Time.timeScale;
        FlipSprite();
        ManageDustParticles();
        if (hMovement >= 1)
        {
            hMovement = 1;
        }

        if (hMovement <= -1)
        {
            hMovement = -1;
        }
    }

    private void FixedUpdate()
    {
        if (stopMovement) return;

        // this moves the "lookahead" gameobject for camera
        if (hMovement != 0)
        {
            camTarget.localPosition = new Vector2(Mathf.Lerp(camTarget.localPosition.x, aheadAmount * hMovement, aheadSpeed * Time.deltaTime / Time.timeScale), camTarget.localPosition.y);
        }

        if (!isDashing)
        {
            rb2D.velocity = new Vector2(hMovement * movementSpeed, rb2D.velocity.y);
        }
    }

    void FlipSprite()
    {
        if (hMovement < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (hMovement > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void ManageDustParticles()
    {
        if (!playerJump.IsOnGround) return;
        if(Input.GetButtonDown("Horizontal"))
        {
            dustParticles.Play();
        }
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            dustParticles.Play();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            dustParticles.Play();
        }*/
    }

    /// <summary>
    /// stores player momentum before pause
    /// and gives it to player after pause
    /// </summary>
    public void PauseResume(bool gamePaused)
    {
        if (gamePaused)
        {
            playerMomentum = rb2D.velocity;
            rb2D.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.velocity = playerMomentum;
        }
        stopMovement = gamePaused;
    }


    /// <summary>
    /// Manages player speed
    /// on slow motion
    /// </summary>
    void OnSlowMotion(bool isTimeSlow)
    {
        if (isTimeSlow)
        {
            movementSpeed /= timeManager.GetSlowdownFactor;
        }
        else
        {
            movementSpeed *= timeManager.GetSlowdownFactor;
        }
    }

    private void OnDestroy()
    {
        GameManager.GetInstance.onGamePaused -= PauseResume;
    }
    #endregion
}
