using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
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

        for (int i = 0; i < standByGroup.childCount; i++)
        {
            standByGroup.GetChild(i).gameObject.SetActive(false);
        }

        // StandByGroup에 추가된 적 count와 스폰 시간을 정해준 적 count가 일치하는지 체크
        if (standByGroup.childCount != standByTimeToSpawn.Count)
            Debug.LogError($"StandByGroup에 추가된 적 count와 스폰 시간을 정해준 적 count가 일치하지 않습니다. \n오브젝트 이름 : {name}");
        else 
        {
            if (state == SpawnerState.Spawnable)
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

    // TODO:
    // 콜라이더로 스포너 영역 내 적/플레이어 탱크 들어오면 스폰 불가 상태로 변경? 필요하면 추가
    // 스포너 영역 내 아무도 없으면 스폰가능 상태로 변경
}

public enum SpawnerState { Spawning, Spawnable, Disable }
