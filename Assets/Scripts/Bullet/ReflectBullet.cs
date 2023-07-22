using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectBullet : Bullet
{
    Vector3 _lastVelocity;
    void Update()
    {
        _lastVelocity = rigidbody2D.velocity;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall")) // "Wall" 태그가 있는 게임오브젝트와 충돌했는지 확인
        {
            rigidbody2D.freezeRotation = false;
            // 충돌 접점에서의 법선 벡터 가져오기
            float speed = _lastVelocity.magnitude; // 충돌 전 총알의 속도 저장

            Vector2 reflectVec = Vector2.Reflect(_lastVelocity.normalized, other.contacts[0].normal);
            rigidbody2D.velocity = reflectVec * Mathf.Max(speed, 0f);

            // 총알의 회전을 반사각에 맞게 변경
            float angle = Mathf.Atan2(reflectVec.y, reflectVec.x) * Mathf.Rad2Deg;
            rigidbody2D.rotation = angle;
            StartCoroutine(RoTate());
        }
    }

    IEnumerator RoTate()
    {
        yield return new WaitForFixedUpdate();
        rigidbody2D.freezeRotation = true;
    }
}
