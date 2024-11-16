using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    private int instantDashDir; // -1 = left, 1 = right

    public override void Enter()
    {
        base.Enter();
        player.AcceptInput = false;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            instantDashDir = 1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            instantDashDir = -1;
        }
        else
        {
            instantDashDir = player.facingDir;
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.AcceptInput = true;
        player.SetVelocity(0, 0);
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && player.IsGrounded)
        {
            player.anim.SetBool("canRunning?", true);
        }
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(instantDashDir * player.dashForce*-1, 0);
        player.FlipController();
    }
}
