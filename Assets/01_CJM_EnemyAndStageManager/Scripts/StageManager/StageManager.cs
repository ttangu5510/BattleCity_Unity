using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private static StageManager instance;
    public static StageManager Instance { get { return instance; } }

    [SerializeField] private List<EnemySpawner> spawners;
    [SerializeField] private List<Enemy> activateEnemys;
    [SerializeField] private List<Enemy> slayedEnemys;

    [Header("Now Stage Info")]
    [Tooltip("Maximum count that can exist in map same time")]
    [SerializeField] private int maxActiveEnemyCount;   // 맵 상에 동시에 존재할 수 있는 최대 적 수
    [Tooltip("Count of lives of the enemy to clear")]
    [SerializeField] private int enemyLifeCount;        // 남은 적의 목숨
    [Tooltip("Scores earned on the current stage")]
    [SerializeField] private int sumedScore;


    [HideInInspector] public UnityEvent StageStartEvent;
    [HideInInspector] public UnityEvent StageCloseEvent;

    private GameManager gm;

    private void Awake()
    {
        // 싱글톤 인스턴스 생성
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += StageStart;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        gm = GameManager.Instance;
    }

    public void StageClear()
    {
        // 클리어 UI 보여줌(점수 합산 장면)
        Debug.Log("스테이지 클리어");

        gm.StageComplete();
        

        StageClose();
    }

    public void StageFail()
    {
        Debug.Log("스테이지 실패");
        // 이어서 진행할건지 여부 판단 후 안한다면 게임 오버 판정

        StageClose();
    }

    public void StageStart(Scene scene, LoadSceneMode mode)
    {
        // Todo
        if (true /*씬 이름에 Stage가 들어간다면*/)
        {
            // 스테이지 시작 이벤트 발생
            StageStartEvent?.Invoke();
            // 이거 리스너는 어디서 초기화할 지 고민 필요.
        }
    }

    public void StageClose()
    {
        StageCloseEvent?.Invoke();
        StageCloseEvent.RemoveAllListeners();
        
        // TODO : 스테이지 닫을 때,
        // 스테이지 클리어 상태면 -> 다음 스테이지로
        // 스테이지 실패 상태면 -> 게임 매니저.게임 오버 이벤트
    }

    // 스테이지 씬 불러올 때, StageData에서 초기화
    public void StageDataInit()
    {
        // 리스트 초기화
        spawners = new List<EnemySpawner>();
        activateEnemys = new List<Enemy>();
        slayedEnemys = new List<Enemy>();
    }

    // 스테이지 씬 불러올 때, StageData에서 동기화
    public void SynchronizeStageData(int maxActiveEnemyCount, int enemyLifeCount)
    {
        this.maxActiveEnemyCount = maxActiveEnemyCount;
        this.enemyLifeCount = enemyLifeCount;
    }

    // 스테이지에 존재하는 EnemySpawner들을 리스트에 추가
    public void SpawnerAddToList(EnemySpawner spawner)
    {
        spawners.Add(spawner);
    }

    // Enemy Spawn() 시 호출
    public void ActiveEnemyAdd(Enemy instance)
    {
        activateEnemys.Add(instance);   // 활성화된 적 리스트에서 삭제
    }

    // Enemy Dead() 시 호출
    public void ActiveEnemyListRemove(Enemy instance)
    {
        
        activateEnemys.Remove(instance);    // 활성화된 적 리스트에서 삭제
        slayedEnemys.Add(instance);         // 죽은 적 리스트에 추가

        enemyLifeCount -= 1;
        // 승리조건 체크
        if (enemyLifeCount <= 0)
        {
            StageClear();
        }
    }

    public int GetSlayeeEnemyCountByGrade(EnemyGrade grade)
    {
        int count = 0;
        
        foreach (Enemy enemy in slayedEnemys)
        {
            if (enemy.Grade == grade)
                count += 1;
        }

        return count;
    }

    public bool GetSpawnable()
    {
        if (activateEnemys.Count >= maxActiveEnemyCount)
            return false;
        else
            return true;
    }

}
