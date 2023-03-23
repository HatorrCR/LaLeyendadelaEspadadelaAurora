using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Seccion de variables y parametros
    private Rigidbody2D rb; 
    private Vector2 direccion;

    [Header("Stadistics")]
    public float movementSpeed = 11;
    public float strongjump = 15;
    public float gravityScale = 3;

    [Header("Dash")]
    public float dashVelocity = 16;
    public float dashDuration = 0.2f;
    private float dashCooldown = 0.3f;
    private float dashCooldownT = 0.0f;
    public bool canDash;
    public bool onDash = false;
    public bool runDash;

    [Header("Colisions")]
    public Vector2 abajo;
    public float rColision;
    public LayerMask layerFloor;

    [Header("Booleans")]
    public bool canMove = true;
    public bool onFloor = true;
    public bool touchFloor;
    public bool onClimb;


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

        if (dashCooldownT > 0.0f)
        {
            dashCooldownT -= Time.deltaTime;
        }//Actualizar el cooldown del dash
    }

    private void Dash(float x, float y) //Dash
    {
        if (!onDash && dashCooldownT <= 0.0f) //Si no esta en dash
        {
            onDash = true;
            canDash = true; //Si es posible
            rb.velocity = Vector2.zero; //Lo iniciamos en 0
            rb.velocity += new Vector2(x, y).normalized * dashVelocity; //Normalizamos a 1 y multiplicamos el dash
            StartCoroutine(PrepDash()); //Llamamos corrutina
        }
    }

    private IEnumerator PrepDash() //Corrutina
    {
        StartCoroutine(FloorDash());
        rb.gravityScale = 3; //Gravedad en 0 para que no afecte
        runDash = true; //Hace el dash

        yield return new WaitForSeconds(dashDuration); //Tiempo de espera tras el dash
        rb.gravityScale = 3; //Gravedad tras acabas el dash
        runDash = false; //Acaba el dash
        onDash = false;
        dashCooldownT = dashCooldown;
    }

    private IEnumerator FloorDash() //Corrutina
    {
        yield return new WaitForSeconds(dashDuration); //Tiempo de espera
        if(onFloor) //Si en el suelo 
            canDash = false;
    }

    private void TouchFloor() //Si esta tocando el suelo
    {
        canDash = false;
        runDash = false;
    }

    private void Movement() //Obtencion Movimientos
    {
        float x = Input.GetAxis("Horizontal"); //Cambio en x
        float y = Input.GetAxis("Vertical"); //Cambio en y

        float xRaw = Input.GetAxisRaw("Horizontal"); //Cambio en x absoluto
        float yRaw = Input.GetAxisRaw("Vertical"); //Cambio en y absoluto

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
        if (Input.GetKeyDown(KeyCode.Mouse1) && !runDash) //Con el clik dash si tiene alguna direccion en raw solo
        {
            if (xRaw != 0 || yRaw != 0)
                Dash(xRaw, 0);
        }
        if(onFloor && !touchFloor)  //Comprobar el contacto con el suelo
        {
            TouchFloor();
            touchFloor = true;
        }
        if (!onFloor && touchFloor) //Comprobar que no este en suelo
            touchFloor = false;
    }

    private void ImproveJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime; //Velocidad de bajada
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (3.0f - 1) * Time.deltaTime; //Subida
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
        if (canMove && !runDash)
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
