using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IDamagable
{
    [SerializeField] private SpawnerState state;
    [SerializeField] private Transform standByGroup;
    [SerializeField] private GameObject EffectPrefab;
    [SerializeField] private List<GameObject> standByEnemys;
    
    private StageManager sm;

    private void Start()
    {
        sm = StageManager.Instance;
        sm.SpawnerAddToList(this);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEnemy();
        }
    }
    

    public void SpawnEnemy()
    {
        // 자식 활성화
        if (!standByGroup.GetChild(0).gameObject.activeSelf)
            standByGroup.GetChild(0).gameObject.SetActive(true);
    }

    public void TakeDamage() 
    {
        // 어차피 콜라이더 없어서 직접적인 충돌은 없고
        // 자식 오브젝트로 들어갈 Enemy들의 Damagable을 전달하는 방식
        IDamagable damagable = standByGroup.GetChild(0).gameObject.GetComponent<Enemy>();
        damagable?.TakeDamage();
    }

    // TODO:
    // 콜라이더로 스포너 영역 내 적/플레이어 탱크 들어오면 스폰 불가 상태로 변경
    // 스포너 영역 내 아무도 없으면 스폰가능 상태로 변경
}

public enum SpawnerState { Spawning, Spawnable, Disable }
