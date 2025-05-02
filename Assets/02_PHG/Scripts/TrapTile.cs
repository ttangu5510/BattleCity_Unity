using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : MonoBehaviour
{
    [SerializeField] private GameObject triggeredTrapPrefab;
    [SerializeField] private float damageDelay = 0.01f;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float pushForce = 45f;
    [SerializeField] private GameObject explosionEffectPrefab;
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;
        if (other.GetComponentInParent<IDamagable>() == null)
            return; // 데미지 대상 아니면 무시
        isTriggered = true;
        Invoke(nameof(CheckAndDamage), damageDelay);
    }

    private void CheckAndDamage()
    {
        // 1. 먼저 데미지 검사
        Collider[] hits = Physics.OverlapSphere(transform.position + Vector3.up * 5f, 5f);
        foreach (var hit in hits)
        {
            IDamagable dmg = hit.GetComponentInParent<IDamagable>();
            if (dmg != null)
            {
                dmg.TakeDamage();
                Instantiate(explosionEffectPrefab, transform.position + Vector3.down * 0.5f, Quaternion.identity);
                Debug.Log($"[Trap] {hit.name}에게 데미지 부여됨!");
                PushForward(hit.gameObject);
                
            }
        }
        Instantiate(triggeredTrapPrefab, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject, 0.05f);

    }
    private void PushForward(GameObject target)
    {
        Rigidbody rb = target.GetComponentInParent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("대상에 Rigidbody가 없습니다. 밀 수 없습니다.");
            return;
        }

        Vector3 trapPos = transform.position;
        Vector3 targetPos = target.transform.position;
        Vector3 toTarget = (targetPos - trapPos).normalized;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float dotF = Vector3.Dot(forward, toTarget);
        float dotR = Vector3.Dot(right, toTarget);

        Vector3 primaryDir = Vector3.zero;
        Vector3 backupDir = Vector3.zero;

        if (Mathf.Abs(dotF) > Mathf.Abs(dotR))
        {
            primaryDir = dotF >= 0 ? forward : -forward;
            backupDir = dotF >= 0 ? -forward : forward;
        }
        else
        {
            primaryDir = dotR >= 0 ? right : -right;
            backupDir = dotR >= 0 ? -right : right;
        }

        float checkDistance = 1f;
        Vector3 rayStart = target.transform.position + Vector3.up * 0.2f;

        bool isBlocked = Physics.Raycast(rayStart, primaryDir, checkDistance, obstacleMask);
        Vector3 pushDir = isBlocked ? backupDir : primaryDir;

        rb.velocity = Vector3.zero;
        rb.AddForce(pushDir * pushForce, ForceMode.VelocityChange);

        Debug.Log($"[Trap] {target.name} {(isBlocked ? "우회" : "직진")} 밀림 → 방향: {pushDir}");
    }


}
