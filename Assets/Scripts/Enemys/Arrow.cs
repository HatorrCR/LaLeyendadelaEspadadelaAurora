using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    public LayerMask layerFloor;
    public GameObject RangedEnemy;
    public Vector2 directionarrow;
    public float rcolision = 0.25f;
    public bool touchFloor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().GetDamage(-(collision.transform.position - RangedEnemy.transform.position).normalized);
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        touchFloor = Physics2D.OverlapCircle((Vector2)transform.position, rcolision, layerFloor);
        if (touchFloor)
        {
            rb.bodyType = RigidbodyType2D.Static;
            bc.enabled = false;
            this.enabled = false;
        }

        float angulo = Mathf.Atan2(directionarrow.y, directionarrow.x) * Mathf.Rad2Deg;

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.y, transform.localEulerAngles.x, angulo);
    }
}
