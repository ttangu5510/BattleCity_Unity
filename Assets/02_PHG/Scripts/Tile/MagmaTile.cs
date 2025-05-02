using System.Collections;
using UnityEngine;

public class MagmaTile : TileEnviorment
{
    [SerializeField] private GameObject burnEffectPrefab;
    [SerializeField] private float damageCycleTime;
    [SerializeField] private bool canHeat = true;
    Coroutine damageCyclePattern;
    protected override void StayTile_EffectRun(Rigidbody rb, IDamagable damagable)
    {
        if (damageCyclePattern == null && canHeat)
        {
            Debug.LogError("���� ���� ����");
            damageCyclePattern = StartCoroutine(DotDamageCycle(damagable));
        }

    }

    IEnumerator DotDamageCycle(IDamagable damagable)
    {
        damagable.TakeDamage();
        canHeat = false;
        Debug.Log("������ �ް� ��� ��");
        yield return new WaitForSeconds(damageCycleTime);
        canHeat = true;
        Debug.Log("��Ÿ�� �Ϸ�");
    }

    protected override void SendTileType(IMovable movable)
    {
        movable.moveType = MoveType.lavarDotDamaged;
    }

    private void OnDisable()
    {
        if (damageCyclePattern != null)
        {
            StopCoroutine(damageCyclePattern);
        }
    }
}
