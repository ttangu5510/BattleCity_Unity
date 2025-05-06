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

        // 파티클 시작 후 0.5초 뒤에 폭발 데미지 발생
        Invoke(nameof(TriggerExplosionDamage), delayBeforeDamage);
    }

    private void TriggerExplosionDamage()
    {
        if (hasTriggered) return; // 중복 방지
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
                Debug.Log($"데미지 입힘: {hit.name}");
            }
        }
    }
}