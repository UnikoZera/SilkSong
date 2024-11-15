using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.anim.SetBool("isMoving", false);
        player.anim.SetBool("canRunning?", false);
    }

    public override void Update()
    {
        
        base.Update();

        player.FlipController(); //翻转角色

        player.SetVelocity(xInput*player.moveSpeed, rb.velocity.y); //给角色一个速度

        if (xInput == 0)
        {
            player.anim.SetBool("isMoving", false);
        }
        else
        {
            player.anim.SetBool("isMoving", true);
        }

        if (rb.velocity.x > 0 && player.isFacingLeft)
        { 
            player.anim.SetBool("isMoving", false);
        }
        else if (rb.velocity.x < 0 && !player.isFacingLeft)
        { 
            player.anim.SetBool("isMoving", false);
        }
    }
}
