using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State
    {
        normal,
        walk,
        jump,
        dash,
        fall,
    }
public class HeroController : MonoBehaviour
{
    public IPlayerInput pi;
    public Rigidbody2D rigid;
    public LayerMask ground;
    public Transform groundCheck;
    public Transform ceiling;
    public Animator anim;
    public State state;

    [Header("----- Other Data -----")]
    public float walkSpeed = 3;
    public float jumpForce = 5;
    public int jumpCount = 2;
    public float dashForce = 5;
    public int dashCount = 2;

    [Header("----- First Order State Flags -----")]
    public bool jumpPressed;
    public bool dashPressed;
    public bool isGround;
    public bool isJump;
    public bool isDash;
    public bool isFacingRight;


    private void Awake()
    {
        pi = this.GetComponent<IPlayerInput>();
        rigid = this.GetComponent<Rigidbody2D>();
        groundCheck = transform.DeepFind("groundCheck");
        ceiling = transform.DeepFind("ceiling");
        anim = this.GetComponent<Animator>();
        state = State.normal;
    }

    private void Update()
    {
        if(pi.jump)
        {
            jumpPressed = true;
        }
        if(pi.dash)
        {
            dashPressed = true;
            state = State.dash;
        }
        if(pi.attack)
        {
            anim.SetTrigger("Attack");
        }
        SetAnimation();
    }

    private void FixedUpdate()
    {
        DetectGround();
        //移动速度，惯性可以用lerp做
        Jump();
        if(state==State.dash)
        {
            Dash();
        }
        else if (state != State.dash)
        {
            Move();
        }

        if(isGround&&state==State.normal)
        {
            dashCount = 1;
        }
    }

    public void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (isGround && jumpPressed)
        {
            anim.SetTrigger("Jump");
            isJump = true;
            state = State.jump;
            rigid.velocity += new Vector2(rigid.velocity.x, jumpForce);
            jumpCount -= 1;
            jumpPressed = false;
        }
        else if (isJump && jumpPressed && jumpCount > 0)
        {
            anim.SetTrigger("Jump");
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            rigid.velocity += new Vector2(rigid.velocity.x, jumpForce);
            jumpCount -= 1;
            jumpPressed = false;
            state = State.jump;
        }


    }

    public void Dash()
    {
        if(dashPressed&&dashCount>0)
        {
            anim.SetTrigger("Dash");
            Vector2 temp1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dashDir = (temp1 - this.rigid.position).normalized;
            rigid.velocity = dashDir * dashForce;
            transform.localScale = new Vector3(((temp1.x>this.rigid.position.x)?1:-1), 1, 1);
            dashPressed = false;
            dashCount -= 1;
        }
    }

    public void DetectGround()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
    }

    public void Move()
    {
        rigid.velocity = new Vector2(pi.dRight * walkSpeed, rigid.velocity.y);
        if (pi.dRight != 0)
        {
            transform.localScale = new Vector3(pi.dRight, 1, 1);
        }
        if (pi.dRight == 1)
        {
            isFacingRight = true;
        }
        else if (pi.dRight == -1)
        {
            isFacingRight = false;
        }
    }

    public void SetAnimation()
    {
        anim.SetFloat("WalkValue", Mathf.Abs(pi.dRight));
        anim.SetFloat("FallValue", rigid.velocity.y);
        anim.SetBool("IsGround", isGround);
    }


    public void OnDashEnter()
    {
        rigid.gravityScale = 0;
    }

    public void OnDashExit()
    {
        dashPressed = false;
        rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y*0.1f);
        rigid.gravityScale = 3;
    }

    public void OnWalkEnter()
    {
        state = State.walk;
    }

    public void OnIdleEnter()
    {
        state = State.normal;
    }

    public void OnFallEnter()
    {
        state = State.fall;
    }
}
