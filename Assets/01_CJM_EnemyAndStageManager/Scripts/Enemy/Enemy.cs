using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyGrade grade;

    [SerializeField] private int hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float shotSpeed;
    [SerializeField] private int scorePoint;
    // Item itmePossession;  // ������ ���� ���� �߰�

    [SerializeField] Transform muzzPoint;
    [SerializeField] Transform body;

    private StageManager sm;
    private EnemyManager em;

    public bool isDamagable { get; private set; } // �ǰ� ���� ���� ����  (������ �� ����, ������ ������� ���� ���� ����, ���)

    private void Start()
    {
        sm = StageManager.Instance;
        em = EnemyManager.Instance;

        em.StatSetting(out hp, out moveSpeed, out shotSpeed, out scorePoint, grade);
    }



    void Update()
    {
        // �̵� ���� ���⿡
        
    }

    public void Spawn()
    {
        // ���� ��ġ�� �θ� ������Ʈ�� Spanwer ��ġ
        Vector3 spawnPoint = transform.parent.position;

        sm.ActiveEnemyAdd(this);
        gameObject.SetActive(true);
    }

    public void Dead()
    {
        sm.ActiveEnemyListRemove(this);
        gameObject.SetActive(false);
    }

    #region �̵� ���� ����

    #endregion

}

public enum EnemyGrade
{
    normal, elite, boss
}
