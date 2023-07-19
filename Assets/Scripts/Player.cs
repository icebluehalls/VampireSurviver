using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerPosition
{
    MainHall,
    BigHall,
    LeftClass
}

public class Player : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    private Vector2 movement;
    private int hp = 3;
    private int spd = 1;
    
    public PlayerPosition playerPosition { get; private set; }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        // Input을 받습니다
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        // Normalize the movement vector
        if(movement != Vector2.zero) 
        {
            movement.Normalize();
        }
    }

    private void FixedUpdate()
    {
        // Rigidbody에 움직임을 적용합니다
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("MainHall"))
        {
            playerPosition = PlayerPosition.MainHall;
        }
        else if (other.transform.CompareTag("MainHall"))
        {
            playerPosition = PlayerPosition.BigHall;
        }
        else if (other.transform.CompareTag("LeftClass"))
        {
            playerPosition = PlayerPosition.LeftClass;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // 적과 플레이어 사이의 방향을 계산합니다.
            Vector2 bounceDirection = transform.position - other.transform.position;

            // 정규화하여 방향 벡터만을 얻습니다.
            bounceDirection = bounceDirection.normalized;

            // 플레이어를 반대방향으로 튕겨냅니다.
            rb.AddForce(bounceDirection * 1000.0f, ForceMode2D.Force);
        }
    }
}
