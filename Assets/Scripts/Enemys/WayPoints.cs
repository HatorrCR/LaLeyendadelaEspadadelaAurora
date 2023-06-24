using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class WayPoints : MonoBehaviour
{
    private Vector3 direction;
    private PlayerController player;
    private CinemachineVirtualCamera cm;
    private Rigidbody2D rb;
    public SpriteRenderer sp;
    private int indexnow = 0;
    private bool recoil = false;

    public int lives = 3;
    public Vector2 positionhead;
    public float velocity;
    public List<Transform> puntos = new List<Transform>();
    public bool wait;
    public float waittime;
    
    private void Awake()
    {
        cm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        if (gameObject.CompareTag("Enemy"))
            gameObject.name = "StaticEnemy";
    }

    private void FixedUpdate()
    {
        WaypointsMovement();
        if (gameObject.CompareTag("Enemy"))
        {
            TransformEnemy();
        }

        if (recoil)
        {
            rb.AddForce((transform.position - player.transform.position).normalized * 10, ForceMode2D.Impulse);
            recoil = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy"))
        {
            if(player.transform.position.y - 0.7f > transform.position.y + positionhead.y)
            {
                player.GetComponent<Rigidbody2D>().velocity = Vector2.up * player.strongjump;
                Destroy(this.gameObject, 0.2f);
            }
            else
            {
                player.GetDamage(-(player.transform.position - transform.position).normalized);
            }
        }
        else if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Platform"))
        {
            if(player.transform.position.y - 0.8f > transform.position.y)
            {
                player.transform.parent = transform;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Platform"))
        {
            player.transform.parent = null;
        }
    }

    private void TransformEnemy()
    {
        if (direction.x < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void WaypointsMovement()
    {
        direction = (puntos[indexnow].position - transform.position).normalized;
        if(!wait)
            transform.position = (Vector2.MoveTowards(transform.position, puntos[indexnow].position, velocity * Time.deltaTime));
        if (Vector2.Distance(transform.position, puntos[indexnow].position) <= 0.7f)
        {
            if (!wait)
            {
                StartCoroutine(Wait());
            }
            
        }
    }

    private IEnumerator Wait()
    {
        wait = true;
        yield return new WaitForSeconds(waittime);
        wait = false;
        indexnow++;
        if (indexnow >= puntos.Count)
            indexnow = 0;
    }

    public void GetDamage()
    {
        if (lives > 0)
        {
            StartCoroutine(DamageEffect());
            StartCoroutine(ShakeCamera(0.1f));
            recoil = true;
            lives--;
        }
        else
        {
            StartCoroutine(ShakeCamera(0.1f));
        }
    }

    private void Death()
    {
        if(lives <= 0)
        {
            velocity = 0;
            rb.velocity = Vector2.zero;
            Destroy(this.gameObject, 0.2f);
        }
    }

    private IEnumerator ShakeCamera(float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultichannelPerlin = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultichannelPerlin.m_AmplitudeGain = 5;
        yield return new WaitForSeconds(time);
        cinemachineBasicMultichannelPerlin.m_AmplitudeGain = 0;
        Death();
    }

    private IEnumerator DamageEffect()
    {
        sp.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sp.color = Color.white;
    }
}
