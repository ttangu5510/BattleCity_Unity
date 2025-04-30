using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class PooledObject : MonoBehaviour
{
    public BulletObjectPool returnPool;
    // TODO : �߻��� ���� �߰�
    
    [SerializeField] ParticleSystem bulletExplosion;
    private Rigidbody rigid;
    private void Awake()
    {
        rigid ??= GetComponent<Rigidbody>();
    }
    private void Update()
    { 
        if (rigid.velocity.magnitude > 3)
        {
            //��ź�� ��� �׸��� ���ư���
            transform.forward = rigid.velocity;
            // ������Ʈ�� ȸ�� ��� �� �ϳ���
            // transform.forward = ����
            // ���� ������ �ٶ󺸵���(forward ���� ������ ���ϵ���) ȸ����
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(bulletExplosion, transform.position, transform.rotation).Play();
        // TODO : ���̾ ���� �ٸ� ��� ����
        
        returnPool.ReturnToPool(this);
    }

}
