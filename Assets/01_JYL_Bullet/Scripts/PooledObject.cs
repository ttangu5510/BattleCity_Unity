using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class PooledObject : MonoBehaviour
{
    public BulletObjectPool returnPool;
    // TODO : �߻��� ���� �߰�
    
    [SerializeField] ParticleSystem bulletExplosion;
    
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(bulletExplosion, transform.position, transform.rotation).Play();
        // TODO : ���̾ ���� �ٸ� ��� ����
        returnPool.ReturnToPool(this);
    }

}
