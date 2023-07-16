using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    private Vector2 movement;

    private int hp = 3;
    private int spd = 1;

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
}
