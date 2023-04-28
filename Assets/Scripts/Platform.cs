using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private bool applypush;
    private bool detectplayer;
    private PlayerController player;

    public bool platjump;
    public float pjump;
    public BoxCollider2D platCollider;
    public BoxCollider2D platTrygger;



    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        if (!platjump)
            Physics2D.IgnoreCollision(platCollider, platTrygger, true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!platjump)
                Physics2D.IgnoreCollision(platCollider, player.GetComponent<CapsuleCollider2D>(), true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!platjump)
                Physics2D.IgnoreCollision(platCollider, player.GetComponent<CapsuleCollider2D>(), false);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            detectplayer = true;
            if(platjump)
                applypush = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            detectplayer = false;
        }
    }

    private void Update()
    {
        if (platjump)
        {
            if (player.transform.position.y - 0.8f > transform.position.y)
            {
                platCollider.isTrigger = false;
            }
            else
            {
                platCollider.isTrigger = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (applypush)
        {
            
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * pjump, ForceMode2D.Impulse);
            applypush = false;
        }
    }
}
