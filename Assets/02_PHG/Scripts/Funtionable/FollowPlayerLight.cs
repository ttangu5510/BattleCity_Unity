using UnityEngine;

public class LightFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float rotateSpeed = 3f;

    private void Update()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("플레이어 Transform이 비어 있음");
            return;
        }

        Vector3 dir = playerTransform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        Debug.DrawLine(transform.position, playerTransform.position, Color.yellow); // 시각 확인
    }
}