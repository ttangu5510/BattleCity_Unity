using CJM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IMovable
{
    [Header("���")]
    [SerializeField] private EnemyGrade grade;
    [HideInInspector] public EnemyGrade Grade { get { return grade; } }
    [Header("����")]
    [SerializeField] public EnemyState state;
    //[SerializeField] private GameObject target;

    [Header("���� ������")]
    [SerializeField] private Item item; // ���� ������

    [Header("���� (Ȯ�ο�)")]
    [SerializeField] protected int hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] protected float shotSpeed;
    [SerializeField] private int scorePoint;
    [SerializeField] protected float shotCycleRandomSeed_min;
    [SerializeField] protected float shotCycleRandomSeed_max;
    // Item itmePossession;  // ������ ���� ���� �߰�


    [Header("����")]
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

    public bool isDamagable { get; private set; } // �ǰ� ���� ���� ����  (������ �� ����, ������ ������� ���� ���� ����, ���)
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
        //state = EnemyState.General;

        sm.ActiveEnemyAdd(this);

        // ���� ����
        dir = body.forward;

        if (coroutine_Attack == null)
            coroutine_Attack = StartCoroutine(AttackCycle());

        // Ư�� �̵� ������ �����صθ� �׿� �´� �������� �̵� ����
        if (coroutine_MovePattern == null)
        {
            if (state == EnemyState.RandomMove) 
                coroutine_MovePattern = StartCoroutine(MovePattern_A(seedMin_DirChangeCycle, seedMax_DirChangeCycle));
            else if (state == EnemyState.ChasingTarget)
                coroutine_MovePattern = StartCoroutine(MovePattern_B());
            else if (state == EnemyState.General)
                coroutine_MovePattern = StartCoroutine(MovePattern_C(seedMin_DirChangeCycle, seedMax_DirChangeCycle));
        }

        // ������ �������̶�� => ������ ���� ȿ�� ����
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
        // �̵� ���� ���⿡
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

        // �ǰ� ����Ʈ instantiate
        GameObject explosion = Instantiate(explosionFBX);
        explosion.transform.position = body.transform.position;

        // �����ؾ��Ѵٸ� ��������Ʈ�� ��ġ,���� ���� �� ��Ȱ��ȭ
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

    #region �� �̵� ���� ����
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
        // 4���� �� �ϳ� �������� ��ȯ
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


        // Ÿ���� ������ �����ʿ� ���� ��
        if (target.position.x - enemyPos.position.x > 1)
        {
            if (exceptDir != Vector3.right)
                directions.Add(Vector3.right);
        }
        // Ÿ���� ������ �����ʿ� ���� ��
        else if (target.position.x - enemyPos.position.x < -1)
        {
            if (exceptDir != -Vector3.right)
                directions.Add(-Vector3.right);
        }
        // Ÿ���� ���� ���� x��ǥ�� ��
        else { }

        // Ÿ���� ������ ���� ���� ��
        if (target.position.z - enemyPos.position.z > 1)
        {
            if (exceptDir != Vector3.forward)
                directions.Add(Vector3.forward);
        }
        // Ÿ���� ������ �Ʒ��� ���� ��
        else if (target.position.z - enemyPos.position.z < -1)
        {
            if (exceptDir != -Vector3.forward)
                directions.Add(-Vector3.forward);
        }
        // Ÿ���� ���� ���� z��ǥ�� ��
        else { }

        if (directions.Count > 0)
        {
            //Debug.Log($"{directions.Count}��, exceptDir =  {exceptDir}");
            int R = Random.Range(0, directions.Count);
            DirSet(directions[R]);
        }
    }

    public void DirSetByWeight(float Chase_Weight, bool isBlocked)
    {
        //Debug.Log("ȸ��!");
        Transform target = em.gameOverFlag.transform;
        Transform enemyPos = body.transform;

        // 4���� �� �ϳ� �������� ��ȯ
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
        // Ÿ���� ������ �����ʿ� ���� ��
        if (target.position.x - enemyPos.position.x > 0)
        {
            if (blockedDir != Vector3.right)
                chaseDir.Add(Vector3.right);
            if (blockedDir != -Vector3.right)
                unchaseDir.Add(-Vector3.right);
        }
        // Ÿ���� ������ �����ʿ� ���� ��
        else if (target.position.x - enemyPos.position.x < 0)
        {
            if (blockedDir != -Vector3.right)
                chaseDir.Add(-Vector3.right);
            if (blockedDir != Vector3.right)
                unchaseDir.Add(Vector3.right);
        }
        // Ÿ���� ���� ���� x��ǥ�� ��
        else { }

        // Ÿ���� ������ ���� ���� ��
        if (target.position.z - enemyPos.position.z > 0)
        {
            if (blockedDir != Vector3.forward)
                chaseDir.Add(Vector3.forward);
            if (blockedDir != -Vector3.forward)
                unchaseDir.Add(-Vector3.forward);


        }
        // Ÿ���� ������ �Ʒ��� ���� ��
        else if (target.position.z - enemyPos.position.z < 0)
        {
            if (blockedDir != -Vector3.forward)
                chaseDir.Add(-Vector3.forward);
            if (blockedDir != Vector3.forward)
                unchaseDir.Add(Vector3.forward);
        }
        // Ÿ���� ���� ���� z��ǥ�� ��
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

    // �ڷ�ƾ���� �̵�
    IEnumerator MovePattern_A(float seed_min, float seed_max)
    {
        //Debug.Log("MovePattern_A �����");
        state = EnemyState.General;
        while (true)
        {
            if (onTriggerObj != null)
            {
                //Debug.Log("�� �浹 ������: " + onTriggerObj.name);
                RandomDirSet();
                //yield return new WaitUntil(() => onTriggerObj == null);
                yield return null;
            }
            else
            {
                float r = Random.Range(seed_min, seed_max);
                float t = 0f; // �ð� ����

                // �Ϲ� �̵� ���߿� �� ������
                while (t < r)
                {
                    // �� �ȿ��� ���������� ����
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

            // Ÿ�ٰ� ���� ���� ���� ��� ������ȯ (���� �ε����� ����, ����� �Ÿ��� ����� ����)
            if ((Mathf.Abs(target.x - enemyPos.x) <= 1f || Mathf.Abs(target.z - enemyPos.z) <= 1f) &&
                (target - enemyPos).magnitude < 4f)
            {
                //Debug.LogError("����! Ÿ�� ���� ����");
                if (isMoveFixed) { yield return null; continue; }
                ChaseDirSet(false);
                isMoveFixed = true;
                yield return null;
                continue;
            }

            // ���� �ε����� ��� ���� ��ȯ
            if (onTriggerObj != null)
            {
                //Debug.LogWarning("���� �浹!");
                ChaseDirSet(true);
                //yield return new WaitUntil(() => onTriggerObj == null);
                yield return null;
                continue;
            }

            float r = Random.Range(1f, 3f);
            float t = 0f;
            // �Ϲ� �̵� ���߿� �� ������
            while (t < r)
            {
                enemyPos = body.transform.position;

                // �� �ȿ��� ���������� ����
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

            // �⺻ ���� �̵�
            if (onTriggerObj == null && ((Mathf.Abs(target.x - enemyPos.x) <= 1f || Mathf.Abs(target.z - enemyPos.z) <= 1f) &&
                (target - enemyPos).magnitude < 4f) == false)
            {
                ChaseDirSet(false);
            }


        }
    }

    IEnumerator MovePattern_C(float seed_min, float seed_max)
    {
        //Debug.Log("MovePattern_C �����");
        state = EnemyState.General;
        while (true)
        {
            if (onTriggerObj != null)
            {
                //Debug.Log("�� �浹 ������: " + onTriggerObj.name);
                DirSetByWeight(Pattern_C_Weight, true);
                //yield return new WaitUntil(() => onTriggerObj == null);
                yield return null;

            }
            else
            {
                float r = Random.Range(seed_min, seed_max);
                float t = 0f; // �ð� ����

                // �Ϲ� �̵� ���߿� �� ������
                while (t < r)
                {
                    // �� �ȿ��� ���������� ����
                    if (onTriggerObj != null)
                    {
                        //Debug.Log("�̵� �� �浹 ������: " + onTriggerObj.name);
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

    #region �� ���� ���� ����

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
        /*if (moveType == MoveType.normal)
        {

        }
        else if (moveType == MoveType.sandSlow)
        {

        }*/
    }
    #endregion

    #region ������ ��� ����

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
    normal, normalFast, normalStrong, elite, boss // �߰� ����
}

public enum EnemyState
{
    General, RandomMove, ChasingTarget, Stop
}

