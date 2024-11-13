using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.AcceptInput = false;
    }

    public override void Exit()
    {
        base.Exit();
        player.AcceptInput = true;
        player.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(player.facingDir * player.dashForce*-1, 0);
    }
}
