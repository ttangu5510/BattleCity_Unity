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
    [SerializeField] GameObject bulletPrefab; // �ӽ� / temp

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();

        // Bullet Pool �����ڷ� bulletPool �ʵ忡 �Ҵ�. 
    }

    private void Update()
    {
        #region ����Ű �Է�, �̵� �� ȸ��
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        dir = new Vector3(x,0,z).normalized;

        // rb�� �̵� �����Ϸ��� FixedUpdate�� �ű���
        Move();
        Rotate();
        #endregion

        #region ����Ű �Է�, ����
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
        GameObject gameObject = Instantiate(bulletPrefab); // �ӽ� / temp
        //GameObject gameObject = bulletPool.BulletOut().gameObject;

        gameObject.transform.position = muzzPoint.position;
        gameObject.transform.forward = muzzPoint.forward;

        gameObject.SetActive(true);
    }

}
