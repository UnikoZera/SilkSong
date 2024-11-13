using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        player.FlipController(); //翻转角色
        player.SetVelocity(xInput*player.moveSpeed, rb.velocity.y); //给角色一个速度

        if (player.IsGrounded)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
