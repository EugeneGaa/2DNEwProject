using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public LayerMask enemy;
    public HeroController hc;
    public Collider2D defCol;

    private void Awake()
    {
        defCol = this.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.layer);
        if(other.gameObject.layer == 9)
        {
            hc.TakeDamage(other.transform.position.x);
        }
    }
}
