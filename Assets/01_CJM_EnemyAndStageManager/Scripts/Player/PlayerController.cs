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

        // Bullet Pool �����ڷ� bulletPool �ʵ忡 �Ҵ�. 
    }

    private void Update()
    {
        // �Է��� 4���� �������ͷ� ���� �� dir�� ����
        #region dir(�Է�)
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(x, 0, z).normalized;

        // x���� �Է��� �� ������ Ⱦ �Է� ���� (���̽�ƽ ����, Ű����� ���� ���� �����ϴ� ������)
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

        // �̵��ϱ� ���ϴ� ������ ȸ��

        // ����Ű �Է�, ����
        if (Input.GetKeyDown(KeyCode.X))
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        // �̵�(����)
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
            Debug.Log("Ǯ ������Ʈ ��� ����!");
            return;
        }

        GameObject gameObject = bulletPool.BulletOut().gameObject;
        
        // Todo: �÷��̾� ��޿� ���� �Ѿ� Ÿ�� ����, ���� �� Ȱ��ȭ �սô�
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
