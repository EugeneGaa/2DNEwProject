using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    wolf,
    spider,
    bird,
    test,

}

public enum EnemyState
{
    walk,
    hit,
    die,

}

public abstract class IEnemyController : MonoBehaviour
{
    [Header("----- Components -----")]
    public Collider2D defCol;
    public Animator anim;
    public Rigidbody2D rigid;

    [Header("----- Other General Data -----")]
    public EnemyType enemyType;
    public float moveSpeed;
    public int enemyHp;
    public int enemyDamage;
    public int energyDrop;
    [HideInInspector]
    public float direction;
    public float hitForceX;
    public float hitForceY;
    public EnemyState enemyState;

    [Header("----- First Order State Flag -----")]
    public bool isFacingRight;

    public virtual  void TakeDamage()
    {

    }

    public virtual void Die()
    {

    }

    public virtual void Move()
    {

    }

    public virtual void Attack()
    {

    }

    public void OnDieExit()
    {
        Destroy(this.gameObject);
    }




}
