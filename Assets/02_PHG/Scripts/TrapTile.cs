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

        // 1. ������
        damagable.TakeDamage();

        // 2. ���� ����Ʈ
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position + Vector3.down * 0.5f, Quaternion.identity);
        }

        // 3. �̵� ���� ���� �ݴ�� �б�

        // 4. �� Ÿ�� ����
        if (triggeredTrapPrefab != null)
        {
            Instantiate(triggeredTrapPrefab, transform.position, transform.rotation, transform.parent);
        }

        // 5. �ڱ� ����
        Destroy(gameObject, 0.05f);
    }
    protected override void ExitTile_EffectOff()
    {
        hasTriggered = false;
    }
}