using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class PooledObject : MonoBehaviour
{
    public BulletObjectPool returnPool;
    // TODO : 발사자 정보 추가
    
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
            //포탄이 곡선을 그리며 날아간다
            transform.forward = rigid.velocity;
            // 오브젝트의 회전 방법 중 하나다
            // transform.forward = 벡터
            // 벡터 방향을 바라보도록(forward 앞이 그쪽을 향하도록) 회전함
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(bulletExplosion, transform.position, transform.rotation).Play();
        // TODO : 레이어에 따라서 다른 결과 연산
        
        returnPool.ReturnToPool(this);
    }

}
