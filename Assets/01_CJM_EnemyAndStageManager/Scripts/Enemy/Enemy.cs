using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IMovable
{
    [Header("등급")]
    [SerializeField] private EnemyGrade grade;
    [HideInInspector] public EnemyGrade Grade { get { return grade; } }
    [Header("상태")]
    [SerializeField] private EnemyState state;
    [SerializeField] private GameObject target;

    [Header("스펙 (확인용)")]
    [SerializeField] private int hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float shotSpeed;
    [SerializeField] private int scorePoint;
    [SerializeField] public float shotCycleRandomSeed_min;
    [SerializeField] public float shotCycleRandomSeed_max;
    // Item itmePossession;  // 아이템 소유 정보 추가

    [Header("세팅")]
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

    public bool isDamagable { get; private set; } // 피격 가능 상태 여부  (리스폰 중 무적, 아이템 사용으로 인한 무적 상태, 등등)
    public MoveType moveType { get; set; }

    private Vector3 rangeLevel;

    Coroutine coroutine_Attack;

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

        em.StatSetting(out hp, out moveSpeed, out shotSpeed, out scorePoint, out shotCycleRandomSeed_min, out shotCycleRandomSeed_max, grade);
        state = EnemyState.General;

        sm.ActiveEnemyAdd(this);

        // 시작 방향
        dir = body.forward;

        if (coroutine_Attack == null)
            coroutine_Attack = StartCoroutine(AttackCycle());
    }




    void Update()
    {
        //TargetChecking();

        // 이동 로직 여기에
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

    #region 적 이동 관련 로직
    public void RandomDirSet()
    {
        // 4방향 중 하나 랜덤으로 반환
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

        Vector3 right;
        if (Mathf.Abs(dir.x) > 0)
        {
            // 횡 이동 중
            right = transform.forward * 0.7f;
        }
        else
        {
            // 열 이동 중
            right = transform.right * 0.7f;
        }

        Vector3 originPos1 = muzzPoint.position + right;
        Vector3 originPos2 = muzzPoint.position - right;
        LayerMask layerMask = LayerMask.GetMask("SolidBlock", "Brick", "Enemy"); // 적들 서로, 벽만 체크 되도록--- 벽 레이어 추가되면 여기 추가
        if (Physics.Raycast(originPos1, muzzPoint.forward, rayForwardDistance, layerMask, QueryTriggerInteraction.Ignore) ||
            Physics.Raycast(originPos2, muzzPoint.forward, rayForwardDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(originPos1, originPos1 + muzzPoint.forward * rayForwardDistance, Color.red);
            Debug.DrawLine(originPos2, originPos2 + muzzPoint.forward * rayForwardDistance, Color.red);
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
            Debug.DrawLine(originPos1, originPos1 + muzzPoint.forward * rayForwardDistance, Color.green);
            Debug.DrawLine(originPos2, originPos2 + muzzPoint.forward * rayForwardDistance, Color.green);
        }

        Move(dir);
        Rotate(dir);
    }
    #endregion

    // 적 공격 로직
    // 랜덤 공격 코루틴 구현 고고
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

    // 바닥 타일 연관 함수
    public void MoveTypeUpdate()
    {
        // 움직임 타입에 따라 이펙트&효과 업데이트
        // 이펙트는 플레이어 프리펩 안에 자식 오브젝트 폴더로 정리
        // 효과는 케이스별로 수치만 변경하면 됨
    }

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

