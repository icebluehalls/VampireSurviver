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
    public float speed { get; private set; }= 5f;
    public int hp { get; private set; } = 3;
    public Sword sword;
    private Rigidbody2D _rb;
    private Vector2 movement;
    private SpriteRenderer _spriteRenderer;
    private bool _isDamaged = false;
    public PlayerPosition playerPosition { get; private set; }
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        sword = GetComponent<Sword>();
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void HpUp()
    {
        if (hp >= 4)
        {
            return;
        }
        hp++;
        UIManager.Instance.UpdateHp(hp);
    }
    
    private void Update()
    {
        if (GameManager.Instance.IsGameEnd)
        {
            return;
        }
        // Input을 받습니다
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        Vector3 mousePosition = Input.mousePosition;

        // 화면의 중앙보다 왼쪽인지 오른쪽인지를 판단
        if (mousePosition.x < Screen.width / 2)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
        
        // Normalize the movement vector
        if(movement != Vector2.zero) 
        {
            movement.Normalize();
        }

        if (transform.position.x < -15 || transform.position.x > 43)
        {
            GameManager.Instance.GameEnd();
        }
        if (transform.position.y < -16 || transform.position.y > 10)
        {
            GameManager.Instance.GameEnd();
        }
    }

    private void FixedUpdate()
    {
        // Rigidbody에 움직임을 적용합니다
        _rb.MovePosition(_rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("MainHall"))
        {
            playerPosition = PlayerPosition.MainHall;
        }
        else if (other.transform.CompareTag("BigHall"))
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
        if (other.gameObject.CompareTag("Enemy") && !_isDamaged)
        {
            hp -= 1;
            if (hp <= 0)
            {
                GameManager.Instance.GameEnd();
                return;
            }
            UIManager.Instance.UpdateHp(hp);

            // 적과 플레이어 사이의 방향을 계산합니다.
            Vector2 bounceDirection = transform.position - other.transform.position;

            // 정규화하여 방향 벡터만을 얻습니다.
            bounceDirection = bounceDirection.normalized;

            // 플레이어를 반대방향으로 튕겨냅니다.
            _rb.AddForce(bounceDirection * 1000.0f, ForceMode2D.Force);
            StartCoroutine(GetDamaged());
        }
    }

    private IEnumerator GetDamaged()
    {
        _isDamaged = true;
        _spriteRenderer.color = new Color(1,1,1,0.5f);
        yield return new WaitForSeconds(3.0f);
        _isDamaged = false;
        _spriteRenderer.color = new Color(1,1,1,1);
    }
}
