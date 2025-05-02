using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IDamagable
{
    [SerializeField] private SpawnerState state;
    [SerializeField] private Transform standByGroup;
    [SerializeField] private GameObject EffectPrefab;
    [SerializeField] private List<float> standByTimeToSpawn;
    [SerializeField] private int standByIndex;

    Coroutine spPattern;

    private StageManager sm;

    private void Start()
    {
        sm = StageManager.Instance;
        sm.SpawnerAddToList(this);
        standByIndex = 0;

        // StandByGroup에 추가된 적 count와 스폰 시간을 정해준 적 count가 일치하는지 체크
        if (standByGroup.childCount != standByTimeToSpawn.Count)
            Debug.LogError($"StandByGroup에 추가된 적 count와 스폰 시간을 정해준 적 count가 일치하지 않습니다. \n오브젝트 이름 : {name}");
        else 
        {
            spPattern = StartCoroutine(SpawnPattern());
        } 
    }

    private void OnDestroy()
    {
        if (spPattern != null)
            StopCoroutine(spPattern);
    }

    IEnumerator SpawnPattern()
    {
        while (standByIndex < standByGroup.childCount)
        {
            if (Time.time >= standByTimeToSpawn[standByIndex])
            {
                standByGroup.GetChild(standByIndex).gameObject.SetActive(true);
                standByIndex += 1;
            }
            
            yield return null;
        }
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
