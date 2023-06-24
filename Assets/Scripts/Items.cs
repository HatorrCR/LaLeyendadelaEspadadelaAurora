using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
<<<<<<< HEAD
    public AudioClip coinSound;
    public Animator animator;
=======
>>>>>>> 76e78fa39e3393cae13d4d28c0f8f7ab72dc404d
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AsignItem();
<<<<<<< HEAD
            
=======
>>>>>>> 76e78fa39e3393cae13d4d28c0f8f7ab72dc404d
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
<<<<<<< HEAD
        AudioSource.PlayClipAtPoint(coinSound, transform.position);

        animator.SetBool("Destroy", true);
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
=======
        Destroy(gameObject);
>>>>>>> 76e78fa39e3393cae13d4d28c0f8f7ab72dc404d
    }
}
