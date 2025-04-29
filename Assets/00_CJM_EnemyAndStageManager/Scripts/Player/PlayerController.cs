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

    //[SerializeField] private BulletObjectPool bulletPool;
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
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(x, 0, z).normalized;

        if (inputDir == Vector3.zero) return;

        // x방향 입력이 더 많으면 횡 입력 판정 (조이스틱 기준, 키보드는 현재 방향 유지하는 쪽으로)
        if (Mathf.Abs(inputDir.x) > Mathf.Abs(inputDir.z))
            dir = (transform.right * inputDir.x).normalized;
        else
            dir = (transform.forward * inputDir.z).normalized;

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

    private void FixedUpdate()
    {
        if (dir == Vector3.zero) return;
        
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
        gameObject.GetComponent<Rigidbody>().velocity = player.shotSpeed * gameObject.transform.forward;
        
        gameObject.SetActive(true);
    }

}
