using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    [SerializeField] private TileType tileType;
    [Header("Ice Settings")]
    [SerializeField] private float slideForce = 5f;

    [Header("Sand Settings")]
    [SerializeField] private float slowFactor = 0.5f;

    [Header("Magma Settings")]
    [SerializeField] private float burnDamagePerSecond = 10f;
    private Dictionary<GameObject, float> magmaDamageTimers = new Dictionary<GameObject, float>();

    private void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponentInParent<Rigidbody>();

        switch (tileType)
        {
            case TileType.Ice:
                var ctrl = other.GetComponentInParent<IceSlideController>();
                if (ctrl != null && !ctrl.IsSliding)
                {
                    ctrl.StartSliding();
                    Debug.Log("빙판 진입 즉시 미끄러짐");
                }
                break;

            case TileType.Sand:
                if (rb != null)
                    Debug.Log("플레이어감지 Sand 와 샌드다!");
                    rb.velocity *= slowFactor;
                break;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        var rb = other.GetComponentInParent<Rigidbody>();
        var ctrl = other.GetComponentInParent<IceSlideController>();

        if (rb == null) return;

        switch (tileType)
        {
            case TileType.Ice:
                if (ctrl == null) return;

                if (ctrl.IsSliding)
                {
                    rb.velocity = ctrl.lastMoveDir.normalized * slideForce;
                }
                else
                {
                    ctrl.StartSliding();
                    Debug.Log("빙판 위 감지 → 슬라이딩 재시작");
                }
                break;

            case TileType.Sand:
                if (rb != null)
                {
                    Vector3 cancel = -rb.velocity * (1f - slowFactor); // 역방향 힘
                    rb.AddForce(cancel, ForceMode.VelocityChange);
                    Debug.Log("샌드 타일에서 속도 조정 시도");
                }
                break;

            case TileType.Magma:
                var damageable = other.GetComponentInParent<IDamagable>();
                if (damageable == null) return;

                float lastTime;
                magmaDamageTimers.TryGetValue(other.gameObject, out lastTime);

                float timeSinceEnter = Time.time - lastTime;

                if (!magmaDamageTimers.ContainsKey(other.gameObject))
                {
                    magmaDamageTimers[other.gameObject] = Time.time;
                    
                }
                else if (timeSinceEnter>=1f)
                {
                    magmaDamageTimers[other.gameObject] = Time.time;
                    damageable.TakeDamage();
                }
                break;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        var ctrl = other.GetComponentInParent<IceSlideController>();
        if (tileType == TileType.Ice && ctrl != null)
        {
            ctrl.StopSliding();
        }

        if (tileType == TileType.Magma)
        {
            magmaDamageTimers.Remove(other.gameObject);
        }
    }
}