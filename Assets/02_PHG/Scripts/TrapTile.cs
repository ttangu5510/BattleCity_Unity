using UnityEngine;

public class TrapTile : TileEnviorment
{
    [SerializeField] private GameObject triggeredTrapPrefab;
    [SerializeField] private GameObject explosionEffectPrefab;

    private bool hasTriggered = false;

    protected override void StayTile_EffectRun(Rigidbody rb = null, IDamagable damagable = null)
    {
        if (hasTriggered || damagable == null) return;

        ExecuteTrap(rb, damagable);
    }

    private void ExecuteTrap(Rigidbody rb, IDamagable damagable)
    {
        hasTriggered = true;

        // 1. 데미지
        damagable.TakeDamage();

        // 2. 폭발 이펙트
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position + Vector3.down * 0.5f, Quaternion.identity);
        }

        // 3. 이동 방향 기준 반대로 밀기

        // 4. 벽 타일 생성
        if (triggeredTrapPrefab != null)
        {
            Instantiate(triggeredTrapPrefab, transform.position, transform.rotation, transform.parent);
        }

        // 5. 자기 제거
        Destroy(gameObject, 0.05f);
    }
    protected override void ExitTile_EffectOff()
    {
        hasTriggered = false;
    }
}