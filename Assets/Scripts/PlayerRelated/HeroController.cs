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
        attack,
        hit,
    }
public class HeroController : MonoBehaviour
{
    [Header("----- Components -----")]
    public IPlayerInput pi;
    public Rigidbody2D rigid;
    public GameManager gm;
    public LayerMask ground;
    public Transform groundCheck;
    public Transform ceiling;
    public Animator anim;
    public State state;
    public BattleManager bm;
    public Collider2D rightAtkCol;
    public Collider2D leftAtkCol;
    public Collider2D defCol;
    public LayerMask traps;

    [Header("----- Other Data -----")]
    public float walkSpeed = 3;
    public float jumpForce = 5;
    public int jumpCount = 2;
    public float dashForce = 5;
    public int dashCount = 2;
    public float hitForceX = 0.5f;
    public float hitForceY = 0.5f;
    public float immrotalTime;
    private float hitTimer = 0;


    [Header("----- First Order State Flags -----")]
    public bool jumpPressed;
    public bool dashPressed;
    public bool isGround;
    public bool isJump;
    public bool isDash;
    public bool isFacingRight;
    public bool canHit;
    public bool isTrap;//有机会和isGround共存，搭建关卡的时候注意


    private void Awake()
    {
        pi = this.GetComponent<IPlayerInput>();
        rigid = this.GetComponent<Rigidbody2D>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        groundCheck = transform.DeepFind("groundCheck");
        ceiling = transform.DeepFind("ceiling");
        anim = this.GetComponent<Animator>();
        bm = this.GetComponentInChildren<BattleManager>();
        bm.hc = this;
        state = State.normal;
        rightAtkCol = transform.DeepFind("rightAttack").GetComponent<Collider2D>();
        leftAtkCol = transform.DeepFind("leftAttack").GetComponent<Collider2D>();
    }

    private void Start()
    {
        defCol = bm.defCol;
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
            state = State.attack;
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
        else if (state != State.dash&&state!=State.hit)
        {
            Move();
        }

        if (isGround && (state == State.normal || state == State.walk))
        {
            dashCount = 1;
        }

        OnTrap();

        ResetCanHit();

    }



    /// <summary>
    /// 角色受伤处理
    /// </summary>
    /// <param name="enemyPositionX"></param>
    public void TakeDamage(float enemyPositionX)
    {
        if (canHit)
        {
            canHit = false;
            if (gm.heroHp > 0)
            {
                gm.heroHp -= 1;
                anim.SetTrigger("Hit");
                state = State.hit;
                if (enemyPositionX > this.transform.position.x)//敌人在右边
                {
                    rigid.velocity = new Vector2(-hitForceX, hitForceY);
                }
                else if (enemyPositionX < this.transform.position.x)//敌人在左边
                {
                    rigid.velocity = new Vector2(hitForceX, hitForceY);
                }
            }
            else if (gm.heroHp <= 0)
            {
                anim.SetTrigger("Die");
                Die();
            }
        }
        else
        {
            //donothing
        }

    }

    /// <summary>
    /// 角色死亡处理
    /// </summary>
    public void Die()
    {

    }


    /// <summary>
    /// 设置动画状态机参数
    /// </summary>
    public void SetAnimation()
    {
        anim.SetFloat("WalkValue", Mathf.Abs(pi.dRight));
        anim.SetFloat("FallValue", rigid.velocity.y);
        anim.SetBool("IsGround", isGround);
    }


    /// <summary>
    /// 重置受伤后霸体功能
    /// </summary>
    private void ResetCanHit()
    {
        if (!canHit)
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= immrotalTime)
            {
                canHit = true;
                hitTimer = 0;
            }
        }
    }


    #region 接受玩家信号后的动作处理部分
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
    #endregion


    #region 动画机返还信息处理中心
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
    #endregion


    #region 控制战斗触发器部分
    public void AtkColOn()
    {
        if(isFacingRight)
        {
            Debug.Log("右边攻击开启");
            rightAtkCol.enabled = true;
        }
        else
        {
            Debug.Log("左边攻击开启");
            leftAtkCol.enabled = true;
        }
    }
    public void AtkColOff()
    {
        Debug.Log("攻击开关闭");
        rightAtkCol.enabled = false;
        leftAtkCol.enabled = false;
    }
    public void DefColOn()
    {

    }
    public void DefColOff()
    {

    }
    #endregion


    /// <summary>
    /// 检测地面状态及检测陷阱状态
    /// </summary>
    public void DetectGround()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        isTrap = Physics2D.OverlapCircle(groundCheck.position, 0.1f, traps);
    }

    /// <summary>
    /// 触碰到陷阱后的处理
    /// </summary>
    public void OnTrap()
    {
        if(isTrap)
        {
            if(isFacingRight)
            {
                TakeDamage(this.transform.position.x + 1);
            }
            else
            {
                TakeDamage(this.transform.position.x - 1);
            }
        }
    }
}
