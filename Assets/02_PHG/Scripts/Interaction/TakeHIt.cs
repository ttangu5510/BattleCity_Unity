using UnityEngine;

public class TakeHit : MonoBehaviour
{
    [SerializeField] private float delayBeforeDamage = 0.5f;
    [SerializeField] private float damageRadius = 5f;
    [SerializeField] private Transform effectOrigin;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private ParticleSystem effect;

    private bool hasTriggered = false;

    private void Start()
    {
        if (effectOrigin == null)
            effectOrigin = transform;

        // ��ƼŬ ���� �� 0.5�� �ڿ� ���� ������ �߻�
        Invoke(nameof(TriggerExplosionDamage), delayBeforeDamage);
    }

    private void TriggerExplosionDamage()
    {
        if (hasTriggered) return; // �ߺ� ����
        hasTriggered = true;

        Collider[] hitColliders = Physics.OverlapSphere(effectOrigin.position, damageRadius, targetLayers);
        foreach (var hit in hitColliders)
        {
            IDamagable damagable =
                hit.GetComponent<IDamagable>() ??
                hit.GetComponentInParent<IDamagable>() ??
                hit.transform.root.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.TakeDamage();
                Debug.Log($"������ ����: {hit.name}");
            }
        }
    }
}