using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SandTile : TileEnviorment
{
    [SerializeField] private float slowFactor=0.8f;

    protected override void StayTile_EffectRun(Rigidbody rb)
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
