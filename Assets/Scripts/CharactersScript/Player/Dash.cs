using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Dash : MonoBehaviour
{
    #region Variables
    #region Setup
    private PlayerJump playerJump;
    private KeyCode dashButton;
    private Rigidbody2D rb2d;
    #endregion
    #region Dash
    public GameObject dashIndicator;
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float dashForceH;
    [SerializeField] private float dashForceV;
    private bool isDashing = false;
    private Vector2 dashDirection;
    private float dashTimer;
    // estas cosas repetidas luego se pueden arreglar pero para el prototipo estan ok
    private float hMovement;
    private float vMovement;
    #endregion
    #region Cooldown
    [SerializeField] private float dashCooldown = 1;
    private bool canDash = true;
    #endregion
    public TimeManager timeManager;
    private PlayerMovement playerMovement; // arreglar esto tambien jaj
    [SerializeField] private ParticleSystem dustParticles;
    #endregion

    #region Methods
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        dashButton = KeybindingsManager.GetInstance.GetDashButton;
        playerJump = GetComponent<PlayerJump>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rb2d.velocity.normalized);

        // for checking dash direction
        hMovement = Input.GetAxisRaw("Horizontal");
        vMovement = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(dashButton) && canDash)
        {
            StartDash();
        }

        if (isDashing)
        {
            Dashing();
        }
    }

    ///<summary>
    /// Starts dash
    ///</summary>
    void StartDash()
    {
        dustParticles.Play();
        dashIndicator.SetActive(false);
        dashTimer = dashDuration; // sets for how long the dash lasts
        rb2d.gravityScale = 0;
        rb2d.velocity = new Vector2(0, 0);
        isDashing = true;
        canDash = false; // for cooldown
        playerMovement.isDashing = isDashing;
        dashDirection = new Vector2(hMovement, vMovement); // dash direction
    }

    void Dashing()
    {
        dashTimer -= Time.unscaledDeltaTime;
        // ver de pasarlo a una corutina
        // sacar gravedad y velocidad cuando lo hagas

        //rb2d.AddForce(new Vector2((dashDirection.x * dashForceH), (dashDirection.y * dashForceV)));
        rb2d.velocity = new Vector2(dashDirection.x * dashForceH / Time.timeScale, dashDirection.y * dashForceV / Time.timeScale);

        if (dashTimer <= 0)
        {
            isDashing = false;
            rb2d.gravityScale = playerJump.GetGravityScale / Mathf.Pow(Time.timeScale, 2);
            rb2d.velocity = Vector2.zero;
            playerMovement.isDashing = isDashing;
            StartCoroutine(DashCooldown());
        }
    }

    ///<summary>
    /// manages cooldown for dash
    /// change this to a scriptable object
    ///</summary>
    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown * Time.timeScale);
        canDash = true;
        dashIndicator.SetActive(true);
        Debug.Log("You can dash again");
    }
    #endregion
}
