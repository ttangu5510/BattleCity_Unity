using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform muzzPoint;
    [SerializeField] Transform body;

    private Player player;
    private Rigidbody rb;
    private Vector3 dir;

    [SerializeField] private BulletObjectPool bulletPool;

    private void Awake()
    {
        player = transform.parent.GetComponent<Player>();
        rb = GetComponent<Rigidbody>();

        // Bullet Pool 생성자로 bulletPool 필드에 할당. 
    }

    private void Update()
    {
        // 입력을 4방향 단위벡터로 연산 후 dir에 저장
        #region dir(입력)
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(x, 0, z).normalized;

        // x방향 입력이 더 많으면 횡 입력 판정 (조이스틱 기준, 키보드는 현재 방향 유지하는 쪽으로)
        if (inputDir == Vector3.zero)
        {
            dir = Vector3.zero;
        }
        else
        {
            if (Mathf.Abs(inputDir.x) > Mathf.Abs(inputDir.z))
            {
                dir = (transform.right * inputDir.x).normalized;
            }
            else
            {
                dir = (transform.forward * inputDir.z).normalized;
            }
            Rotate();
        }
        #endregion

        // 이동하길 원하는 각도로 회전

        // 공격키 입력, 공격
        if (Input.GetKeyDown(KeyCode.X))
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        // 이동(물리)
        Move();
    }

    private void Move()
    {
        if (dir != Vector3.zero)
        {
            rb.velocity = dir * player.moveSpeed;
        }
        else rb.velocity = Vector3.zero;
    }

    private void Rotate()
    {
        body.LookAt(transform.position + dir);
    }

    private void Attack()
    {
        if (bulletPool.PoolCount() <= 0)
        {
            Debug.Log("풀 오브젝트 모두 소진!");
            return;
        }

        GameObject gameObject = bulletPool.BulletOut().gameObject;
        
        // Todo: 플레이어 등급에 따른 총알 타입 구분, 머지 후 활성화 합시다
        if(player.grade == UpgradeType.Grade04)
        {
            // gameObject.GetComponent<PooledObject>().bulletType = bulletType.Type2;
        }
        else
        {
            // gameObject.GetComponent<PooledObject>().bulletType = bulletType.Type1;
        }

        gameObject.transform.position = muzzPoint.position;
        gameObject.transform.forward = muzzPoint.forward;
        gameObject.GetComponent<Rigidbody>().velocity = player.shotSpeed * gameObject.transform.forward;

        gameObject.SetActive(true);
    }

}
