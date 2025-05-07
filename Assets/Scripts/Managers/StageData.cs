using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageData : MonoBehaviour
{
    [Header("스테이지 이름")]
    [SerializeField] private string stageName;
    public string StageName { get { return stageName; } }
    //private StageManager sm;
    //private UIManager um;

    // 스테이지 데이터 자체를 프리펩으로 만들어서 StageManager 스스로 동기화하는 구성으로 바꾸자

    [Header("스테이지 조건 저장")]
    [Tooltip("맵 안에 동시에 존재할 수 있는 최대 적 수")]
    [SerializeField] private int maxActiveEnemyCount;    // 맵 상에 동시에 존재할 수 있는 최대 적 수
    public int MaxActiveEnemyCount { get { return maxActiveEnemyCount; } }
    [Tooltip("처치해야 되는 몬스터 수 / 승리 조건")]
    [SerializeField] private int enemyLifeCount;         // 처치해야 되는 몬스터 수 / 승리 조건
    public int EnemyLifeCount { get { return enemyLifeCount; } }

    /*private void Awake()
    {
        
    }

    // 스테이지 매니저 싱글톤 인스턴스가 Awake에서 할당되므로 Start에서 정보 전달
    private void OnEnable()
    {
        sm = StageManager.Instance;

        Debug.Log("스테이지 데이터 온에이블");
        // 스테이지 매니저 데이터 초기화 (이전 스테이지 적 리스트 정보)
        //sm.StageDataInit();
        // 스테이지 매니저에 현재 데이터 동기화
        //sm.SynchronizeStageData(maxActiveEnemyCount, enemyLifeCount);
    }

    private void Start()
    {
        um = UIManager.Instance;

        // UI에 처치 해야할 몬스터 수 정보 전달하기
        um.inGameUI_Instance.ShowEnemyLife();
    }*/
}
