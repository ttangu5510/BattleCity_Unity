using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform muzzPoint;
    [SerializeField] Transform body;

    private Player player;
    private Rigidbody rb;
    private Vector3 dir;


    // TODO : �׽�Ʈ
    [SerializeField] BulletObjectPool bulletPool;
    //[SerializeField] GameObject bulletPrefab;

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

        Vector3 inputDir = new Vector3(x, 0, z).normalized;

        // x���� �Է��� �� ������ Ⱦ �Է� ���� (���̽�ƽ ����, Ű����� ���� ���� �����ϴ� ������)
        if (Mathf.Abs(inputDir.x) > Mathf.Abs(inputDir.z))
            dir = (transform.right * inputDir.x).normalized;
        else
            dir = (transform.forward * inputDir.z).normalized;

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
        // TODO : �׽�Ʈ
        //GameObject gameObject = Instantiate(bulletPrefab); // �ӽ� / temp
        GameObject gameObject = bulletPool.BulletOut().gameObject;

        gameObject.transform.position = muzzPoint.position;
        gameObject.transform.forward = muzzPoint.forward;
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * Time.deltaTime, ForceMode.Impulse);

        gameObject.SetActive(true);
    }

}
