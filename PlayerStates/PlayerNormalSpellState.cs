using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalSpellState : PlayerState
{
    public PlayerNormalSpellState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0, 0);//停止移动
    }
}
