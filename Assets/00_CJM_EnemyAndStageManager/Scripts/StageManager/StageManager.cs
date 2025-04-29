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

    [SerializeField] private int maxActiveEnemyCount;   // �� �� ���ÿ� ������ �� �ִ� �ִ� �� ��
    [SerializeField] private int enemyLifeCount;        // ���� ���� ���

    [SerializeField] private int sumedScore;

    [Header("���� ��� �� ����")]
    [SerializeField] private int normal_Point;
    [SerializeField] private int elite_Point;
    [SerializeField] private int boss_Point;

    public UnityEvent StageStartEvent;
    public UnityEvent StageCloseEvent;

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
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
        if (true /*�� �̸��� Stage�� ���ٸ�*/)
        {
            // �������� ���� �̺�Ʈ �߻�
            StageStartEvent?.Invoke();
            // �̰� �����ʴ� ��� �ʱ�ȭ�� �� ��� �ʿ�.
        }
    }

    public void StageClose()
    {
        StageCloseEvent?.Invoke();
        StageCloseEvent.RemoveAllListeners();
        
        // TODO : ���� ���������� �̵� (����ȯ���� �����ؾ��ϴ��� ��� ��)
    }

    public void StageDataInit()
    {
        // ����Ʈ �ʱ�ȭ
        activateEnemys = new List<Enemy>();
        slayedEnemys = new List<Enemy>();
    }

    // �������� �� �ҷ��� ��, StageData���� ����ȭ
    public void SynchronizeStageData(int maxActiveEnemyCount, int enemyLifeCount)
    {
        this.maxActiveEnemyCount = maxActiveEnemyCount;
        this.enemyLifeCount = enemyLifeCount;
    }

    // Enemy Spawn() �� ȣ��
    public void ActiveEnemyAdd(Enemy instance)
    {
        activateEnemys.Add(instance);   // Ȱ��ȭ�� �� ����Ʈ���� ����
    }

    // Enemy Dead() �� ȣ��
    public void ActiveEnemyListRemove(Enemy instance)
    {
        
        activateEnemys.Remove(instance);    // Ȱ��ȭ�� �� ����Ʈ���� ����
        slayedEnemys.Add(instance);         // ���� �� ����Ʈ�� �߰�

        enemyLifeCount -= 1;
        // �¸����� üũ
        if (enemyLifeCount <= 0)
        {
            // �¸�!
        }
    }


    // �������� ���� ��, óġ�� �� ��޺��� ���� ȯ��
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
