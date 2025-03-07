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
    [SerializeField] private LayerMask whatIsWall;

    private int attackAnimCounter = 0;

    [Header("Player Stats Info")]
    public float moveSpeed = 6;
    public float jumpForce = 1;
    public float dashForce = 10.0f;
    public float jumpTimeLimited;    //跳跃时间限制
    public float dashTimeLimited;    //冲刺时间限制
    public int facingDir = 1; // 1 = left;, -1 = right

    [Header("Player Attack Info")]
    public Transform attackPos;
    public float attackRange;


    [Header("Variables")]
#region keyVariables
    public bool isStillJumping;
    public bool AcceptInput = true;
    public bool atBench;
    public bool IsGrounded;
    public bool isFacingLeft = true;
    public bool canDash = true;
#endregion

#region states
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerNormalSpellState normalSpellState { get; private set; }
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
        normalSpellState = new PlayerNormalSpellState(this, stateMachine, "NormalSpell");

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
        dashTimeLimited -= Time.deltaTime;
    #region input detection
        /////JUMP
        if (rb.velocity.y > 0.01 && !IsGrounded && AcceptInput && !(stateMachine.currentState is PlayerAttackState) && !(stateMachine.currentState is PlayerJumpState))
        {
            stateMachine.ChangeState(jumpState);
        }

        if (Input.GetKeyDown(KeyCode.Z) && IsGrounded && AcceptInput && !(stateMachine.currentState is PlayerJumpState))
        {   
            if (stateMachine.currentState is PlayerAttackState && !(anim.GetFloat("AttackCounter") == 2))
            {}
            else
                stateMachine.ChangeState(jumpState);
        }

        if (!(stateMachine.currentState is PlayerJumpState) && IsGrounded) //
        {
            jumpTimeLimited = .5f;
        }

        if (jumpTimeLimited >= 0 && isStillJumping && Input.GetKey(KeyCode.Z) && ((stateMachine.currentState is PlayerJumpState)||((stateMachine.currentState is PlayerAttackState) && !(anim.GetFloat("AttackCounter") == 2)))) ///
        {            
            SetVelocity(rb.velocity.x, jumpForce);
        }

        if (!Input.GetKey(KeyCode.Z) && !IsGrounded)
        {
            isStillJumping = false;
        }

        /////////FALL
        if (rb.velocity.y < 0 && !IsGrounded && !(stateMachine.currentState is PlayerAttackState) && !(stateMachine.currentState is PlayerFallState))
        {
            stateMachine.ChangeState(fallState);
        }

        if (rb.velocity.y < -21)
        {
            rb.velocity = new Vector2(rb.velocity.x, -21);
        }

        ////////DASH
        if (Input.GetKeyDown(KeyCode.C) && canDash && AcceptInput)
        {
            stateMachine.ChangeState(dashState);
            canDash = false;
        }
        if (IsGrounded && dashTimeLimited <= 0)
        {
            canDash = true;
        }
        if ((IsGrounded && Input.GetKeyDown(KeyCode.Z)) || (!IsGrounded && Input.GetKey(KeyCode.Z)))
        {
            isStillJumping = true;
        }
        if (!IsGrounded && rb.velocity.y <= 0)
        {
            isStillJumping = false;
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
        
        ////////NORMAL SPELL
        if (Input.GetKeyDown(KeyCode.F) && AcceptInput)
        {
            // stateMachine.ChangeState(normalSpellState);
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
    //落地检查
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            IsGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            IsGrounded = false;
        }
    }
}

