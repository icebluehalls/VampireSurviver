using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Sword : MonoBehaviour
{
    public GameObject shield;
    public float shieldDistance = 4.0f;  // How far the shield is from the player
    private WaitForFixedUpdate waitForFixedUpdate;
    private WaitForSeconds waitForCoolTime;
    private WaitForSeconds waitForImagedTime;
    public float damage = 5;
    public float coolTime = 2.0f;
    private const float imagedTime = 0.1f;
    public float nuckback = 100;
    private bool isAttack;

    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;
    // Update is called once per frame
    void Start()
    {
        isAttack = false;
        waitForFixedUpdate = new WaitForFixedUpdate();
        waitForImagedTime = new WaitForSeconds(imagedTime);
        waitForCoolTime = new WaitForSeconds(coolTime - imagedTime);
        _boxCollider2D = shield.GetComponent<BoxCollider2D>();
        _boxCollider2D.enabled = false;

        _spriteRenderer = shield.GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

    }

    void Update()
    {
        if (isAttack)
        {
            return;
        }
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        shield.transform.position = (Vector2)transform.position + direction * shieldDistance;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shield.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        
        if (Input.GetMouseButtonDown(1) && isAttack == false)
        {
            StartCoroutine(SwingAttack());
        }
    }

    public void CoolTimeDown(float coolDown)
    {
        coolTime -= coolDown;
        waitForCoolTime = new WaitForSeconds(coolTime - imagedTime);
    }

    public void MultipleSize(float multiple)
    {
        shield.transform.localScale *= multiple;
        shieldDistance *= multiple;
    }
    IEnumerator SwingAttack()
    {
        isAttack = true;
        _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.7f);
        _boxCollider2D.enabled = true;
        yield return waitForFixedUpdate;

        _boxCollider2D.enabled = false;
        yield return waitForImagedTime;
        _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        yield return waitForCoolTime;
        isAttack = false;

    }
    
    
    
    
}