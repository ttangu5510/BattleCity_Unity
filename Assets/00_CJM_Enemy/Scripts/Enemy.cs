using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour // IDamagable
{
    [SerializeField] private int hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float shotSpeed;
    [SerializeField] public UpgradeType grade;

    // private BulletObjectPool<Bullet> bulletPool;
    [SerializeField] Transform muzzPoint;
    [SerializeField] Transform body;
    [SerializeField] GameObject bulletPrefab; // �ӽ� / temp



    // ������ ���� => ���� ����
    public void TakeDamage() // : IDamagable
    {
        hp -= 1;
        if (hp <= 0) Dead();
    }

    // ���� => ���ӿ��� ����
    public void Dead()
    {
        //StageManager.Instance.�� ���� ��� ����;
        Despawn();

        // �� ���� ���
        if (/*�� ���� ��� <= 0*/ false)
        {
            //StageManager.Instance.�¸����� üũ;
        }
    }

    // ����
    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    // ����
    public void Despawn()
    {
        // ���� ����Ʈ�� �̵�
        transform.position = transform.parent.position;

        // ��Ȱ��ȭ
        gameObject.SetActive(false);
    }



    #region �̵� ����

    // �⺻ �̵�
    private void Move_Basic()
    {

    }

    // Ÿ�� ���� �̵�
    private void Move_Chasing(GameObject target)
    {

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
        GameObject gameObject = Instantiate(bulletPrefab); // �ӽ� / temp
        //GameObject gameObject = bulletPool.BulletOut().gameObject;

        gameObject.transform.position = muzzPoint.position;
        gameObject.transform.forward = muzzPoint.forward;

        gameObject.SetActive(true);
    }
}
