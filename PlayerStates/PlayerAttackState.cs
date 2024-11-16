using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.AcceptInput = true;
        player.SetVelocity(0, rb.velocity.y);
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && player.IsGrounded)
        {
            player.anim.SetBool("canRunning?", true);
        }
    }

    public override void Update()
    {
        base.Update();
        
        if (player.anim.GetFloat("AttackCounter") == 2 && player.anim.GetFloat("SpecialKeys") == 2)
        {
            player.SetVelocity(player.facingDir*12*-1, -12);
            player.AcceptInput = false;
            if (player.IsGrounded)
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
        else
        {
            player.SetVelocity(xInput*player.moveSpeed, rb.velocity.y);
        }
    }
}
