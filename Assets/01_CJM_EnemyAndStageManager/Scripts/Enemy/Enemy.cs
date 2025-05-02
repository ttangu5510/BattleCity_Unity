using UnityEditor.PackageManager;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IMovable
{
    [Header("���")]
    [SerializeField] private EnemyGrade grade;
    [HideInInspector] public EnemyGrade Grade { get; }
    [Header("����")]
    [SerializeField] private EnemyState state;
    [SerializeField] private GameObject target;

    [Header("���� (��޺� ���� ������ EnemyManager���� �ϰ� ����)")]
    [SerializeField] private int hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float shotSpeed;
    [SerializeField] private int scorePoint;
    // Item itmePossession;  // ������ ���� ���� �߰�

    [Header("����")]
    [SerializeField] Transform muzzPoint;
    [SerializeField] Transform body;
    [SerializeField] Vector3 rayOffset;
    [SerializeField] float rayFarForwardDistance;
    [SerializeField] float rayForwardDistance;

    private StageManager sm;
    private EnemyManager em;
    private Rigidbody rb;
     
    private Vector3 dir;

    public bool isDamagable { get; private set; } // �ǰ� ���� ���� ����  (������ �� ����, ������ ������� ���� ���� ����, ���)
    public MoveType moveType { get; set; }


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

        em.StatSetting(out hp, out moveSpeed, out shotSpeed, out scorePoint, grade);
        state = EnemyState.General;

        sm.ActiveEnemyAdd(this);
    }



    void Update()
    {
        //TargetChecking();

        // �̵� ���� ���⿡
        switch (state)
        {
            case EnemyState.General:
                GeneralMove();
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
        PlayerData.Instance.UpdateScore(scorePoint);
    }



    #region �� �̵� ���� ����

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

        Vector3 originPos = muzzPoint.position;
        LayerMask layerMask = LayerMask.GetMask("SolidBlock", "Brick", "Enemy"); // ���� ����, ���� üũ �ǵ���--- �� ���̾� �߰��Ǹ� ���� �߰�
        if (Physics.Raycast(originPos, muzzPoint.forward, rayForwardDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(originPos, originPos + muzzPoint.forward * rayForwardDistance, Color.red);
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
            Debug.DrawLine(originPos, originPos + muzzPoint.forward * rayForwardDistance, Color.green);
        }

        Move(dir);
        Rotate(dir);
    }
    #endregion


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

