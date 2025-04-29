using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class PooledObject : MonoBehaviour
{
    public BulletObjectPool returnPool;
    // TODO : 발사자 정보 추가
    
    [SerializeField] ParticleSystem bulletExplosion;
    
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(bulletExplosion, transform.position, transform.rotation).Play();
        // TODO : 레이어에 따라서 다른 결과 연산
        returnPool.ReturnToPool(this);
    }

}
