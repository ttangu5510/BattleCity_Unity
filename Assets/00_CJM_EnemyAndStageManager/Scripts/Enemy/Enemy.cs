using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] public int hp;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float shotSpeed;
    [SerializeField] public EnemyGrade grade;
    // Item itmePossession;  // ������ ���� ���� �߰�

    [SerializeField] Transform muzzPoint;
    [SerializeField] Transform body;

    private StageManager sm;

    public bool isDamagable { get; private set; } // �ǰ� ���� ���� ����  (������ �� ����, ������ ������� ���� ���� ����, ���)

    private void Awake()
    {
        sm = StageManager.Instance;
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
