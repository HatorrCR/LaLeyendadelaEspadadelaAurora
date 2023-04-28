using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObject : MonoBehaviour
{
    public bool objetoRecogido = false;
    public GameObject mecanicaDesbloqueada;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objetoRecogido = true;
            mecanicaDesbloqueada.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
