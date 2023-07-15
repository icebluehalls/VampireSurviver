using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerTransform;  // 플레이어의 위치
    // Update is called once per frame
    void Update()
    {
        // 카메라의 위치를 플레이어의 위치로 설정
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }
}