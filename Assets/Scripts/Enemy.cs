using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f; // 적의 이동 속도
    private Transform player; // 플레이어의 위치

    // Start is called before the first frame update
    void Start()
    {
        // Player 오브젝트를 찾습니다.
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어의 방향으로 이동
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
