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
    // Update is called once per frame
    private void Start()
    {
        shield.SetActive(false);
        isAttack = false;
        waitForFixedUpdate = new WaitForFixedUpdate();
        waitForCoolTime = new WaitForSeconds(coolTime);
    }

    void Update()
    {
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

    public void DamageUp(float damage)
    {
        this.damage += damage;
    }
    

    IEnumerator SwingAttack()
    {
        isAttack = true;
        shield.SetActive(true);
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        shield.transform.position = (Vector2)transform.position + direction * shieldDistance;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shield.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        yield return waitForFixedUpdate;
        
        shield.SetActive(false);

        yield return waitForCoolTime;
        isAttack = false;
    }
    
    
    
    
}