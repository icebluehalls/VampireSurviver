using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Normal,
    Reflect
}

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;  // 총알 프리팹을 여기에 설정합니다.
    [SerializeField] private GameObject normalBullet;
    [SerializeField] private GameObject reflectBullet;
    
    public BulletType bulletType;
    public float bulletSpeed = 20f;  // 총알의 속도
    public float fireDelay = 0.25f;  // 총알의 연사 속도
    public float fireDelayLeft = 0f; // 다음 총알 발사 시간을 관리합니다.
    public float damage = 1;
    public float knockback = 1;
    public int enemyTouchCount = 1;
    
    // Update is called once per frame
    void Update()
    {
        fireDelayLeft += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && fireDelayLeft >= fireDelay)
        {
            fireDelayLeft = 0;
            Shoot();
        }
        
        
    }
    
    public void ChangeBullet(BulletType bulletType)
    {
        this.bulletType = bulletType;
        switch (bulletType)
        {
            case BulletType.Normal:
                bulletPrefab = bulletPrefab;
                break;
            case BulletType.Reflect:
                bulletPrefab = reflectBullet;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(bulletType), bulletType, null);
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
        bullet.GetComponent<Bullet>().Shoot(damage, bulletSpeed, knockback, enemyTouchCount, direction);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}