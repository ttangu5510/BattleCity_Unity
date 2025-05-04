using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IMovable
{
    [Header("���")]
    [SerializeField] private EnemyGrade grade;
    [HideInInspector] public EnemyGrade Grade { get { return grade; } }
    [Header("����")]
    [SerializeField] public EnemyState state;
    [SerializeField] private GameObject target;

    [Header("���� (Ȯ�ο�)")]
    [SerializeField] private int hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float shotSpeed;
    [SerializeField] private int scorePoint;
    [SerializeField] public float shotCycleRandomSeed_min;
    [SerializeField] public float shotCycleRandomSeed_max;
    // Item itmePossession;  // ������ ���� ���� �߰�

    [Header("����")]
    [SerializeField] Transform muzzPoint;
    [SerializeField] Transform body;
    [SerializeField] Vector3 rayOffset;
    [SerializeField] float rayFarForwardDistance;
    [SerializeField] float rayForwardDistance;
    [SerializeField] private BulletObjectPool bulletPool;


    private StageManager sm;
    private EnemyManager em;
    private Rigidbody rb;

    private Vector3 dir;
    private Vector3 rotDir;

    public bool isDamagable { get; private set; } // �ǰ� ���� ���� ����  (������ �� ����, ������ ������� ���� ���� ����, ���)
    public MoveType moveType { get; set; }

    private Vector3 rangeLevel;

    Coroutine coroutine_Attack;
    Coroutine coroutine_MovePatter_A;


    [SerializeField] private float seedMin_A;
    [SerializeField] private float seedMax_A;

    [SerializeField] public GameObject onTriggerObj;

    //[SerializeField] private Item item;
    // Todo 0430
    // 1. �� �̵� ���� (���¿� ���� �̵� ����)
    // 2. ����(�ݶ��̴�) �������� ���� ������Ʈ
    // => ���� �ȿ� �÷��̾ ������ [�÷��̾� ���� ����], ���� �ȿ� Ÿ���� ������ [Ÿ�� ���� ����], �ƹ��͵� ������ [�Ϲ� ����]
    // 3. StageManager���� �¸�����, �й����� �׽�Ʈ �ʿ�

    private void Start()
    {
        sm = StageManager.Instance;
        em = EnemyManager.Instance;
        rb = transform.GetChild(0).GetComponent<Rigidbody>();

        em.StatSetting(out hp, out moveSpeed, out shotSpeed, out scorePoint, out shotCycleRandomSeed_min, out shotCycleRandomSeed_max, grade);
        state = EnemyState.General;

        sm.ActiveEnemyAdd(this);

        // ���� ����
        dir = body.forward;

        if (coroutine_Attack == null)
            coroutine_Attack = StartCoroutine(AttackCycle());

        if (coroutine_MovePatter_A == null)
            coroutine_MovePatter_A = StartCoroutine(MovePattern_A(seedMin_A, seedMax_A));
    }




    void Update()
    {
        //TargetChecking();

        // �̵� ���� ���⿡
        switch (state)
        {
            case EnemyState.General:
                Move(dir);
                Rotate(rotDir);
                break;
            case EnemyState.ChasingPlayaer:

                break;
            case EnemyState.ChasingTarget:

                break;

            case EnemyState.Stop:

                break;
        }
    }


    public void TakeDamage()
    {
        hp -= 1;
        if (hp <= 0) Dead();
    }

    public void Dead()
    {
        sm.ActiveEnemyListRemove(this);

        // �����ؾ��Ѵٸ� ��������Ʈ�� ��ġ,���� ���� �� ��Ȱ��ȭ
        gameObject.SetActive(false);
        PlayerManager.Instance.ScoreUpdate(scorePoint);

        StopCoroutine(coroutine_Attack);
        StopCoroutine(coroutine_MovePatter_A);
    }

    private void OnDisable()
    {
        StopCoroutine(coroutine_Attack);
    }

    #region �� �̵� ���� ����
    public void RandomDirSet()
    {
        // 4���� �� �ϳ� �������� ��ȯ
        Vector3[] directions = new Vector3[]
        {
                -body.forward,
                body.right,
                -body.right
        };

        int R = Random.Range(0, 3);
        rotDir = directions[R];
        
        if (moveType == MoveType.iceSlide)
        {
            if (rb.velocity.magnitude < 0.1f) dir = rotDir;
            else return;
        }
        else dir = rotDir;
    }

    private void Move(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            rb.velocity = dir * moveSpeed;
        }
        else rb.velocity = Vector3.zero;
    }

    private void Rotate(Vector3 dir)
    {
        body.LookAt(transform.GetChild(0).position + dir);
    }


    // �ڷ�ƾ���� �̵�
    IEnumerator MovePattern_A(float seed_min, float seed_max)
    {
        while (true)
        {
            float r = Random.Range(seed_min, seed_max);
            yield return new WaitForSeconds(r);
            if (onTriggerObj != null)
            {
                yield return new WaitUntil(() => onTriggerObj == null);
                continue;
            }
            RandomDirSet();
        }
    }
    #endregion


    // �� ���� ����
    // ���� ���� �ڷ�ƾ ���� ���
    public void Attack()
    {
        PooledObject bullet = bulletPool.BulletOut();
        if (bullet == null) return;

        bullet.bulletType = PooledObject.BulletType.Type1;

        bullet.transform.position = muzzPoint.position;
        bullet.transform.forward = muzzPoint.forward;
        bullet.GetComponent<Rigidbody>().velocity = shotSpeed * bullet.transform.forward;

        bullet.gameObject.SetActive(true);
    }

    public IEnumerator AttackCycle()
    {
        while (true)
        {
            float r = Random.Range(shotCycleRandomSeed_min, shotCycleRandomSeed_max);
            yield return new WaitForSeconds(r);
            if (state == EnemyState.Stop) yield return new WaitUntil(() => state != EnemyState.Stop);
            Attack();
        }
    }

    // �ٴ� Ÿ�� ���� �Լ�
    public void MoveTypeUpdate()
    {
        // ������ Ÿ�Կ� ���� ����Ʈ&ȿ�� ������Ʈ
        // ����Ʈ�� �÷��̾� ������ �ȿ� �ڽ� ������Ʈ ������ ����
        // ȿ���� ���̽����� ��ġ�� �����ϸ� ��
    }

    // EnemyState�� ���� �̵����� ����
    /*// ���� �� ���� �߻�, �÷��̾�orŸ�� ������ ���� EnemyState ����Ī
    private void TargetChecking()
    {
        Vector3 originPos = muzzPoint.position + rayOffset;
        int layerMask = ~LayerMask.GetMask("PlayerBullet", "EnemyBullet");
        if (Physics.Raycast(originPos, muzzPoint.forward, out RaycastHit hitInfo, rayFarForwardDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            // �÷��̾ �νĵȴٸ�
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.DrawLine(originPos, hitInfo.point, Color.red);

                target = hitInfo.collider.gameObject;
                state = EnemyState.ChasingPlayaer;
            }
            // Todo : ���ӿ������� ���̾� �߰� �ʿ��մϴ�
            // ���� ���� ������ �νĵȴٸ�
            else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("���ӿ�������"))
            {
                // ����� ������ �����սô�. ForwardChecking ����
                target = hitInfo.collider.gameObject;
                state = EnemyState.ChasingTarget;
            }
            // �ƹ��͵� ���ٸ�
            else
            {
                Debug.DrawLine(originPos, hitInfo.point, Color.red);

                target = null;
                state = EnemyState.General;
            }
        }
        else
        {
            Debug.DrawLine(originPos, hitInfo.point, Color.green);
        }
    }

*/
}

public enum EnemyGrade
{
    normal, elite, boss // �߰� ����
}

public enum EnemyState
{
    General, ChasingPlayaer, ChasingTarget, Stop
}

