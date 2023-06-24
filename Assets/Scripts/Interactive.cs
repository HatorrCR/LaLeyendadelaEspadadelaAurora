using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactive : MonoBehaviour
{
    private BoxCollider2D bc;
    private SpriteRenderer sp;
    private GameObject indInteract;
    private Animator anim;
    public Animator animator;

    public UnityEvent evento;

    public GameObject[] objets;

    private bool canInteract;
    public bool isChest;
    public bool islever;
    public bool leveractivated;
    public bool isCheckPoint;
    public bool isSelector;

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (transform.GetChild(0) != null)
            indInteract = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;
            indInteract.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            if (collision.CompareTag("Player"))
            {
                canInteract = false;
                indInteract.SetActive(false);
            }
    }
    
    private void Chest()
    {
        if (isChest)
        {
            Instantiate(objets[Random.Range(0, objets.Length)], transform.position, Quaternion.identity);
            anim.SetBool("Open", true);
            bc.enabled = false;
        }
    }

    private void Lever()
    {
        if (islever && !leveractivated)
        {
            anim.SetBool("Active", true);
            leveractivated = true;
            evento.Invoke();
            indInteract.SetActive(false);
            bc.enabled = false;
            this.enabled = false;
        }
    }

    private void CheckPoint()
    {
        if (isCheckPoint)
        {
            evento.Invoke();
            animator.SetBool("Active", true);
        }
    }

    private void LevelSelect()
    {
        if (isSelector)
        {
            evento.Invoke();
        }
    }

    private void Update()
    {
        if(canInteract && Input.GetKeyDown(KeyCode.C))
        {
            Chest();
            Lever();
            CheckPoint();
            LevelSelect();
        }
    }
}
