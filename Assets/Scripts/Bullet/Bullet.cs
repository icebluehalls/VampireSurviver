using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float knockback;
    protected Rigidbody2D rigidbody2D;
    public float enemyTouchCount = 1;
    public void Shoot(float damage, float bulletSpeed, float knockback, float enemyTouchCount, Vector2 direction)
    {
        this.damage = damage;
        this.knockback = knockback;
        this.enemyTouchCount = enemyTouchCount;
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = direction * bulletSpeed;
        Destroy(gameObject, 5);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
