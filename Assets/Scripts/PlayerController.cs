using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
    //Seccion de variables y parametros
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 direccion;
    private Vector2 respawnPoint;
    private CinemachineVirtualCamera cm;
    private Vector2 movementdirection;
    private Vector2 damagedirection;
    private bool block;
    private GreyCamera gc;
    private SpriteRenderer sprite;

    [Header("Stadistics")]
    public float movementSpeed = 7;
    public float strongjump = 10;
    public float gravityScale = 3;
    public float slideVel;
    public int lives = 3;
    public float InmortalTime;
    public float recoil;

    [Header("Dash")]
    public float dashVelocity = 15;
    public bool canDash;
    public bool onDash = false;

    [Header("Colisions")]
    public Vector2 abajo;
    public Vector2 derecha;
    public Vector2 izquierda;
    public float rColision;
    public LayerMask layerFloor;


    [Header("Booleans")]
    public bool canMove = true;
    public bool onFloor = true;
    public bool touchFloor;
    public bool onClimb;
    public bool onAttack;
    public bool onShake;
    public bool objetoRecogido = false;
    public bool onWall;
    public bool rightWall;
    public bool leftWall;
    public bool hangOn;
    public bool wallJump;
    public bool Inmortal;
    public bool applystrong;

    private void Awake() //Obtencion del Rigibody
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawnPoint = transform.position; //Seteamos el respawn
        cm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        gc = Camera.main.GetComponent<GreyCamera>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetBlockTrue()
    {
        block = true;
    }

    public void Death()
    {
        if (lives > 0)
            return;
        GameManager.instance.GameOver();
        this.enabled = false;
    }

    public void GetDamage()
    {
        StartCoroutine(GetImpact(Vector2.zero));
    }

    public void GetDamage(Vector2 damagedirection)
    {
        StartCoroutine(GetImpact(damagedirection));
    }

    private IEnumerator GetImpact(Vector2 damagedirection)
    {
        if (!Inmortal)
        {
            StartCoroutine(Inmortality());
            lives--;
            gc.enabled = true;
            float velAux = movementSpeed;
            this.damagedirection = damagedirection;
            applystrong = true;
            Time.timeScale = 0.4f;
            FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
            StartCoroutine(ShakeCamera());
            yield return new WaitForSeconds(0.2f);
            Time.timeScale = 1;
            gc.enabled = false;

            for(int i =  GameManager.instance.vidasUI.transform.childCount -1; i >= 0; i--)
            {
            {
                if (GameManager.instance.vidasUI.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    GameManager.instance.vidasUI.transform.GetChild(i).gameObject.SetActive(false);
                    break;
                }
            }
            movementSpeed = velAux;
            Death();
            }
        }
    }

    private void FixedUpdate()
    {
        if (applystrong)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(-damagedirection * recoil, ForceMode2D.Impulse);
            applystrong = false;
        }
    }
    
    public void GetInmortality()
    {
        StartCoroutine(Inmortality());
    }

    private IEnumerator Inmortality()
    {
        Inmortal = true;
        float timelapse = 0;
        while (timelapse < InmortalTime)
        {
            sprite.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(InmortalTime / 20);
            sprite.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(InmortalTime / 20);
            timelapse += InmortalTime / 10;
        }
        Inmortal = false;
    }


    void Start()
    {

    }

    void Update() //Cambios por actualizacion
    {
        Movement();
        Grip();
    }

    private IEnumerator ShakeCamera() //Rutina para el movimiento de camara
    {
        onShake = true;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultichannelPerlin = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultichannelPerlin.m_AmplitudeGain = 5;
        yield return new WaitForSeconds(0.3f);
        cinemachineBasicMultichannelPerlin.m_AmplitudeGain = 0;
        onShake = false;
    }
    private IEnumerator ShakeCamera(float time) //Polimorfismo para variar el tiempo del shake
    {
        onShake = true;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultichannelPerlin = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultichannelPerlin.m_AmplitudeGain = 5;
        yield return new WaitForSeconds(time);
        cinemachineBasicMultichannelPerlin.m_AmplitudeGain = 0;
        onShake = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) //Metodo de los trigger
    {
        if(collision.tag == "Respawn")
        {
            transform.position = respawnPoint;
        }
    }


    private void Attack(Vector2 direccion) //Ataque segun la direcciæon del golpe
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!onAttack && !onDash)
            {
                onAttack = true;
                anim.SetFloat("AttackX", direccion.x);
                anim.SetFloat("AttackY", direccion.y);
                anim.SetBool("Attack", true);
            }
        }
    }

    public void FinishAttack()
    {
        anim.SetBool("Attack", false);
        block = false;
        onAttack = false;
    }

    private Vector2 attackdirection (Vector2 movementdirection, Vector2 direction)
    {
        if (rb.velocity.x == 0 && direction.y != 0)
            return new Vector2(0, direction.y);
        return new Vector2(movementdirection.x, direction.y);
    }

    private void Dash(float x, float y) //Dash
    {
        anim.SetBool("Dash", true);
        Vector3 playerpos = Camera.main.WorldToViewportPoint(transform.position);
        Camera.main.GetComponent<RippleEffect>().Emit(playerpos); ;
        canDash = true;
        rb.velocity = Vector2.zero; 
        rb.velocity += new Vector2(x, y).normalized * dashVelocity;
        StartCoroutine(PrepDash());
        
    }

    private IEnumerator PrepDash()
    {
        StartCoroutine(FloorDash());
        rb.gravityScale = 0;
        onDash = true;
        yield return new WaitForSeconds(0.3f);
        rb.gravityScale = 3;
        onDash = false;
        FinishDash();
    }

    private IEnumerator FloorDash() 
    {
        yield return new WaitForSeconds(0.15f);
        if(onFloor)
            canDash = false;
    }

    public void FinishDash()
    {
        anim.SetBool("Dash", false);
    }

    private void TouchFloor() //Si esta tocando el suelo
    {
        canDash = false;
        onDash = false;
        anim.SetBool("Jump", false);
    }

    private void Movement() //Obtencion Movimientos
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        direccion = new Vector2(x, y);
        Vector2 directionRaw = new Vector2(xRaw, yRaw);
        Walk(direccion);
        Attack(attackdirection(movementdirection,directionRaw));

        if(onFloor && !onDash)
        {
            wallJump = false;
        }

        hangOn = onWall && Input.GetKey(KeyCode.LeftShift);

        if (hangOn && !onFloor)
        {
            anim.SetBool("Climb", true);
            if (rb.velocity == Vector2.zero)
            {
                anim.SetFloat("Velocity", 0);
            }
            else
            {
                anim.SetFloat("Velocity", 1);
            }
        }
        else
        {
            anim.SetBool("Climb", false);
            anim.SetFloat("Velocity", 0);
        }

        if(hangOn && !onDash)
        {
            rb.gravityScale = 0;
            if(x > 0.2f || x < 0.2f)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            float velocityMod = y > 0 ? 0.5f : 1;
            rb.velocity = new Vector2(rb.velocity.x, y * (movementSpeed * velocityMod));
            if(leftWall && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (rightWall && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            rb.gravityScale = 3;
        }

        if (onWall && !onFloor)
        {
            anim.SetBool("Climb", true);
            if (x != 0 && !hangOn)
                SlideWall();
        }

        ImproveJump();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onFloor)
            {
                anim.SetBool("Jump", true);
                Jump();
            }
            if(onWall && !onFloor)
            {
                anim.SetBool("Climb", false);
                anim.SetBool("Jump", true);
                WallJump();
            }
        }

        float velocity; // Variable del blendtree
        if (rb.velocity.y > 0)
            velocity = 1;
        else
            velocity = -1;

        if (!onFloor)  // Uso de blendtree
        {
            anim.SetFloat("VerticalVelocity", velocity); 
        }
        else
        {   
            if(velocity == -1)
            FinishJump();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && !onDash && !canDash)
        {
            if (xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw);
        }
        if(onFloor && !touchFloor)
        {
            anim.SetBool("Climb", false);
            TouchFloor();
            touchFloor = true;
        }
        if (!onFloor && touchFloor)
            touchFloor = false;
    }

    private void SlideWall()
    {
        if(canMove)
            rb.velocity = new Vector2(rb.velocity.x, -slideVel);
    }

    private void WallJump()
    {
        StopCoroutine(DeshabMov(0));
        StartCoroutine(DeshabMov(0.1f));
        Vector2 wallDirection = rightWall ? Vector2.left : Vector2.right;
        if (wallDirection.x < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (wallDirection.x > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        anim.SetBool("Jump", true);
        anim.SetBool("Climb", false);
        Jump((Vector2.up + wallDirection), true);
        wallJump = true;
    }

    private IEnumerator DeshabMov(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true; 
    }

    public void FinishJump() //Metodo del evento de la animacion de salto
    {
        anim.SetBool("Jump", false);
    }

    private void ImproveJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime; 
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (3.0f - 1) * Time.deltaTime;
        }
    } //Propiedades del salto

    private void Grip() //Trepar
    {
        onFloor = Physics2D.OverlapCircle((Vector2)transform.position + abajo, rColision, layerFloor);
        Collider2D collisionR = Physics2D.OverlapCircle((Vector2)transform.position + derecha, rColision, layerFloor);
        Collider2D collisionL = Physics2D.OverlapCircle((Vector2)transform.position + izquierda, rColision, layerFloor);
        if(collisionR != null)
        {
            onWall = !collisionR.CompareTag("Platform");
        }
        else if(collisionL != null)
        {
            onWall = !collisionL.CompareTag("Platform");
        }
        else
        {
            onWall= false;
        }

        rightWall = Physics2D.OverlapCircle((Vector2)transform.position + derecha, rColision, layerFloor);
        leftWall = Physics2D.OverlapCircle((Vector2)transform.position + izquierda, rColision, layerFloor);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * strongjump;
    }
    private void Jump(Vector2 wallDirection, bool wall)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += wallDirection * strongjump;
    }

    private void Walk(Vector2 direccion) //Caminar obtiene movimiento
    {
        if (canMove && !onDash && !onAttack)
        {
            if(wallJump)
            {
                rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(direccion.x * movementSpeed, rb.velocity.y)), Time.deltaTime / 2);
            }
            else
            {
                if (direccion != Vector2.zero && !hangOn)
                {
                    if (!onFloor)
                    {
                        anim.SetBool("Jump", true);
                    }
                    else
                    {
                        anim.SetBool("Walk", true);
                    }

                    rb.velocity = (new Vector2(direccion.x * movementSpeed, rb.velocity.y));
                    if (direccion.x < 0 && transform.localScale.x > 0)
                    {
                        movementdirection = attackdirection(Vector2.left, direccion);
                        ; transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    }
                    else if (direccion.x > 0 && transform.localScale.x < 0)
                    {
                        movementdirection = attackdirection(Vector2.right, direccion);
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }
                }
                else
                {
                    if (direccion.y > 0 && direccion.x == 0)
                    {
                        movementdirection = attackdirection(direccion, Vector2.up);
                    }
                    anim.SetBool("Walk", false); 
                }
            }

        }
        else
        {
            if (block)
            {
                FinishAttack();
            }
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ObjetoRecogible"))
        {
            objetoRecogido = true;
        }

    }
}