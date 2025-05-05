using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputSystem inputControl;

    public Vector2 inputDirection;

    private Rigidbody2D rb;

    private SpriteRenderer sr;

    public Animator anim;

    public LayerMask platform;



    [Header("基本参数")]

    public float speed = 5;

    public float jumpForce = 2f;
    public float jumpTwoforce = 0.2f;
    public float jumpHoldforce = 0.1f;

    public float fallForce = 0.1f;
    public float fallAcceleration = 0.05f;

    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    public float dashForce = 3f;

    public float jumpTime = 0.0f;

    public float jumpCount = 2f;

    private float speedY;

    public int attackCount = 1;






    [Header("状态检测")]

    public bool isOnground = true;

    public bool isFall = false;

    public bool isDash = false;

    public bool isJump;
    public bool isJumpTwo;

    public bool isAttack = false;
    public bool isAttackOne = false;
    public bool isAttackTwo = false;

    public bool isRight = true;

    private bool canDash = true;

    private void Awake()
    {
        inputControl = new PlayerInputSystem();

        inputControl.Gameplayer.Jump.started += Jump;

        inputControl.Gameplayer.Attack.started += Attack;

        inputControl.Gameplayer.Dash.started += Dash;

        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();
    }


    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    void Update()
    {
       
        inputDirection = inputControl.Gameplayer.Move.ReadValue<Vector2>();

        if (inputDirection.y != 1 && inputDirection.y > 0) inputDirection.y = 1;
        if (inputDirection.y != -1 && inputDirection.y < 0) inputDirection.y = -1;
        if (inputDirection.x != 1 && inputDirection.x > 0) inputDirection.x = 1;
        if (inputDirection.x != -1 && inputDirection.x < 0) inputDirection.x = -1;

        isOnground = rb.IsTouchingLayers(platform);

        if (jumpCount<1 && isOnground) jumpCount = 1;

        speedY = rb.velocity.y;
    }

    private void FixedUpdate()
    {
        Move();
        Fall();
        Jumphold();
 
    }

    public void Move()
    {
        if (!isDash)
        {
            rb.velocity = new Vector2(inputDirection.x * speed, rb.velocity.y);

            //角色翻转
            if (inputDirection.x > 0) sr.flipX = true;
            if (inputDirection.x < 0) sr.flipX = false;

            //检测朝向
            if (Keyboard.current.dKey.isPressed) isRight = true;
            if (Keyboard.current.aKey.isPressed) isRight = false;
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (!isDash && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        // 准备阶段
        isDash = true;
        canDash = false;
        anim.SetBool("isDash", true);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0; // 取消重力影响

        // 确定方向
        float dashDirection = (inputDirection.x != 0) ?
            Mathf.Sign(inputDirection.x) :
            (isRight ? 1 : -1);

        // 应用冲刺速度
        rb.velocity = new Vector2(dashForce*dashDirection, 0);

        // 锁定输入
        inputControl.Gameplayer.Move.Disable();

        // 保持冲刺状态
        yield return new WaitForSeconds(dashDuration);

        // 恢复阶段
        inputControl.Gameplayer.Move.Enable();
        rb.gravityScale = originalGravity;
        anim.SetBool("isDash", false);
        isDash = false;

        // 冷却计时
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void Jump(InputAction.CallbackContext context)
    {
       //Debug.Log("Jump");

        if (jumpCount > 0&&!isDash)
        {
            rb.velocity = new Vector2(0, 0);

            if (isOnground)
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumpTime = Time.time + 0.2f;

                isJump = true;
            }
            else
            {
                //Debug.Log("JumpTwo");
                isJumpTwo = true;

                jumpCount--;

                fallForce = 0.05f;

                jumpTime = Time.time + 0.15f;

                rb.AddForce(new Vector2(0, jumpTwoforce), ForceMode2D.Impulse);

            }
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

    }
    //实现按的越久跳的越高
    private void Jumphold()
    {
        if (Keyboard.current.kKey.isPressed && !isOnground && Time.time < jumpTime)
        {
            rb.AddForce(new Vector2(0, jumpHoldforce), ForceMode2D.Impulse);
        }

    }
    //实现对下落状态的判断以及下落加速度
    private void Fall()
    {
        if (!isOnground)
        {
            fallForce += fallAcceleration;
            rb.AddForce(new Vector2(0, -fallForce), ForceMode2D.Impulse);
        }
        else fallForce = 0.05f;

        if (speedY < -0.05f ) isFall = true;
        else isFall = false;

        if (isFall)
        {
            isJump = false;
            isJumpTwo = false;
        }
    }

    //平a
    private void Attack(InputAction.CallbackContext context)
    {
        if(!isDash)
        {
            if (!isAttack && Keyboard.current.wKey.isPressed) anim.SetTrigger("isAttackUp");
            else if (!isAttack && !isOnground && Keyboard.current.sKey.isPressed) anim.SetTrigger("isAttackDown");
            else
            {
                if (!isAttack && isRight)
                {
                    if (attackCount == 1)
                    {
                        isAttackOne = true;
                        isAttackTwo = false;

                        anim.SetTrigger("isAttackOneRight");

                        attackCount++;
                    }
                    else
                    {
                        isAttackTwo = true;
                        isAttackOne = false;

                        anim.SetTrigger("isAttackTwoRight");

                        attackCount = 1;
                    }
                }

                if (!isAttack && !isRight)
                {
                    if (attackCount == 1)
                    {
                        isAttackOne = true;
                        isAttackTwo = false;

                        anim.SetTrigger("isAttackOneLeft");

                        attackCount++;
                    }
                    else
                    {
                        isAttackTwo = true;
                        isAttackOne = false;

                        anim.SetTrigger("isAttackTwoLeft");

                        attackCount = 1;
                    }
                }
            }
        }
        
       
    }

    public void SetDashFalse()
    {
        isDash = false;
    }

    public void SetDashTrue()
    {
        isDash = true;
    }

    public void SetAttactTrue()
    {
        isAttack = true;
    }

    public void SetAttactFalse()
    {
        isAttack = false;
    }
}
