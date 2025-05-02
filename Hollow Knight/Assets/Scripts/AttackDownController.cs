using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDownController : MonoBehaviour
{
    private SpriteRenderer sr;
    public PlayerController UsingPlayController;

    public bool isRight;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

        Flip();
    }

    private void Flip()
    {
        isRight = UsingPlayController.isRight;

        if (isRight) sr.flipX = true;
        else sr.flipX = false;
    }

}
