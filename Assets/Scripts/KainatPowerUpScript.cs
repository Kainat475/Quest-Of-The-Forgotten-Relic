using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KainatPowerUpScript : MonoBehaviour
{   
    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
