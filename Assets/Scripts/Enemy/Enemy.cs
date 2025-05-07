using CJM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IMovable
{
    [Header("등급")]
    [SerializeField] private EnemyGrade grade;
    [HideInInspector] public EnemyGrade Grade { get { return grade; } }
    [Header("상태")]
    [SerializeField] public EnemyState state;
    //[SerializeField] private GameObject target;

    [Header("보유 아이템")]
    [SerializeField] private Item item; // 보유 아이템

    [Header("스펙 (확인용)")]
    [SerializeField] protected int hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] protected float shotSpeed;
    [SerializeField] private int scorePoint;
    [SerializeField] protected float shotCycleRandomSeed_min;
    [SerializeField] protected float shotCycleRandomSeed_max;
    // Item itmePossession;  // 아이템 소유 정보 추가


    [Header("세팅")]
    [SerializeField] protected Transform muzzPoint;
    [SerializeField] private Transform body;
    [SerializeField] protected BulletObjectPool bulletPool;
    [SerializeField] protected GameObject explosionFBX;
    [SerializeField] protected GameObject itemPossessFBX;

    private StageManager sm;
    private EnemyManager em;
    private Rigidbody rb;

    [SerializeField] private Vector3 dir;
    private Vector3 rotDir;

    public bool isDamagable { get; private set; } // 피격 가능 상태 여부  (리스폰 중 무적, 아이템 사용으로 인한 무적 상태, 등등)
    public MoveType moveType { get; set; }

    private Vector3 rangeLevel;

    Coroutine coroutine_Attack;
    Coroutine coroutine_MovePattern;

    Coroutine coroutine_stopItem;

    [SerializeField] private float seedMin_DirChangeCycle;
    [SerializeField] private float seedMax_DirChangeCycle;

    [SerializeField] float Pattern_C_Weight;

    [HideInInspector] public GameObject onTriggerObj;


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
        //state = EnemyState.General;

        sm.ActiveEnemyAdd(this);

        // 시작 방향
        dir = body.forward;

        if (coroutine_Attack == null)
            coroutine_Attack = StartCoroutine(AttackCycle());

        // 특정 이동 패턴을 설정해두면 그에 맞는 패턴으로 이동 진행
        if (coroutine_MovePattern == null)
        {
            if (state == EnemyState.RandomMove) 
                coroutine_MovePattern = StartCoroutine(MovePattern_A(seedMin_DirChangeCycle, seedMax_DirChangeCycle));
            else if (state == EnemyState.ChasingTarget)
                coroutine_MovePattern = StartCoroutine(MovePattern_B());
            else if (state == EnemyState.General)
                coroutine_MovePattern = StartCoroutine(MovePattern_C(seedMin_DirChangeCycle, seedMax_DirChangeCycle));
        }

        // 아이템 보유중이라면 => 아이템 보유 효과 실행
        if (item != null)
        {

        }
    }

    IEnumerator ItemPossessFBXCycle()
    {
        GameObject fbx = Instantiate(itemPossessFBX, body.transform);

        Destroy(fbx);
        yield return null;
    }


    void Update()
    {
        // 이동 로직 여기에
        switch (state)
        {
            case EnemyState.General:
            case EnemyState.ChasingTarget:
                Move(dir);
                Rotate(rotDir);
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

        // 피격 이펙트 instantiate
        GameObject explosion = Instantiate(explosionFBX);
        explosion.transform.position = body.transform.position;

        // 재사용해야한다면 스폰포인트로 위치,각도 조정 후 비활성화
        gameObject.SetActive(false);
        PlayerManager.Instance.ScoreUpdate(scorePoint);

        if (item != null)
        {
            /*Instantiate(item);
            item.transform.position = body.transform.position;*/
            item.DropItem(body.transform.position);
        }

        StopCoroutine(coroutine_Attack);
        StopCoroutine(coroutine_MovePattern);
    }
    private void OnDisable()
    {
        if (coroutine_Attack != null)
            StopCoroutine(coroutine_Attack);
        if (coroutine_MovePattern != null)
            StopCoroutine(coroutine_MovePattern);
    }

    #region 적 이동 관련 로직
    public void DirSet(Vector3 dir)
    {
        rotDir = dir;

        if (moveType == MoveType.iceSlide)
        {
            if (rb.velocity.magnitude < 0.1f) this.dir = rotDir;
            else return;
        }
        else this.dir = rotDir;
    }

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

        DirSet(directions[R]);
    }

    public void ChaseDirSet(bool isBlocked)
    {
        Transform target = em.gameOverFlag.transform;
        Transform enemyPos = body.transform;
        List<Vector3> directions = new List<Vector3>();

        Vector3 exceptDir = Vector3.zero;
        if (isBlocked)
        {
            exceptDir = enemyPos.forward;
        }


        // 타겟이 적보다 오른쪽에 있을 때
        if (target.position.x - enemyPos.position.x > 1)
        {
            if (exceptDir != Vector3.right)
                directions.Add(Vector3.right);
        }
        // 타겟이 적보다 오른쪽에 있을 때
        else if (target.position.x - enemyPos.position.x < -1)
        {
            if (exceptDir != -Vector3.right)
                directions.Add(-Vector3.right);
        }
        // 타겟이 적과 같은 x좌표일 때
        else { }

        // 타겟이 적보다 위에 있을 때
        if (target.position.z - enemyPos.position.z > 1)
        {
            if (exceptDir != Vector3.forward)
                directions.Add(Vector3.forward);
        }
        // 타겟이 적보다 아래에 있을 때
        else if (target.position.z - enemyPos.position.z < -1)
        {
            if (exceptDir != -Vector3.forward)
                directions.Add(-Vector3.forward);
        }
        // 타겟이 적과 같은 z좌표일 때
        else { }

        if (directions.Count > 0)
        {
            //Debug.Log($"{directions.Count}개, exceptDir =  {exceptDir}");
            int R = Random.Range(0, directions.Count);
            DirSet(directions[R]);
        }
    }

    public void DirSetByWeight(float Chase_Weight, bool isBlocked)
    {
        //Debug.Log("회전!");
        Transform target = em.gameOverFlag.transform;
        Transform enemyPos = body.transform;

        // 4방향 중 하나 랜덤으로 반환
        Vector3[] directions = new Vector3[]
        {
                Vector3.forward,
                -Vector3.forward,
                Vector3.right,
                -Vector3.right
        };

        List<Vector3> chaseDir = new List<Vector3>(); ;
        List<Vector3> unchaseDir = new List<Vector3>(); ;
        Vector3 blockedDir = Vector3.zero;
        if (isBlocked)
        {
            blockedDir = enemyPos.forward;
        }
        // 타겟이 적보다 오른쪽에 있을 때
        if (target.position.x - enemyPos.position.x > 0)
        {
            if (blockedDir != Vector3.right)
                chaseDir.Add(Vector3.right);
            if (blockedDir != -Vector3.right)
                unchaseDir.Add(-Vector3.right);
        }
        // 타겟이 적보다 오른쪽에 있을 때
        else if (target.position.x - enemyPos.position.x < 0)
        {
            if (blockedDir != -Vector3.right)
                chaseDir.Add(-Vector3.right);
            if (blockedDir != Vector3.right)
                unchaseDir.Add(Vector3.right);
        }
        // 타겟이 적과 같은 x좌표일 때
        else { }

        // 타겟이 적보다 위에 있을 때
        if (target.position.z - enemyPos.position.z > 0)
        {
            if (blockedDir != Vector3.forward)
                chaseDir.Add(Vector3.forward);
            if (blockedDir != -Vector3.forward)
                unchaseDir.Add(-Vector3.forward);


        }
        // 타겟이 적보다 아래에 있을 때
        else if (target.position.z - enemyPos.position.z < 0)
        {
            if (blockedDir != -Vector3.forward)
                chaseDir.Add(-Vector3.forward);
            if (blockedDir != Vector3.forward)
                unchaseDir.Add(Vector3.forward);
        }
        // 타겟이 적과 같은 z좌표일 때
        else { }


        float percent = Random.Range(0, 100);


        //Debug.Log($"{chaseDir.Count}, {unchaseDir.Count}");
        if (percent <= Chase_Weight)
        {
            if (chaseDir.Count <= 0) return;
            int r_chase = Random.Range(0, chaseDir.Count);
            DirSet(chaseDir[r_chase]);
        }
        else
        {
            if (unchaseDir.Count <= 0) return;
            int r_unchase = Random.Range(0, unchaseDir.Count);
            DirSet(unchaseDir[r_unchase]);
        }

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

    /*IEnumerator MovePattern_Switching(float A_Time, float B_Time)
    {
        while (true)
        {
            coroutine_MovePattern = StartCoroutine(MovePattern_A(seedMin_DirChangeCycle, seedMax_DirChangeCycle));
            yield return new WaitForSeconds(A_Time);
            StopCoroutine(MovePattern_A(seedMin_DirChangeCycle, seedMax_DirChangeCycle));

            coroutine_MovePattern = StartCoroutine(MovePattern_B(seedMin_DirChangeCycle, seedMax_DirChangeCycle));
            yield return new WaitForSeconds(B_Time);
            StopCoroutine(MovePattern_B(seedMin_DirChangeCycle, seedMax_DirChangeCycle));
        }
    }*/

    // 코루틴으로 이동
    IEnumerator MovePattern_A(float seed_min, float seed_max)
    {
        //Debug.Log("MovePattern_A 실행됨");
        state = EnemyState.General;
        while (true)
        {
            if (onTriggerObj != null)
            {
                //Debug.Log("벽 충돌 감지됨: " + onTriggerObj.name);
                RandomDirSet();
                //yield return new WaitUntil(() => onTriggerObj == null);
                yield return null;
            }
            else
            {
                float r = Random.Range(seed_min, seed_max);
                float t = 0f; // 시간 감지

                // 일반 이동 도중에 벽 감지용
                while (t < r)
                {
                    // 이 안에서 지속적으로 감시
                    if (onTriggerObj != null)
                    {
                        //RandomDirSet();
                        break;
                    }

                    t += Time.deltaTime;
                    yield return null;
                }

                if (onTriggerObj == null)
                    RandomDirSet();
            }

            yield return null;
        }
    }

    IEnumerator MovePattern_B()
    {
        state = EnemyState.ChasingTarget;
        bool isMoveFixed = false;
        Vector3 target = em.gameOverFlag.transform.position;
        while (true)
        {
            Vector3 enemyPos = body.transform.position;

            // 타겟과 동일 선상에 오면 즉시 방향전환 (벽에 부딪혀도 직진, 충분히 거리가 가까울 때만)
            if ((Mathf.Abs(target.x - enemyPos.x) <= 1f || Mathf.Abs(target.z - enemyPos.z) <= 1f) &&
                (target - enemyPos).magnitude < 4f)
            {
                //Debug.LogError("근접! 타겟 고정 접근");
                if (isMoveFixed) { yield return null; continue; }
                ChaseDirSet(false);
                isMoveFixed = true;
                yield return null;
                continue;
            }

            // 벽에 부딪히면 즉시 방향 전환
            if (onTriggerObj != null)
            {
                //Debug.LogWarning("벽에 충돌!");
                ChaseDirSet(true);
                //yield return new WaitUntil(() => onTriggerObj == null);
                yield return null;
                continue;
            }

            float r = Random.Range(1f, 3f);
            float t = 0f;
            // 일반 이동 도중에 벽 감지용
            while (t < r)
            {
                enemyPos = body.transform.position;

                // 이 안에서 지속적으로 감시
                if (onTriggerObj != null)
                {
                    //ChaseDirSet(true);
                    break;
                }
                if ((Mathf.Abs(target.x - enemyPos.x) <= 1f || Mathf.Abs(target.z - enemyPos.z) <= 1f) &&
                (target - enemyPos).magnitude < 4f)
                {
                    //ChaseDirSet(true);
                    break;
                }

                t += Time.deltaTime;
                yield return null;
            }

            // 기본 추적 이동
            if (onTriggerObj == null && ((Mathf.Abs(target.x - enemyPos.x) <= 1f || Mathf.Abs(target.z - enemyPos.z) <= 1f) &&
                (target - enemyPos).magnitude < 4f) == false)
            {
                ChaseDirSet(false);
            }


        }
    }

    IEnumerator MovePattern_C(float seed_min, float seed_max)
    {
        //Debug.Log("MovePattern_C 실행됨");
        state = EnemyState.General;
        while (true)
        {
            if (onTriggerObj != null)
            {
                //Debug.Log("벽 충돌 감지됨: " + onTriggerObj.name);
                DirSetByWeight(Pattern_C_Weight, true);
                //yield return new WaitUntil(() => onTriggerObj == null);
                yield return null;

            }
            else
            {
                float r = Random.Range(seed_min, seed_max);
                float t = 0f; // 시간 감지

                // 일반 이동 도중에 벽 감지용
                while (t < r)
                {
                    // 이 안에서 지속적으로 감시
                    if (onTriggerObj != null)
                    {
                        //Debug.Log("이동 중 충돌 감지됨: " + onTriggerObj.name);
                        break;
                    }

                    t += Time.deltaTime;
                    yield return null;
                }

                if (onTriggerObj == null)
                    DirSetByWeight(Pattern_C_Weight, false);
            }

            yield return null;
        }
    }



    #endregion

    #region 적 공격 관련 로직

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
            if (state == EnemyState.Stop) yield return new WaitUntil(() => state != EnemyState.Stop);
            Attack();
        }
    }

    // 바닥 타일 연관 함수
    public void MoveTypeUpdate()
    {
        // 움직임 타입에 따라 이펙트&효과 업데이트
        // 이펙트는 플레이어 프리펩 안에 자식 오브젝트 폴더로 정리
        // 효과는 케이스별로 수치만 변경하면 됨
        /*if (moveType == MoveType.normal)
        {

        }
        else if (moveType == MoveType.sandSlow)
        {

        }*/
    }
    #endregion

    #region 아이템 사용 관련

    public void TimeStopItemEffect(float duration)
    {
        if (coroutine_stopItem == null)
            coroutine_stopItem = StartCoroutine(TimeStopItemEffectCycle(duration));
        else
        {
            StopCoroutine(coroutine_stopItem);
            coroutine_stopItem = StartCoroutine(TimeStopItemEffectCycle(duration));
        }
    }
    IEnumerator TimeStopItemEffectCycle(float duration)
    {
        state = EnemyState.Stop;
        yield return new WaitForSeconds(duration);
        state = EnemyState.General;
    }

    #endregion
}

public enum EnemyGrade
{
    normal, normalFast, normalStrong, elite, boss // 추가 가능
}

public enum EnemyState
{
    General, RandomMove, ChasingTarget, Stop
}

