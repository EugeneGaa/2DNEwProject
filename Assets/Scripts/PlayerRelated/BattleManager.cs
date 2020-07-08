using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public LayerMask enemy;
    public LayerMask traps;
    public HeroController hc;
    public Collider2D defCol;

    private void Awake()
    {
        defCol = this.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //如果敌人/武器进入到触发器，就调用受伤
        if(other.gameObject.layer == 9)
        {
            hc.TakeDamage(other.transform.position.x);
        }
    }
}
