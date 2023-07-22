using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage { get; private set; }
    public float knockback { get; private set; }
    protected Rigidbody2D rigidbody2D;
    public float enemyTouchCount { get; private set; } = 1;
    public void Shoot(float damage, float bulletSpeed, float knockback, float enemyTouchCount, Vector2 direction)
    {
        this.damage = damage;
        this.knockback = knockback;
        this.enemyTouchCount = enemyTouchCount;
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = direction * bulletSpeed;
        rigidbody2D.freezeRotation = true;
        Destroy(gameObject, 5);
    }

    public void DownEnemyTouchCount(int count)
    {
        enemyTouchCount -= count;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
