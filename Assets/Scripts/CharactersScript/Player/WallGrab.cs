using UnityEngine;

public class WallGrab : MonoBehaviour
{
    public bool isOnWall = false;
    public bool startTimer;
    [SerializeField] private float raycastLenght = 0.26f;
    [SerializeField] private float jumpTimer = 0.1f;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;
    private bool checkingForWall = true;
    private bool releaseJump = false;
    private PlayerJump playerJump;
    private float jumpDirection;
    private float gravityScale;
    private Rigidbody2D rb;
    private float timer;
    private KeyCode jumpButton;
    [SerializeField] private Vector3 rayOff = new Vector3(0, 0.8f, 0);
    [SerializeField] private ParticleSystem dustParticles;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityScale = rb.gravityScale;
        playerJump = GetComponent<PlayerJump>();
        playerMovement = GetComponent<PlayerMovement>();
        jumpButton = KeybindingsManager.GetInstance.GetJumpButton;
        timer = jumpTimer;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {

        if (checkingForWall && isWallColliding())
        {
            isOnWall = true;
        }
        else
        {
            isOnWall = false;
        }

        if (isOnWall)
        {
            rb.velocity = Vector2.zero;

            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                rb.gravityScale = 0;
                playerMovement.isDashing = true;
            }
            else
            {
                rb.gravityScale = gravityScale / Mathf.Pow(Time.timeScale, 2);
                playerMovement.isDashing = false;
            }
        }

        //if (isOnWall && !startTimer && Input.GetKey(KeyCode.LeftControl))
        //{
        //rb.gravityScale = 0;
        //rb.velocity = Vector2.zero;
        //playerMovement.isDashing = true;
        //}

        if (isOnWall && !playerJump.IsOnGround && Input.GetKeyDown(jumpButton))
        {
            WallJump();
        }

        if (Input.GetKeyUp(jumpButton) && startTimer)
        {
            releaseJump = true;
        }

        if (startTimer)
        {
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

    private bool isWallColliding()
    {
        RaycastHit2D rayhit1 = Physics2D.Raycast(transform.position + rayOff, Vector3.right, raycastLenght);
        RaycastHit2D rayhit2 = Physics2D.Raycast(transform.position + rayOff, Vector3.left, raycastLenght);

        bool isCollidingRight = rayhit1.collider && (rayhit1.collider.CompareTag("WallGrab"));
        bool isCollidingLeft = rayhit2.collider && (rayhit2.collider.CompareTag("WallGrab"));

        //esta horrible lo se como muchas otras cosas
        //pero funcionan
        if (isCollidingRight)
        {
            //spriteRenderer.flipX = true;
            jumpDirection = -1;
        }
        else
        {
            if (isCollidingLeft)
            {
                //spriteRenderer.flipX = false;
                jumpDirection = 1;
            }
        }

        DrawRaycast();
        return isCollidingRight || isCollidingLeft;
    }

    void DrawRaycast()
    {
        Debug.DrawRay(transform.position + rayOff, Vector3.right * raycastLenght, Color.green);
        Debug.DrawRay(transform.position + rayOff, Vector3.left * raycastLenght, Color.green);
    }

    private void WallJump()
    {
        // esta se puede combinar con Jump() de PlayerJump haciendole unos pocos cambios
        // de momento es un copio y pego de ese metodo
        dustParticles.Play();
        checkingForWall = false;
        playerMovement.isDashing = true;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(jumpDirection, 1) * playerJump.GetJumpForce / Time.timeScale;
        startTimer = true;
    }

    void StopJump()
    {
        playerMovement.isDashing = false;
        checkingForWall = true;
        rb.gravityScale = gravityScale / Mathf.Pow(Time.timeScale, 2);
        releaseJump = false;
        startTimer = false;
        timer = jumpTimer;
    }
}
