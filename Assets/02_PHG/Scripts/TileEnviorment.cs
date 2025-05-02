using UnityEngine;

public class TileEnviorment : MonoBehaviour
{
    [SerializeField] private bool flag;
    protected virtual void EnterTile_EffectOn(Rigidbody rb) { }


    protected virtual void StayTile_EffectRun(Rigidbody rb) { }


    protected virtual void ExitTile_EffectOff() { }


    protected virtual void SendTileType(IMovable movable) { }






    private void OnTriggerEnter(Collider other)
    {
        if (flag) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IMovable movable = other.GetComponentInParent<IMovable>();
            if (movable == null)
            {
                Debug.LogWarning("IMovable 인터페이스가 없는 적 or 플레이어 존재");
                return;
            }
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            EnterTile_EffectOn(rb);
            SendTileType(movable);
            movable.MoveTypeUpdate();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (flag) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IMovable movable = other.GetComponentInParent<IMovable>();
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            StayTile_EffectRun(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!flag) return;
        flag = true;
        ExitTile_EffectOff();

    }
}
