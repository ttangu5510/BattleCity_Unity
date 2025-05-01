using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnerState state;
    [SerializeField] private Transform standByGroup;
    [SerializeField] private GameObject EffectPrefab;
    
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

    // TODO:
    // 콜라이더로 스포너 영역 내 적/플레이어 탱크 들어오면 스폰 불가 상태로 변경
    // 스포너 영역 내 아무도 없으면 스폰가능 상태로 변경
}

public enum SpawnerState { Spawning, Spawnable, Disable }
