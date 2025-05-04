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
    
    // 포탄 프리펩에서 RigidBody Constrains에서 각도 회전 잠궈두었습니다.
    // 기능 중복으로 삭제해도 되지만, 혹시 모르니 주석처리 해둡니다. (곡사포를 추가로 만들 수도 있으니 남겨둘게요)
    /*private void Update()
    {
        if (rigid.velocity.magnitude > 1)
        {
            //포탄이 곡선을 그리며 날아간다
            transform.forward = rigid.velocity;
            // 오브젝트의 회전 방법 중 하나다
            // transform.forward = 벡터
            // 벡터 방향을 바라보도록(forward 앞이 그쪽을 향하도록) 회전함
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(bulletExplosion, transform.position, transform.rotation).Play();
        goPosition = gameObject.transform.position;
        // 비트시프트를 사용해서 찾는 방법과, NameToLayer를 통해서 찾을 수 있다
        // layer는 LayerMask와 같지 않다. 이것을 이해해야 해결되는 문제
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
