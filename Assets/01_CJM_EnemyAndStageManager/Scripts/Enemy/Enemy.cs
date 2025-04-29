using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [Header("등급")]
    [SerializeField] private EnemyGrade grade;
    [HideInInspector] public EnemyGrade Grade { get; }

    [Header("상태")]
    [SerializeField] private EnemyGrade state;
    [SerializeField] private int hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float shotSpeed;
    [SerializeField] private int scorePoint;
    // Item itmePossession;  // 아이템 소유 정보 추가

    [Header("포탄 발사 위치와 회전용 몸통")]
    [SerializeField] Transform muzzPoint;
    [SerializeField] Transform body;

    private StageManager sm;
    private EnemyManager em;
    private Rigidbody rb;

    public bool isDamagable { get; private set; } // 피격 가능 상태 여부  (리스폰 중 무적, 아이템 사용으로 인한 무적 상태, 등등)

    private void OnEnable()
    {
        sm = StageManager.Instance;
        em = EnemyManager.Instance;
        rb = GetComponent<Rigidbody>();

        em.StatSetting(out hp, out moveSpeed, out shotSpeed, out scorePoint, grade);
        state = EnemyGrade.normal;

        sm.ActiveEnemyAdd(this);
    }



    void Update()
    {
        // 이동 로직 여기에
        if (state == EnemyGrade.normal)
        {
            rb.velocity = transform.forward;
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
        Destroy(gameObject);
    }

    


    // EnemyState에 따라서 이동로직 구분


}

public enum EnemyGrade
{
    normal, elite, boss
}

public enum EnemyState
{
    Normal, ChasingPlayaer, ChasingTarget
}

