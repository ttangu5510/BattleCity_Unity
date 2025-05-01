﻿using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public BulletObjectPool returnPool;
    public enum BulletType { Type1, Type2 };
    [SerializeField] ParticleSystem bulletExplosion;
    private Rigidbody rigid;
    public BulletType bulletType;
    private Vector3 rangeLevel1;
    private Vector3 rangeLevel2;
    private Quaternion rotation;
    [SerializeField] private LayerMask Breakable;
    private Vector3 goPosition;
    private void Awake()
    {
        rigid ??= GetComponent<Rigidbody>();

        rotation = Quaternion.LookRotation(transform.forward);
        Breakable = LayerMask.GetMask("Brick", "SolidBlock");
    }
    private void Update()
    {
        if (rigid.velocity.magnitude > 3)
        {
            //포탄이 곡선을 그리며 날아간다
            transform.forward = rigid.velocity;
            // 오브젝트의 회전 방법 중 하나다
            // transform.forward = 벡터
            // 벡터 방향을 바라보도록(forward 앞이 그쪽을 향하도록) 회전함
        }
    }
    private void FixedUpdate()
    {
        if (Mathf.Abs(transform.forward.x) > Mathf.Abs(transform.forward.z))
        {
            rangeLevel1 = new Vector3(0.5f, 5, 5);

        }
        else
        {
            rangeLevel1 = new Vector3(5, 5, 0.5f);

        }
        if (Mathf.Abs(transform.forward.x) > Mathf.Abs(transform.forward.z))
        {
            rangeLevel2 = new Vector3(1.5f, 5, 5);
        }
        else
        {
            rangeLevel2 = new Vector3(5, 5, 1.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(bulletExplosion, transform.position, transform.rotation).Play();
        // 비트시프트를 사용해서 찾는 방법과, NameToLayer를 통해서 찾을 수 있다
        // layer는 LayerMask와 같지 않다. 이것을 이해해야 해결되는 문제
        // if((1 << collision.gameObject.layer & Breakable.value) == 1 << collision.gameObject.layer)
        if (bulletType == BulletType.Type1)
        {

            if (collision.gameObject.layer == LayerMask.NameToLayer("Brick"))
            {
                Collider[] colliders = Physics.OverlapBox(transform.position, rangeLevel1, Quaternion.identity, LayerMask.GetMask("Brick"));
                goPosition = gameObject.transform.position;
                foreach (var collide in colliders)
                {
                    BrickAction ba = collide.gameObject.GetComponent<BrickAction>();
                    if (ba != null)
                        ba.BrickDestroy(goPosition);
                }
            }
        }
        else
        {

            if (collision.gameObject.layer == LayerMask.NameToLayer("Brick") || collision.gameObject.layer == LayerMask.NameToLayer("SolidBlock"))
            {
                Collider[] colliders = Physics.OverlapBox(transform.position, rangeLevel1, Quaternion.identity, Breakable);
                goPosition = gameObject.transform.position;
                foreach (var collide in colliders)
                {
                    BrickAction ba = collide.gameObject.GetComponent<BrickAction>();
                    if (ba != null)
                        ba.BrickDestroy(goPosition);
                }
            }
        }

        IDamagable damagable = collision.gameObject.transform.root.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage();
        }

        returnPool.ReturnToPool(this);
    }
    private void OnDrawGizmos()
    {
        if (bulletType == BulletType.Type1)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position + transform.forward, rangeLevel1);

        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position + transform.forward, rangeLevel2);
        }

    }
}
