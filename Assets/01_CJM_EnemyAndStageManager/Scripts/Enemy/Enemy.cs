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
    // Item itmePossession;  // 아이템 소유 정보 추가

    [SerializeField] Transform muzzPoint;
    [SerializeField] Transform body;

    private StageManager sm;
    private EnemyManager em;

    public bool isDamagable { get; private set; } // 피격 가능 상태 여부  (리스폰 중 무적, 아이템 사용으로 인한 무적 상태, 등등)

    private void Start()
    {
        sm = StageManager.Instance;
        em = EnemyManager.Instance;

        em.StatSetting(out hp, out moveSpeed, out shotSpeed, out scorePoint, grade);
    }



    void Update()
    {
        // 이동 로직 여기에
        
    }

    public void Spawn()
    {
        // 스폰 위치는 부모 오브젝트인 Spanwer 위치
        Vector3 spawnPoint = transform.parent.position;

        sm.ActiveEnemyAdd(this);
        gameObject.SetActive(true);
    }

    public void Dead()
    {
        sm.ActiveEnemyListRemove(this);
        gameObject.SetActive(false);
    }

    #region 이동 로직 정리

    #endregion

}

public enum EnemyGrade
{
    normal, elite, boss
}
