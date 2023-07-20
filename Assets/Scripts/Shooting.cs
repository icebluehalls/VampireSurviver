using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab;  // 총알 프리팹을 여기에 설정합니다.
    public float bulletSpeed = 20f;  // 총알의 속도

    public float damage = 1;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // 마우스 포인터의 위치를 가져옵니다.
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 발사할 방향을 계산합니다.
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        // 총알을 생성하고 발사합니다.
        GameObject bullet = Instantiate( bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Shoot(damage, bulletSpeed, direction);
    }
}