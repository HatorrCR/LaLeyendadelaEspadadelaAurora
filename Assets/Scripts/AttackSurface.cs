using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSurface : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if(collision.name == "FlyEnemy")
            {
                collision.GetComponent<FlyEnemy>().GetDamage();
            }
            else if (collision.name == "RangedEnemy")
            {
                collision.GetComponent<RangedEnemy>().GetDamage();
            }
            else if(collision.name == "StaticEnemy")
            {
                collision.GetComponent<WayPoints>().GetDamage();
            }
        }
        else if (collision.CompareTag ("Destructible"))
        {
            collision.GetComponent<Animator>().SetBool("Destroy", true);
        }
    }
}
