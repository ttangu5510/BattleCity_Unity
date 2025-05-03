using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public BulletObjectPool returnPool;
    public enum BulletType { Type1, Type2 };
    [SerializeField] ParticleSystem bulletExplosion;
    private Rigidbody rigid;
    public BulletType bulletType;
    private Vector3 rangeLevel1;
    private Vector3 rangeLevel2;
    [SerializeField] private LayerMask Breakable;
    private Vector3 goPosition;

    private bool isFirstInit = true;

    private void Awake()
    {
        rigid ??= GetComponent<Rigidbody>();
        rangeLevel1 = new Vector3(5, 5, 0.5f);
        rangeLevel2 = new Vector3(5, 5, 1.5f);
        Breakable = LayerMask.GetMask("Brick", "SolidBlock");
    }
    private void FixedUpdate()
    {
        if (Mathf.Abs(transform.forward.x) > Mathf.Abs(transform.forward.z))
        {
            rangeLevel1 = new Vector3(0.1f, 2f, 0.8f);
            rangeLevel2 = new Vector3(0.5f, 2f, 0.8f);

        }
        else
        {
            rangeLevel1 = new Vector3(0.8f, 2f, 0.1f);
            rangeLevel2 = new Vector3(0.8f, 2f, 0.5f);

        }
    }
    
    // ��ź �����鿡�� RigidBody Constrains���� ���� ȸ�� ��ŵξ����ϴ�.
    // ��� �ߺ����� �����ص� ������, Ȥ�� �𸣴� �ּ�ó�� �صӴϴ�. (������� �߰��� ���� ���� ������ ���ܵѰԿ�)
    /*private void Update()
    {
        if (rigid.velocity.magnitude > 1)
        {
            //��ź�� ��� �׸��� ���ư���
            transform.forward = rigid.velocity;
            // ������Ʈ�� ȸ�� ��� �� �ϳ���
            // transform.forward = ����
            // ���� ������ �ٶ󺸵���(forward ���� ������ ���ϵ���) ȸ����
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(bulletExplosion, transform.position, transform.rotation).Play();
        goPosition = gameObject.transform.position;
        // ��Ʈ����Ʈ�� ����ؼ� ã�� �����, NameToLayer�� ���ؼ� ã�� �� �ִ�
        // layer�� LayerMask�� ���� �ʴ�. �̰��� �����ؾ� �ذ�Ǵ� ����
        // if((1 << collision.gameObject.layer & Breakable.value) == 1 << collision.gameObject.layer)
        if (bulletType == BulletType.Type1)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Brick"))
            {
                Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward * 0.5f, rangeLevel1, Quaternion.identity, LayerMask.GetMask("Brick"));
                foreach (var collide in colliders)
                {
                    BrickAction ba = collide.gameObject.GetComponent<BrickAction>();
                    if (ba != null)
                        ba.BrickDestroy(goPosition);
                }
            }
        }
        else
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Brick") || collision.gameObject.layer == LayerMask.NameToLayer("SolidBlock"))
            {
                Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward * 0.5f, rangeLevel2, Quaternion.identity, Breakable);
                foreach (var collide in colliders)
                {
                    BrickAction ba = collide.gameObject.GetComponent<BrickAction>();
                    if (ba != null)
                        ba.BrickDestroy(goPosition);
                }
            }
        }

        IDamagable damagable = collision.gameObject.transform.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage();
        }

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (isFirstInit)
        {
            isFirstInit = false;
        }
        else returnPool.ReturnToPool(this);
    }

    private void OnDrawGizmos()
    {
        if (bulletType == BulletType.Type1)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position + transform.forward * 0.5f, rangeLevel1*2);

        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position + transform.forward * 0.5f, rangeLevel2*2);
        }

    }
}
