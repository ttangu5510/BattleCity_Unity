using UnityEditor.PackageManager;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IMovable
{
    [Header("등급")]
    [SerializeField] private EnemyGrade grade;
    [HideInInspector] public EnemyGrade Grade { get; }
    [Header("상태")]
    [SerializeField] private EnemyState state;
    [SerializeField] private GameObject target;

    [Header("스펙 (등급별 스펙 정보는 EnemyManager에서 일괄 수정)")]
    [SerializeField] private int hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float shotSpeed;
    [SerializeField] private int scorePoint;
    // Item itmePossession;  // 아이템 소유 정보 추가

    [Header("세팅")]
    [SerializeField] Transform muzzPoint;
    [SerializeField] Transform body;
    [SerializeField] Vector3 rayOffset;
    [SerializeField] float rayFarForwardDistance;
    [SerializeField] float rayForwardDistance;

    private StageManager sm;
    private EnemyManager em;
    private Rigidbody rb;
     
    private Vector3 dir;

    public bool isDamagable { get; private set; } // 피격 가능 상태 여부  (리스폰 중 무적, 아이템 사용으로 인한 무적 상태, 등등)
    public MoveType moveType { get; set; }


    // Todo 0430
    // 1. 적 이동 로직 (상태에 따른 이동 구현)
    // 2. 범위(콜라이더) 판정으로 상태 업데이트
    // => 범위 안에 플레이어가 들어오면 [플레이어 추적 상태], 범위 안에 타겟이 들어오면 [타겟 추적 상태], 아무것도 없으면 [일반 상태]
    // 3. StageManager에서 승리조건, 패배조건 테스트 필요

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

        // 이동 로직 여기에
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



    #region 적 이동 관련 로직

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


    // 정면 긴 레이 발사, 플레이어or타겟 유무에 따라 EnemyState 스위칭
    private void TargetChecking()
    {
        Vector3 originPos = muzzPoint.position + rayOffset;
        int layerMask = ~LayerMask.GetMask("PlayerBullet", "EnemyBullet");
        if (Physics.Raycast(originPos, muzzPoint.forward, out RaycastHit hitInfo, rayFarForwardDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            // 플레이어가 인식된다면
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.DrawLine(originPos, hitInfo.point, Color.red);

                target = hitInfo.collider.gameObject;
                state = EnemyState.ChasingPlayaer;
            }
            // Todo : 게임오버조건 레이어 추가 필요합니다
            // 게임 오버 조건이 인식된다면
            else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("게임오버조건"))
            {
                // 여기는 범위로 설정합시다. ForwardChecking 말고
                target = hitInfo.collider.gameObject;
                state = EnemyState.ChasingTarget;
            }
            // 아무것도 없다면
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
        // 현재 상태 : 랜덤 백터 4방향으로 이동
        // 목표 : 랜덤 벡터 4방향 중 하나 선택해서 쭉 이동, 벽이 생기면 다른 방향 벡터로 바꿔주기
        if (dir == Vector3.zero) dir = transform.forward;

        // 짧은 레이 발사
        // 벽에 닿으면 방향 전환 고고

        Vector3 originPos = muzzPoint.position;
        LayerMask layerMask = LayerMask.GetMask("SolidBlock", "Brick", "Enemy"); // 적들 서로, 벽만 체크 되도록--- 벽 레이어 추가되면 여기 추가
        if (Physics.Raycast(originPos, muzzPoint.forward, rayForwardDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(originPos, originPos + muzzPoint.forward * rayForwardDistance, Color.red);
            Debug.Log("벽에 닿음");
            // 4방향 중 하나 랜덤으로 반환
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


    // EnemyState에 따라서 이동로직 구분
}

public enum EnemyGrade
{
    normal, elite, boss // 추가 가능
}

public enum EnemyState
{
    General, ChasingPlayaer, ChasingTarget
}

