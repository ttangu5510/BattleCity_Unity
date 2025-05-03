using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IMovable
{
    [Header("���")]
    [SerializeField] private EnemyGrade grade;
    [HideInInspector] public EnemyGrade Grade { get { return grade; } }
    [Header("����")]
    [SerializeField] private EnemyState state;
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

    public bool isDamagable { get; private set; } // �ǰ� ���� ���� ����  (������ �� ����, ������ ������� ���� ���� ����, ���)
    public MoveType moveType { get; set; }

    private Vector3 rangeLevel;

    Coroutine coroutine_Attack;

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
    }




    void Update()
    {
        //TargetChecking();

        // �̵� ���� ���⿡
        switch (state)
        {
            case EnemyState.General:
                //GeneralMove();
                /*if (rb.velocity.magnitude <= 2.99f)
                {
                    RandomDirSet();
                }*/
                Move(dir);
                Rotate(dir);
                break;
            case EnemyState.ChasingPlayaer:

                break;
            case EnemyState.ChasingTarget:

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

        gameObject.SetActive(false);
        transform.SetSiblingIndex(transform.parent.childCount - 1);
        PlayerManager.Instance.ScoreUpdate(scorePoint);
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
        dir = directions[R];
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


    // ���� �� ���� �߻�, �÷��̾�orŸ�� ������ ���� EnemyState ����Ī
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


    private void GeneralMove()
    {
        // ���� ���� : ���� ���� 4�������� �̵�
        // ��ǥ : ���� ���� 4���� �� �ϳ� �����ؼ� �� �̵�, ���� ����� �ٸ� ���� ���ͷ� �ٲ��ֱ�
        if (dir == Vector3.zero) dir = transform.forward;

        // ª�� ���� �߻�
        // ���� ������ ���� ��ȯ ���

        Vector3 right;
        if (Mathf.Abs(dir.x) > 0)
        {
            // Ⱦ �̵� ��
            right = transform.forward * 0.7f;
        }
        else
        {
            // �� �̵� ��
            right = transform.right * 0.7f;
        }

        Vector3 originPos1 = muzzPoint.position + right;
        Vector3 originPos2 = muzzPoint.position - right;
        LayerMask layerMask = LayerMask.GetMask("SolidBlock", "Brick", "Enemy"); // ���� ����, ���� üũ �ǵ���--- �� ���̾� �߰��Ǹ� ���� �߰�
        if (Physics.Raycast(originPos1, muzzPoint.forward, rayForwardDistance, layerMask, QueryTriggerInteraction.Ignore) ||
            Physics.Raycast(originPos2, muzzPoint.forward, rayForwardDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(originPos1, originPos1 + muzzPoint.forward * rayForwardDistance, Color.red);
            Debug.DrawLine(originPos2, originPos2 + muzzPoint.forward * rayForwardDistance, Color.red);
            Debug.Log("���� ����");
            // 4���� �� �ϳ� �������� ��ȯ
            Vector3[] directions = new Vector3[]
            {
               transform.forward,
               -transform.forward,
               transform.right,
               -transform.right
            };

            int R = Random.Range(0, 4);
            dir = directions[R];
        }
        else
        {
            Debug.DrawLine(originPos1, originPos1 + muzzPoint.forward * rayForwardDistance, Color.green);
            Debug.DrawLine(originPos2, originPos2 + muzzPoint.forward * rayForwardDistance, Color.green);
        }

        Move(dir);
        Rotate(dir);
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
}

public enum EnemyGrade
{
    normal, elite, boss // �߰� ����
}

public enum EnemyState
{
    General, ChasingPlayaer, ChasingTarget
}

