using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(CheckAnimationEnd());
    }

    IEnumerator CheckAnimationEnd()
    {
        // 애니메이션 클립의 길이만큼 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 애니메이션이 끝나면 게임 오브젝트 삭제
        Destroy(gameObject);
    }
}
