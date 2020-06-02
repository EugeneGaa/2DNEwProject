using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyController : IEnemyController
{
    [Header("----- Other Data -----")]
    public float moveRadius = 10;
    private float startPositionX;

    private void Awake()
    {
        defCol = this.GetComponent<Collider2D>();
        startPositionX = this.transform.position.x;
        rigid = this.GetComponent<Rigidbody2D>();
        enemyState = EnemyState.walk;
    }

    private void FixedUpdate()
    {
        if (this.transform.position.x > startPositionX + moveRadius || this.transform.position.x < startPositionX - moveRadius)
        {
            ChangeFaceDirection();
        }
        if (enemyState == EnemyState.walk)
        {
            Move();
        }
    }



    public override void Move()
    {
        direction = ((isFacingRight ? 1f : 0f) - (!isFacingRight ? 1f : 0f));
        rigid.velocity = new Vector2(direction*moveSpeed, rigid.velocity.y);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    public override void TakeDamage()
    {
        if(enemyHp>0)
        {
            enemyHp -= 1;
            anim.SetTrigger("Hit");
            enemyState = EnemyState.hit;
            rigid.velocity = new Vector2(hitForceX, hitForceY);

        }
        else if(enemyHp<=0)
        {
            Die();
            enemyState = EnemyState.die;
        }
    }

    public override void Die()
    {
        anim.SetTrigger("Die");
    }

    public void ChangeFaceDirection()
    {
        if(isFacingRight)
        {
            isFacingRight = false;

        }
        else
        {
            isFacingRight = true;
        }
    }

    public void SwitchAnimator()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.GetMask("atkCol"))
        {
            TakeDamage();
        }
    }
}
