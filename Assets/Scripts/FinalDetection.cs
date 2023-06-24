using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDetection : MonoBehaviour
{
    public bool nextlevels;
  private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.ActiveTransitionPanel();
            GameManager.instance.nextLevel = nextlevels;
            StartCoroutine(WaitPositionChange());
            
        }
    }

    private IEnumerator WaitPositionChange()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.ChangePoss();
        if(nextlevels)
            GameManager.instance.actualLevel++;
        else
            GameManager.instance.actualLevel--;
    }

}
