using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Camera : MonoBehaviour
{
    //Variables

    public GameObject Player;
    public Vector2 min; //Encuadre minimo
    public Vector2 max; //Encuadre maximo
    public float soft; //Suavizado al parar
    Vector2 velocity;

    void FixedUpdate()
    {
        float posx = Mathf.SmoothDamp(transform.position.x, Player.transform.position.x, ref velocity.x, soft); //Posiciones X
        float posy = Mathf.SmoothDamp(transform.position.y, Player.transform.position.y, ref velocity.y, soft); //Posiciones y

        transform.position = new Vector3(Mathf.Clamp(posx, min.x, max.x), Mathf.Clamp(posy, min.y, max.y), transform.position.z); //Valor a la camara

    }
}
