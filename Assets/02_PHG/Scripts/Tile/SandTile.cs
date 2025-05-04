using UnityEngine;

public class SandTile : TileEnviorment
{
    [SerializeField] private float slowFactor = 0.8f;

    protected override void StayTile_EffectRun(Rigidbody rb, IDamagable damagable)
    {
        if (rb != null)
        {
            Vector3 cancel = -rb.velocity * (1f - slowFactor); // ¿ª¹æÇâ Èû
            rb.AddForce(cancel, ForceMode.VelocityChange);
        }
    }
    protected override void SendTileType(IMovable movable)
    {
        movable.moveType = MoveType.sandSlow;

    }
}
