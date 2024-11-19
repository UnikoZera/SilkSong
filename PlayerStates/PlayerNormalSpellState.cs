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
        player.SetVelocity(0, 0);
    }
}
