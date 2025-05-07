using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public enum InGameState { InGameRun, InGamePause }

public class StageManager : MonoBehaviour
{
    private static StageManager instance;
    public static StageManager Instance { get { return instance; } }

    private List<EnemySpawner> spawners;
    public List<Enemy> ActivateEnemys { get { return activateEnemys; } }
    private List<Enemy> activateEnemys;
    private List<Enemy> slayedEnemys;

    // 이거 리팩토링해야함. 일단 급해서 public으로 해두겠습니다.
    [HideInInspector] public Enemy_Boss boss;
    [HideInInspector] public List<Enemy_BossTurret> bossTurrets;
    [HideInInspector] public bool bossSlayed;


    [Header("Now Stage Info")]
    [Tooltip("Maximum count that can exist in map same time")]
    [SerializeField] private int maxActiveEnemyCount;   // 맵 상에 동시에 존재할 수 있는 최대 적 수
    [Tooltip("Count of lives of the enemy to clear")]
    public int enemyLifeCount;        // 남은 적의 목숨
    public int EnemyLifeCount { get { return enemyLifeCount; } }

    [Tooltip("Scores earned on the current stage")]
    private int sumedScore;

    [Header("Stages Setting List")]
    public List<StageData> stageDatas;
    private Dictionary<string, StageData> stageDatasDic;

    [Header("스테이지 관리")]
    [SerializeField] private int stageIndex;


    [HideInInspector] public UnityEvent StageStartEvent;
    [HideInInspector] public UnityEvent StageCloseEvent;

    private GameManager gm;
    private UIManager um;

    [HideInInspector] public BaseBlockSpawner baseBlock;
    //private InGameState inGameState;

    bool isStageOpen;

    #region 스테이지 데이터 동기화 및 시작 설정

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("STAGE"))
        {
            // 스테이지 정보 동기화
            StageDataInit();
            string key = scene.name;
            SynchronizeStageData(stageDatasDic[key].MaxActiveEnemyCount, stageDatasDic[key].EnemyLifeCount);
            
            isStageOpen = true;
            StageStartEvent?.Invoke();

            // 임시, 리팩토링 필요
            bossSlayed = true;

            if (gm != null) gm.state = GameState.InGameRun;
        }

        // 임시, 리팩토링 필요
        if (scene.name.Contains("Boss"))
        {
            bossSlayed = false;
        }
    }

    // 스테이지 씬 불러올 때, StageData에서 초기화
    public void StageDataInit()
    {
        // 리스트 초기화
        spawners = new List<EnemySpawner>();
        activateEnemys = new List<Enemy>();
        slayedEnemys = new List<Enemy>();
        bossTurrets = new List<Enemy_BossTurret>();
    }

    // 스테이지 씬 불러올 때, StageData에서 동기화
    public void SynchronizeStageData(int maxActiveEnemyCount, int enemyLifeCount)
    {
        this.maxActiveEnemyCount = maxActiveEnemyCount;
        this.enemyLifeCount = enemyLifeCount;
    }
    #endregion


    private void Awake()
    {
        // 싱글톤 인스턴스 생성
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // 스테이지 데이터 리스트 => 스테이지 데이터 딕셔너리로 넣어주기 (스테이지 이름을 키값으로 가짐)
            stageDatasDic = new Dictionary<string, StageData>();
            foreach (StageData data in stageDatas)
            {
                stageDatasDic[data.StageName] = data;
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("스테이지매니저 싱글톤 생성");

        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        gm = GameManager.Instance;
        um = UIManager.Instance;

        if (isStageOpen)
        {
            gm.state = GameState.InGameRun;
        }
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
        if (!isStageOpen) return;
        
        Debug.Log("스테이지 실패");
        // 이어서 진행할건지 여부 판단 후 안한다면 게임 오버 판정
        um.GameOverUIPlay();
        gm.GameOver();
        StageClose();
    }

    

    public void StageClose()
    {
        StageCloseEvent?.Invoke();
        StageCloseEvent.RemoveAllListeners();

        isStageOpen = false;
        
        // TODO : 스테이지 닫을 때,
        // 스테이지 클리어 상태면 -> 다음 스테이지로
        // 스테이지 실패 상태면 -> 게임 매니저.게임 오버 이벤트
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
        if (enemyLifeCount <= 0 && bossSlayed)
        {
            if(isStageOpen)
                StageClear();
        }

        // UI에 처치한 몬스터 반영하기
        um.inGameUI_Instance.ShowEnemyLife();
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
