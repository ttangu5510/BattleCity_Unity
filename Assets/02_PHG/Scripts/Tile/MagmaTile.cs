using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaTile : TileEnviorment
{
    [SerializeField] private float burnDamagePerSecond = 10f;
    [SerializeField] private GameObject burnEffectPrefab;
    private Dictionary<GameObject, float> magmaDamageTimers = new Dictionary<GameObject, float>();
    protected override void StayTile_EffectRun(Rigidbody rb, IDamagable damagable)
    {
        var damageable = rb.GetComponentInParent<IDamagable>();
        if (damageable == null) return;

        float lastTime;
        magmaDamageTimers.TryGetValue(rb.gameObject, out lastTime);

        float timeSinceEnter = Time.time - lastTime;

        if (!magmaDamageTimers.ContainsKey(rb.gameObject))
        {
            magmaDamageTimers[rb.gameObject] = Time.time;

        }
        else if (timeSinceEnter >= 1f)
        {
            magmaDamageTimers[rb.gameObject] = Time.time;
            Instantiate(burnEffectPrefab, rb.transform.position + Vector3.up * 1.5f, Quaternion.identity);
            damageable.TakeDamage();
        }
    }

    protected override void SendTileType(IMovable movable)
    {
        movable.moveType = MoveType.lavarDotDamaged;
    }

}
