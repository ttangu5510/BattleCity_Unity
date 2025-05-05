using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHIt : MonoBehaviour
{
    [SerializeField] private float damageRadius = 1f;
    [SerializeField] private LayerMask targetLayer; // Player, Enemy 등 감지 대상
    [SerializeField] private Transform effectOrigin; // 이펙트 발생 위치

    private void OnParticleSystemStopped()
    {
        TriggerExplosionDamage();
    }
    private void TriggerExplosionDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(effectOrigin.position, damageRadius, targetLayer);

        foreach (var hit in hitColliders)
        {
            IDamagable damagable = hit.GetComponentInParent<IDamagable>();
            Rigidbody rb = hit.GetComponentInParent<Rigidbody>();

            if (damagable != null)
            {
                damagable.TakeDamage();
            }
        }
    }
}
