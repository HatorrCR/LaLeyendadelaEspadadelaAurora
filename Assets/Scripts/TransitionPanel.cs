using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPanel : MonoBehaviour
{
    public void AppearGame()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Appear");
    }
    public void DefaultTransition()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Default");
    }
}
