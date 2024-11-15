using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get; private set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    [SerializeField] private LayerMask whatIsGround; 

    private int attackAnimCounter = 0;

    public float moveSpeed = 6;
    public float jumpForce = 1;
    public float dashForce = 10.0f;
    public float jumpTimeLimited;    //跳跃时间限制
    public bool isStillJumping;

    public int facingDir = 1; // 1 = left;, -1 = right


#region states
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerFallState fallState { get; private set; }
#endregion

#region keyVariables
    public bool AcceptInput = true;
    public bool atBench;
    public bool IsGrounded;
    public bool isFacingLeft = true;
    public bool canDash = true;
#endregion
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "isMoving");
        jumpState = new PlayerJumpState(this, stateMachine, "isJumping");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        attackState = new PlayerAttackState(this, stateMachine, "Attacking");
        fallState = new PlayerFallState(this, stateMachine, "isFalling");

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        stateMachine.Initialize(new PlayerIdleState(this, stateMachine, "Idle"));
    }

    private void Update()
    {
        stateMachine.currentState.Update();
        jumpTimeLimited -= Time.deltaTime;
    #region input detection
        /////JUMP
        if (rb.velocity.y > 0.01 && !IsGrounded && AcceptInput && !(stateMachine.currentState is PlayerAttackState) && !(stateMachine.currentState is PlayerJumpState))
        {
            stateMachine.ChangeState(jumpState);
        }

        if (Input.GetKeyDown(KeyCode.Z) && IsGrounded && AcceptInput && !(stateMachine.currentState is PlayerJumpState))
        {   
            stateMachine.ChangeState(jumpState);
        }

        if (!(stateMachine.currentState is PlayerJumpState) && IsGrounded) //
        {
            jumpTimeLimited = .5f;
        }

        if (jumpTimeLimited >= 0 && isStillJumping && ((stateMachine.currentState is PlayerJumpState)||((stateMachine.currentState is PlayerAttackState) && !(anim.GetFloat("AttackCounter") == 2) && !IsGrounded))) ///
        {
            SetVelocity(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyUp(KeyCode.Z) && !IsGrounded)
        {
            isStillJumping = false;
        }

        /////////FALL
        if (rb.velocity.y < 0 && !IsGrounded && !(stateMachine.currentState is PlayerAttackState) && !(stateMachine.currentState is PlayerFallState))
        {
            stateMachine.ChangeState(fallState);
        }

        ////////DASH
        if (Input.GetKeyDown(KeyCode.C) && canDash && AcceptInput)
        {
            stateMachine.ChangeState(dashState);
            canDash = false;
        }
        if (IsGrounded)
        {
            canDash = true;
            isStillJumping = true;
        }

        ////////ATTACK
        if (Input.GetKeyDown(KeyCode.X) && AcceptInput)
        {
            if (attackAnimCounter == 0)
            {
                anim.SetFloat("AttackCounter", 1);
                attackAnimCounter = 1;
            }
            else if (attackAnimCounter == 1)
            {
                anim.SetFloat("AttackCounter", 0);
                attackAnimCounter = 0;
            }

            //////////special keys input.
            if (Input.GetKey(KeyCode.UpArrow))
            {
                anim.SetFloat("SpecialKeys", 1);
                anim.SetFloat("AttackCounter", 2);
            }
            else if (Input.GetKey(KeyCode.DownArrow) && !IsGrounded)
            {
                anim.SetFloat("SpecialKeys", 2);
                anim.SetFloat("AttackCounter", 2);
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                anim.SetFloat("SpecialKeys", 0);
                anim.SetFloat("AttackCounter", 0);
            }

            stateMachine.ChangeState(attackState);
        }
    #endregion
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)//移动动代码
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
    }

    public void Flip()
    {
        facingDir *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
        isFacingLeft = !isFacingLeft;
    }

    public void FlipController()
    {
        if (rb.velocity.x > 0 && isFacingLeft)
        { 
            Flip();
        }
        else if (rb.velocity.x < 0 && !isFacingLeft)
        { 
            Flip();
        }
    }
    // 触发器进入时调用
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            IsGrounded = true;
        }
    }
    // 触发器退出时调用
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            IsGrounded = false;
        }
    }
}

