using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossTurret : MonoBehaviour, IDamagable
{
    [SerializeField] GameObject turretObject;
    [SerializeField] float rotateSpeed;
    [SerializeField] float shotSpeed;
    [SerializeField] int hp;
    [SerializeField] Transform muzzPoint;

    [SerializeField] private BulletObjectPool bulletPool;
    [SerializeField] GameObject explosionFBX;

    [SerializeField] private float shotCycleRandomSeed_min;
    [SerializeField] private float shotCycleRandomSeed_max;

    [SerializeField] public EnemyState state;
    [SerializeField] public float delay;
    [SerializeField] public int rotStartDir;
    [SerializeField] public float angularOffset;

    Coroutine Coroutine_Attack;
    Coroutine Coroutine_Rotate;


    private void Start()
    {
        Coroutine_Attack = StartCoroutine(AttackCycle());
        Coroutine_Rotate = StartCoroutine(RotateCycle(angularOffset, rotStartDir));

        StageManager.Instance.bossTurrets.Add(this);
    }


    public void TakeDamage()
    {
        hp -= 1;
        if (hp <= 0) Dead();
    }

    public void Dead()
    {
        // 피격 이펙트 instantiate
        GameObject explosion = Instantiate(explosionFBX);
        explosion.transform.position = transform.position;
        gameObject.SetActive(false);

        // 일단 누더기 코드. 리팩토링 필요
        StageManager.Instance.bossTurrets.Remove(this);
        if (StageManager.Instance.bossTurrets.Count <= 0)
        {
            transform.GetComponentInParent<Enemy_Boss>().state = EnemyState.Stop;
            StageManager.Instance.bossSlayed = true;

            if (StageManager.Instance.enemyLifeCount <= 0)
            {
                
                    StageManager.Instance.StageClear();
            }
        }
    }

    public void Attack()
    {
        PooledObject bullet = bulletPool.BulletOut();
        if (bullet == null) return;

        bullet.bulletType = PooledObject.BulletType.Type1;

        bullet.transform.position = muzzPoint.position;
        bullet.transform.forward = muzzPoint.forward;
        bullet.GetComponent<Rigidbody>().velocity = shotSpeed * bullet.transform.forward;

        bullet.gameObject.SetActive(true);
    }
    public IEnumerator AttackCycle()
    {
        while (true)
        {
            float r = Random.Range(shotCycleRandomSeed_min, shotCycleRandomSeed_max);
            yield return new WaitForSeconds(r);
            if (state == EnemyState.Stop) yield return new WaitUntil(() => state != EnemyState.Stop);
            Attack();
            yield return new WaitForSeconds(0.1f);
            Attack();
            yield return new WaitForSeconds(0.1f);
            Attack();

        }
    }


    // -1 : 시계반대방향, +1 : 시계방향
    public void Rotate(float rotateState_H)
    {
        transform.Rotate(Vector3.up, rotateState_H * Time.deltaTime * rotateSpeed);
    }

    public IEnumerator RotateCycle(float angleOffset , int startDir)
    {
        int flag = startDir;
        while (true)
        {
            // 90~270
            if (state == EnemyState.Stop) yield return new WaitUntil(() => state != EnemyState.Stop);

            yield return new WaitForSeconds(delay);
            if (transform.rotation.eulerAngles.y > 270 - angleOffset) flag = -1;
            else if ((transform.rotation.eulerAngles.y < 90 - angleOffset)) flag = 1;

            transform.Rotate(Vector3.up, flag * Time.deltaTime * rotateSpeed);
        }
    }

    private void OnDisable()
    {
        if (Coroutine_Attack != null)
            StopCoroutine(Coroutine_Attack);
        if (Coroutine_Rotate != null)
            StopCoroutine(Coroutine_Rotate);
    }

}
