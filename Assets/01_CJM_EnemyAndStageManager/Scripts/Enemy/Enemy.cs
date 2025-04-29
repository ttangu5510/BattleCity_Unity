using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [Header("���")]
    [SerializeField] private EnemyGrade grade;
    [HideInInspector] public EnemyGrade Grade { get; }

    [Header("����")]
    [SerializeField] private EnemyGrade state;
    [SerializeField] private int hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float shotSpeed;
    [SerializeField] private int scorePoint;
    // Item itmePossession;  // ������ ���� ���� �߰�

    [Header("��ź �߻� ��ġ�� ȸ���� ����")]
    [SerializeField] Transform muzzPoint;
    [SerializeField] Transform body;

    private StageManager sm;
    private EnemyManager em;
    private Rigidbody rb;

    public bool isDamagable { get; private set; } // �ǰ� ���� ���� ����  (������ �� ����, ������ ������� ���� ���� ����, ���)

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
        // �̵� ���� ���⿡
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

    


    // EnemyState�� ���� �̵����� ����


}

public enum EnemyGrade
{
    normal, elite, boss
}

public enum EnemyState
{
    Normal, ChasingPlayaer, ChasingTarget
}

