using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AsignItem();
        }
    }

    private void AsignItem()
    {
        if (gameObject.CompareTag("Coin"))
        {
            GameManager.instance.ActualCoin();            
        }
        else if(gameObject.CompareTag("PowerUp"))
        {
            GameManager.instance.player.GetInmortality();
        }
        Destroy(gameObject);
    }
}
