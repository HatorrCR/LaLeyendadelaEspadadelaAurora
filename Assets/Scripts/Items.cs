using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public AudioClip coinSound;
    public Animator animator;
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
        AudioSource.PlayClipAtPoint(coinSound, transform.position);

        animator.SetBool("Destroy", true);
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
