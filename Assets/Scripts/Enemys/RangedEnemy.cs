using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class RangedEnemy : MonoBehaviour
{
    private PlayerController player;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private Animator anim;
    private CinemachineVirtualCamera cm;
    private bool recoil;

    public float detectdistance = 17;
    public float detectattack = 11;
    public GameObject arrow;
    public float strattack = 5;
    public float velmov;
    public int lives = 3;
    public bool onArrow;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        cm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }

    void Start()
    {
        gameObject.name = "RangedEnemy";
    }

    void Update()
    {
        Vector2 direccion = (player.transform.position - transform.position).normalized * detectdistance;
        Debug.DrawRay(transform.position, direccion, Color.red);
        float distanciaActual = Vector2.Distance(transform.position, player.transform.position);
        if(distanciaActual <= detectattack)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("Walk", false);
            Vector2 direccionNormalizada = direccion.normalized;
            ChangeView(direccionNormalizada.x);
            if (!onArrow)
                StartCoroutine(TheArrow(direccion, distanciaActual));
        }
        else 
        {
            if(distanciaActual <= detectdistance)
            {
                Vector2 movimiento = new Vector2(direccion.x, 0);
                movimiento = movimiento.normalized;
<<<<<<< HEAD
                rb.velocity = new Vector2(movimiento.x * velmov, rb.velocity.y);
=======
                rb.velocity = movimiento * velmov;
>>>>>>> 76e78fa39e3393cae13d4d28c0f8f7ab72dc404d
                anim.SetBool("Walk", true);
                ChangeView(movimiento.x);
            }
            else
            {
                anim.SetBool("Walk", false);
            }
        }
    }
    private void ChangeView(float direccionX)
    {
        if(direccionX < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if(direccionX > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectdistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectattack);
    }

    private IEnumerator TheArrow(Vector2 directionarrow, float distancia)
    {
        onArrow = true;
        anim.SetBool("Shooting", true);
        yield return new WaitForSeconds(1.42f);
        anim.SetBool("Shooting", false);
<<<<<<< HEAD
        directionarrow = (player.transform.position - transform.position).normalized * detectdistance;
=======
>>>>>>> 76e78fa39e3393cae13d4d28c0f8f7ab72dc404d
        directionarrow = directionarrow.normalized;

        GameObject arrowGO = Instantiate(arrow, transform.position, Quaternion.identity);
        arrowGO.transform.GetComponent<Arrow>().directionarrow = directionarrow;
        arrowGO.transform.GetComponent<Arrow>().RangedEnemy = this.gameObject;

        arrowGO.transform.GetComponent<Rigidbody2D>().velocity = directionarrow * strattack;
        onArrow = false;
    }

    public void GetDamage()
    {
        if(lives > 0)
        {
            StartCoroutine(DamageEffect());
            StartCoroutine(ShakeCamera(0.1f));
            recoil = true;
            lives--;
        }
        else
        {
            StartCoroutine(ShakeCamera(0.1f));
<<<<<<< HEAD
          
        }
    }

    private void Death()
    {
        if(lives <= 0)
        {
=======
>>>>>>> 76e78fa39e3393cae13d4d28c0f8f7ab72dc404d
            velmov = 0;
            rb.velocity = Vector2.zero;
            Destroy(this.gameObject, 0.2f);
        }
    }
<<<<<<< HEAD

=======
>>>>>>> 76e78fa39e3393cae13d4d28c0f8f7ab72dc404d
    private IEnumerator ShakeCamera(float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultichannelPerlin = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultichannelPerlin.m_AmplitudeGain = 5;
        yield return new WaitForSeconds(time);
        cinemachineBasicMultichannelPerlin.m_AmplitudeGain = 0;
<<<<<<< HEAD
        Death();
=======
>>>>>>> 76e78fa39e3393cae13d4d28c0f8f7ab72dc404d
    }

    private IEnumerator DamageEffect()
    {
        sp.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sp.color = Color.white;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           
            player.GetDamage((transform.position - player.transform.position).normalized);
           
        }
    }

    private void FixedUpdate()
    {
        if (recoil)
        {
            rb.AddForce((transform.position - player.transform.position).normalized * 100, ForceMode2D.Impulse);
            recoil = false;
        }
    }
}
