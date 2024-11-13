using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{

    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        rb.gravityScale = 3; // 设置重力
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = 2; // 设置重力
    }

    public override void Update()
    {
        base.Update();
        player.FlipController(); //翻转角色
        player.SetVelocity(xInput*player.moveSpeed, rb.velocity.y); //给角色一个速度

        if (!player.IsGrounded && !(stateMachine.currentState is PlayerAttackState) && !Input.GetKey(KeyCode.Z))
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
