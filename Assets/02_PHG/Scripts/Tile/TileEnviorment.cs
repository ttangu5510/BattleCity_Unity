using UnityEngine;

public class TileEnviorment : MonoBehaviour
{
    [SerializeField] private bool flag = false;

    protected virtual void StayTile_EffectRun(Rigidbody rb = null, IDamagable damagable = null) { }


    protected virtual void ExitTile_EffectOff() { }


    protected virtual void SendTileType(IMovable movable) { }



    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IMovable movable = other.GetComponentInParent<IMovable>();

            if (flag)
            {
                Rigidbody rb = other.GetComponentInParent<Rigidbody>();
                IDamagable damagable = other.GetComponentInParent<IDamagable>();
                    StayTile_EffectRun(rb, damagable);
            }
            else
            {
                Collider myCollider = GetComponent<BoxCollider>();
                if (myCollider.bounds.Contains(other.bounds.min) && myCollider.bounds.Contains(other.bounds.max))
                {
                    SendTileType(movable);
                    flag = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IMovable movable = other.GetComponentInParent<IMovable>();
            ExitTile_EffectOff();
            movable.moveType = MoveType.none;
            flag = false;
        }
    }
}