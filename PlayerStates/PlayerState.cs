using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    private string animBoolName;
    protected Rigidbody2D rb;
    protected float xInput = 0;
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            xInput = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            xInput = 1;
        }
        else
        {
            xInput = 0;
        }
    }
}
