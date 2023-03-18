using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb; //Aøadimos Rigidbody
    private Vector2 direccion;

    [Header("Stadistics")]
    public float movementSpeed = 10;
    public float strongjump = 5;

    [Header("Colisions")]
    public Vector2 abajo;
    public float rColision;
    public LayerMask layerFloor;

    [Header("Booleans")]
    public bool canMove = true;
    public bool onFloor = true;

    private void Awake() //Obtencion del Rigibody
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    void Update() //Cambios por actualizacion
    {
        Movement();
        Grip();
    }

    private void Movement() //Obtencion Movimientos
    {
        float x = Input.GetAxis("Horizontal"); //Cambio en x
        float y = Input.GetAxis("Vertical"); //Cambio en y

        direccion = new Vector2(x, y); //Direccion en 2d con vector2
        Walk(direccion);


        ImproveJump();
        if (Input.GetKeyDown(KeyCode.Space)) //Con el espacio se salta
        {
            if (onFloor)
            {
            Jump();
            }
        }

    }

    private void ImproveJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime; //Velocidad de bajada
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (2.0f - 1) * Time.deltaTime; //Subida
        }
    } //Propiedades del salto

    private void Grip() //Si esta en suelo
    {
        onFloor = Physics2D.OverlapCircle((Vector2)transform.position + abajo, rColision, layerFloor ); //Radio de colision que detecta el suelo
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * strongjump;
    } //Calculos del salto


    private void Walk(Vector2 direccion) //Caminar obtiene movimiento
    {
        if (canMove)
        {
            rb.velocity = new Vector2(direccion.x * movementSpeed, rb.velocity.y); //Solo en eje x para andar
            
            if(direccion != Vector2.zero) //Si la direccion es diferente de 0
            {
                if (direccion.x <0 && transform.localScale.x > 0) //Comprobar si izquierda
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else if(direccion.x > 0 && transform.localScale.x < 0) //Comprobar si derecha
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

                }
            }
        }
    }
}
