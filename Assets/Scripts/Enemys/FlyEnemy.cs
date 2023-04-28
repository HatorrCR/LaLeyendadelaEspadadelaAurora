using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class FlyEnemy : MonoBehaviour
{
    private CinemachineVirtualCamera cm;
    private SpriteRenderer sp;
    private PlayerController player;
    private Rigidbody2D rb;
    private bool recoil;

    
    public float velocity = 3;
    public float detectionrange = 15;
    public LayerMask layerPlayer;

    public Vector2 headposition;

    public float impact = 100;
    public int lives = 3;
    public string nombre;

    private void Awake()
    {
      cm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
      sp = GetComponent<SpriteRenderer>();
      rb= GetComponent<Rigidbody2D>();
      player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Start()
    {
        gameObject.name = name;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionrange);
        Gizmos.color=Color.green;
        Gizmos.DrawCube((Vector2)transform.position + headposition, new Vector2(1, 0.5f) * 0.7f);
    }

    void Update()
    {
        Vector2 direccion = player.transform.position - transform.position;
        float distancia = Vector2.Distance(transform.position, player.transform.position);
        if(distancia <= detectionrange)
        {
            rb.velocity = direccion.normalized * velocity;
            ChangeView(direccion.normalized.x);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void ChangeView(float direcctionX)
    {
        if (direcctionX > 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (direcctionX > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(-transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (transform.position.y + headposition.y < player.transform.position.y - 0.7f)
            {
                player.GetComponent<Rigidbody2D>().velocity = Vector2.up * player.strongjump;
                StartCoroutine(ShakeCamera(0.1f));
                Destroy(gameObject, 0.2f);
            }
            else
            {
                player.GetDamage((transform.position - player.transform.position).normalized);
            }
        }
    }

    private void FixedUpdate()
    {
        if (recoil)
        {
            rb.AddForce((transform.position - player.transform.position).normalized * impact, ForceMode2D.Impulse);
            recoil = false;
        }
    }

    public void GetDamage()
    {
        StartCoroutine(ShakeCamera(0.1f));
        if (lives > 0)
        {
            StartCoroutine(DamageEffect());
            recoil = true;
            lives--;
        }
        else
        {
            Destroy(gameObject, 0.2f);
        }
    }
    private IEnumerator ShakeCamera (float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultichannelPerlin = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultichannelPerlin.m_AmplitudeGain = 5;
        yield return new WaitForSeconds(time);
        cinemachineBasicMultichannelPerlin.m_AmplitudeGain = 0;
    }

    private IEnumerator DamageEffect()
    {
        sp.color = Color.red;
        yield return new WaitForSeconds (0.2f);
        sp.color = Color.white;
    }
}
