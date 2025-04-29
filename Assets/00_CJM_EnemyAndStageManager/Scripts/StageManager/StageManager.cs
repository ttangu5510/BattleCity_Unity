using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private static StageManager instance;
    public static StageManager Instance;

    private List<EnemySpawner> spawners;
    private List<Enemy> activateEnemys;
    private List<Enemy> slayedEnemys;

    [SerializeField] private int maxActiveEnemyCount;   // 맵 상에 동시에 존재할 수 있는 최대 적 수
    [SerializeField] private int enemyLifeCount;        // 남은 적의 목숨

    [SerializeField] private int sumedScore;

    [Header("몬스터 등급 별 점수")]
    [SerializeField] private int normal_Point;
    [SerializeField] private int elite_Point;
    [SerializeField] private int boss_Point;

    public UnityEvent StageStartEvent;
    public UnityEvent StageCloseEvent;

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

    public void StageStart(Scene scene, LoadSceneMode mode)
    {
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
        
        // TODO : 다음 스테이지로 이동 (씬전환에서 구현해야하는지 고민 중)
    }

    public void StageDataInit()
    {
        // 리스트 초기화
        activateEnemys = new List<Enemy>();
        slayedEnemys = new List<Enemy>();
    }

    // 스테이지 씬 불러올 때, StageData에서 동기화
    public void SynchronizeStageData(int maxActiveEnemyCount, int enemyLifeCount)
    {
        this.maxActiveEnemyCount = maxActiveEnemyCount;
        this.enemyLifeCount = enemyLifeCount;
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
            // 승리!
        }
    }


    // 스테이지 종료 시, 처치한 적 등급별로 점수 환산
    public void SumScore(out int enemy_Normal, out int enemy_Elite, out int enemy_Boss, out int sumedScore)
    {
        int normalSum = 0;
        int eliteSum = 0;
        int bossSum = 0;
        
        for (int i = 0; i < slayedEnemys.Count; i++)
        {
            if (slayedEnemys[i].grade == EnemyGrade.normal)
            {
                normalSum += normal_Point;
            }
            else if (slayedEnemys[i].grade == EnemyGrade.elite)
            {
                eliteSum += elite_Point;
            }
            else if (slayedEnemys[i].grade == EnemyGrade.elite)
            {
                bossSum += boss_Point;
            }
        }

        enemy_Normal = normalSum;
        enemy_Elite = eliteSum;
        enemy_Boss = bossSum;

        sumedScore = normalSum + eliteSum + bossSum;
        this.sumedScore = sumedScore;
    }

}
