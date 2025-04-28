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

    // private BulletObjectPool<Bullet> bulletPool;
    [SerializeField] GameObject bulletPrefab; // 임시 / temp

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();

        // Bullet Pool 생성자로 bulletPool 필드에 할당. 
    }

    private void Update()
    {
        #region 방향키 입력, 이동 및 회전
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        dir = new Vector3(x,0,z).normalized;

        // rb로 이동 구현하려면 FixedUpdate로 옮기자
        Move();
        Rotate();
        #endregion

        #region 공격키 입력, 공격
        if (Input.GetKeyDown(KeyCode.X))
        {
            Attack();
        }
        #endregion
    }

    private void Move()
    {
        transform.Translate(dir * player.moveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        body.LookAt(transform.position + dir);
    }

    private void Attack()
    {
        GameObject gameObject = Instantiate(bulletPrefab); // 임시 / temp
        //GameObject gameObject = bulletPool.BulletOut().gameObject;

        gameObject.transform.position = muzzPoint.position;
        gameObject.transform.forward = muzzPoint.forward;

        gameObject.SetActive(true);
    }

}
