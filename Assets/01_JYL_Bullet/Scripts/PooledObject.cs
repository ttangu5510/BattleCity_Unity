using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public BulletObjectPool returnPool;
    public enum BulletType { Type1, Type2 };
    [SerializeField] ParticleSystem bulletExplosion;
    private Rigidbody rigid;
    public BulletType bulletType;
    private Vector3 range;
    [SerializeField] private LayerMask Breakable;
    private void Awake()
    {
        rigid ??= GetComponent<Rigidbody>();
        range = new Vector3(5, 10, 1);
        Breakable = LayerMask.GetMask("Brick", "SolidBlock");
    }
    private void Update()
    {
        if (rigid.velocity.magnitude > 3)
        {
            //��ź�� ��� �׸��� ���ư���
            transform.forward = rigid.velocity;
            // ������Ʈ�� ȸ�� ��� �� �ϳ���
            // transform.forward = ����
            // ���� ������ �ٶ󺸵���(forward ���� ������ ���ϵ���) ȸ����
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("�浹 ����");
        Instantiate(bulletExplosion, transform.position, transform.rotation).Play();
        // TODO : ���̾ ���� �ٸ� ��� ����
        // ��Ʈ����Ʈ�� ����ؼ� ã�� �����, NameToLayer�� ���ؼ� ã�� �� �ִ�
        // layer�� LayerMask�� ���� �ʴ�. �̰��� �����ؾ� �ذ�Ǵ� ����
        // if((1 << collision.gameObject.layer & Breakable.value) == 1 << collision.gameObject.layer)
        if (collision.gameObject.layer == LayerMask.NameToLayer("Brick") || collision.gameObject.layer == LayerMask.NameToLayer("SolidBlock"))
        {
            Debug.Log($"{collision.gameObject.name}������ �ε���");
            Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward, range, Quaternion.identity, Breakable);
            foreach (var collide in colliders)
            {
                collide.gameObject.SetActive(false);
                Debug.Log($"{collide.gameObject.name}");
            }
        }
        else if (collision.gameObject.layer == Breakable && bulletType == BulletType.Type2)
        {

        }

        IDamagable damagable = GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage();
        }

        returnPool.ReturnToPool(this);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position + transform.forward, range);
    }
}
