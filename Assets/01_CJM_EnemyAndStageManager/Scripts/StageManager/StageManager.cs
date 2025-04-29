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

    private List<EnemySpawner> spawners;
    private List<Enemy> activateEnemys;
    private List<Enemy> slayedEnemys;

    [Header("�������� �⺻ ����")]
    [Tooltip("�� �ȿ� ���ÿ� ������ �� �ִ� �ִ� �� ��")]
    [SerializeField] private int maxActiveEnemyCount;   // �� �� ���ÿ� ������ �� �ִ� �ִ� �� ��
    [Tooltip("[�������� Ŭ����] ���� ���� ���� ������ ��")]
    [SerializeField] private int enemyLifeCount;        // ���� ���� ���
    [Tooltip("���� ������������ ���� ����")]
    [SerializeField] private int sumedScore;


    [HideInInspector] public UnityEvent StageStartEvent;
    [HideInInspector] public UnityEvent StageCloseEvent;

    private EnemyManager em;

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

    private void Start()
    {
        em = EnemyManager.Instance;
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
                normalSum += em.score_Normal;
            }
            else if (slayedEnemys[i].grade == EnemyGrade.elite)
            {
                eliteSum += em.score_Elite;
            }
            else if (slayedEnemys[i].grade == EnemyGrade.elite)
            {
                bossSum += em.score_Boss;
            }
        }

        enemy_Normal = normalSum;
        enemy_Elite = eliteSum;
        enemy_Boss = bossSum;

        sumedScore = normalSum + eliteSum + bossSum;
        this.sumedScore = sumedScore;
    }

}
