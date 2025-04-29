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
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(x, 0, z).normalized;

        if (inputDir == Vector3.zero) return;

        // x���� �Է��� �� ������ Ⱦ �Է� ���� (���̽�ƽ ����, Ű����� ���� ���� �����ϴ� ������)
        if (Mathf.Abs(inputDir.x) > Mathf.Abs(inputDir.z))
            dir = (transform.right * inputDir.x).normalized;
        else
            dir = (transform.forward * inputDir.z).normalized;

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
        GameObject gameObject = Instantiate(bulletPrefab); // �ӽ� / temp
        //GameObject gameObject = bulletPool.BulletOut().gameObject;

        gameObject.transform.position = muzzPoint.position;
        gameObject.transform.forward = muzzPoint.forward;
        gameObject.GetComponent<Rigidbody>().velocity = player.shotSpeed * gameObject.transform.forward;
        
        gameObject.SetActive(true);
    }

}
