using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Sword : MonoBehaviour
{
    public GameObject shield;
    public float shieldDistance = 2.0f;  // How far the shield is from the player
    public WaitForFixedUpdate waitForFixedUpdate;
    public WaitForSeconds waitForCoolTime;
    public float damage = 1;
    public float coolTime = 2.0f;
    public float nuckback = 100;
    private bool isAttack;

    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;
    // Update is called once per frame
    private void Start()
    {
        isAttack = false;
        waitForFixedUpdate = new WaitForFixedUpdate();
        waitForCoolTime = new WaitForSeconds(coolTime);
        _boxCollider2D = shield.GetComponent<BoxCollider2D>();
        _boxCollider2D.enabled = false;

        _spriteRenderer = shield.GetComponent<SpriteRenderer>();
        _spriteRenderer.color = new Color(0.4f, 0.4f, 0.4f, 0.4f);

    }

    void Update()
    {
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
        waitForCoolTime = new WaitForSeconds(coolTime);
    }

    public void MultipleSize(float multiple)
    {
        shield.transform.localScale *= multiple;
        shieldDistance *= multiple;
    }
    IEnumerator SwingAttack()
    {
        isAttack = true;
        _boxCollider2D.enabled = true;
        _spriteRenderer.color = new Color(0.4f, 0.4f, 0.4f, 0.5f);
        yield return waitForFixedUpdate;
        
        _boxCollider2D.enabled = false;

        yield return waitForCoolTime;
        isAttack = false;
        _spriteRenderer.color = new Color(0.4f, 0.4f, 0.4f, 0.2f);

    }
    
    
    
    
}