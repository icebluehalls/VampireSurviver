using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    protected Rigidbody2D rigidbody2D;

    public void Shoot(float damage, float bulletSpeed, Vector2 direction)
    {
        this.damage = damage;
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = direction * bulletSpeed;
        Destroy(gameObject, 5);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.transform.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.transform.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
