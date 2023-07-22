using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItem : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 10);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.player.HpUp();
            Destroy(gameObject);
        }
    }
}
