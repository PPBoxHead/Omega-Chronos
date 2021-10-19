using UnityEngine;
using System.Collections.Generic;

public class WallSlide : MonoBehaviour
{
    #region Variables
    #region Setup
    private Rigidbody2D rb;

    private Player player;

    private List<Player.State> WallGrabState = new List<Player.State>() { Player.State.WallGrabing, Player.State.Falling };
    #endregion
    #region WallSlide
    private float gravityScale;
    #endregion
    #endregion

    #region Methods
    void Start()
    {
        player = GetComponent<Player>();
        rb = player.GetRb;

        gravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (!WallGrabState.Contains(player.CurrentState)) return;
        if (player.IsOnWallR || player.IsOnWallL)
        {
            rb.velocity = Vector2.zero;
        }
    }
    #endregion
}
