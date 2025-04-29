using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour // IDamagable
{
    [SerializeField] private int hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float shotSpeed;
    [SerializeField] private EnemyType type;
    [SerializeField] private int scorePoint;

    // Item itemPossession

    // private BulletObjectPool<Bullet> bulletPool;
    [SerializeField] Transform muzzPoint;
    [SerializeField] Transform body;
    [SerializeField] GameObject bulletPrefab; // 임시 / temp

    private void Start()
    {

    }

    // 데미지 받음 => 죽음 판정
    public void TakeDamage() // : IDamagable
    {
        hp -= 1;
        if (hp <= 0) Dead();
    }

    // 죽음 => 게임오버 판정
    public void Dead()
    {
        //StageManager.Instance.적 남은 목숨 감소;
        Despawn();

        // 적 남은 목숨
        if (/*적 남은 목숨 <= 0*/ false)
        {
            //StageManager.Instance.승리조건 체크;
        }
    }

    // 스폰
    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    // 디스폰
    public void Despawn()
    {
        // 스폰 포인트로 이동
        transform.position = transform.parent.position;

        // 비활성화
        gameObject.SetActive(false);
    }



    #region 이동 로직

    // 기본 이동
    private void Move_Basic()
    {
        // 4방향 랜덤 이동
    }

    // 타겟 추적 이동
    private void Move_Chasing(GameObject target)
    {
        // 4방향 한정 타겟 추적
        // NaviMesh
    }

    private void Move(Vector3 dir)
    {
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    private void Rotate(Vector3 dir)
    {
        body.LookAt(transform.position + dir);
    }

    #endregion

    
    private void Attack()
    {
        GameObject gameObject = Instantiate(bulletPrefab); // 임시 / temp
        //GameObject gameObject = bulletPool.BulletOut().gameObject;

        gameObject.transform.position = muzzPoint.position;
        gameObject.transform.forward = muzzPoint.forward;

        gameObject.SetActive(true);
    }
}

public enum EnemyType
{
    normal, elite, boss // 추가 가능
}