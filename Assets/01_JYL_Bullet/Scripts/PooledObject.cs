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
            //포탄이 곡선을 그리며 날아간다
            transform.forward = rigid.velocity;
            // 오브젝트의 회전 방법 중 하나다
            // transform.forward = 벡터
            // 벡터 방향을 바라보도록(forward 앞이 그쪽을 향하도록) 회전함
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌 시작");
        Instantiate(bulletExplosion, transform.position, transform.rotation).Play();
        // TODO : 레이어에 따라서 다른 결과 연산
        // 비트시프트를 사용해서 찾는 방법과, NameToLayer를 통해서 찾을 수 있다
        // layer는 LayerMask와 같지 않다. 이것을 이해해야 해결되는 문제
        // if((1 << collision.gameObject.layer & Breakable.value) == 1 << collision.gameObject.layer)
        if (collision.gameObject.layer == LayerMask.NameToLayer("Brick") || collision.gameObject.layer == LayerMask.NameToLayer("SolidBlock"))
        {
            Debug.Log($"{collision.gameObject.name}벽돌에 부딪힘");
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
