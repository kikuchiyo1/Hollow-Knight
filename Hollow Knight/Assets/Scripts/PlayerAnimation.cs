using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;

    private Rigidbody2D rb;

    public PlayerController UsingPlayController;

    private bool isOnground;
    private bool isFall;
    private bool isJump;
    private bool isJumpTwo;
    private bool isAttackOne;
    private bool isAttackTwo;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SetAnimation();
        StatusDetection();

    }

    private void SetAnimation()
    {

        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));

        if (isOnground) anim.SetBool("isOnground", true);
        else anim.SetBool("isOnground", false);

        if (isFall) anim.SetBool("isFall", true);
        else anim.SetBool("isFall", false);

        if (isJump) anim.SetBool("isJump", true);
        else anim.SetBool("isJump", false);

        if (isJumpTwo) anim.SetBool("isJumpTwo", true);
        else anim.SetBool("isJumpTwo", false);

    }

    private void StatusDetection()
    {
        isOnground = UsingPlayController.isOnground;
        isFall = UsingPlayController.isFall;
        isJump = UsingPlayController.isJump;
        isJumpTwo = UsingPlayController.isJumpTwo;
    }
}
