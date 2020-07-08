using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColManager : MonoBehaviour
{
    public LayerMask enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            Debug.Log("PUNIAMO");
            collision.gameObject.GetComponent<IEnemyController>().TakeDamage();
        }
    }
}
