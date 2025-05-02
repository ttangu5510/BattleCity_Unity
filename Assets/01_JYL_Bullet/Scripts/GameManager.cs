using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ���� ����� ���� ����ü
public struct ScoreBoard
{
    public string name;
    public int score;

}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    #region �̱��� ����
    {
        get
        {
            // Lazy Initialization
            if (instance == null)
            {
                GameObject go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        CreateGameManager();
        lastStageNum = 2;
        waitSec = new WaitForSeconds(2f);
        stageSceneName = new Queue<string>();
        // TODO: Test GameManager
        scores = new ScoreBoard[10];
        // TODO :Test GameManager
        for (int i = 1; i <= 2; i++)
        {
            stageSceneName.Enqueue($"JYL_STAGE {i}");
        }
        //TODO: �������� ����� �߰�
        //for(int i = 2; i <= lastStageNum;i++)
        //{
        //    //stageSceneName.Enqueue($"STAGE {i}");

        //}
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i].name = "BattleCity";
            scores[i].score = 500 * (i * i);
        }
    }
    public GameManager CreateGameManager()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        return instance;
    }
    #endregion

    // �̱��� �ʵ�
    private int lastStageNum;
    private Coroutine waitRoutine;
    private YieldInstruction waitSec;
    public ScoreBoard[] scores;
    // �� ť
    private Queue<string> stageSceneName;
    // �̱��� �Լ�
    public void StageComplete()
    {
        // ���� ���������� ���� ���
        if(stageSceneName.Count==0)
        {
            GameComplete();
        }
        else
        {
            StartCoroutine(StageCompleteRoutine());
        }
    }
    IEnumerator StageCompleteRoutine()
    {
        // �� ��ȯ
        // 1. ���� �ջ� â �� -> 2. �ε�â �Լ� -> 3. ���� �������� ��
        // 1. ���� �ջ� â �� �ҷ�����
        SceneManager.LoadSceneAsync("JYL_StageResultScene");
        // 1-1 �ε� ��� �� ���ٰ� �Ѿ��
        yield return waitSec;
        // 2. �ε�â �ڵ� - ���� �̸��� �־����(�ذ�)
        // 3. ���� �������� �� �ҷ�����
        // ���� �� �������� ���� �� �Ǵ��� �ʿ���
        // �� ť�� �ʿ� -> stageSceneName
        // TODO: Test ChangeScene by GameManager
        //SceneManager.LoadSceneAsync(stageSceneName.Dequeue());
        MySceneManager.Instance.ChangeScene(stageSceneName.Dequeue());
        // �÷��̾� ������ �������� ���� �������� ������ ������ ����Ʈ�� ����(�� ��ü�� ��������� ���� �Ǿ� ����)
    }
    public void GameComplete()
    {
        StartCoroutine(GameCompleteRoutine());

    }
    IEnumerator GameCompleteRoutine()
    {
        // ������ �������� Ŭ���� �� ����
        // �� ��ȯ -> 1. ���� �ջ� â �� -> 2. ���� Ŭ����(Congratulations) �� -> 3. ���� �� �Է� (�����ڵ� �� ��) -> 4. ���ڵ� �� -> 5. Ÿ��Ʋ ��
        // �ʱ�ȭ �۾�
        // �������� ť �ٽ� ä���

        // 1. ���� �ջ� â
        SceneManager.LoadSceneAsync("JYL_StageResultScene");
        yield return waitSec;
        // 2. ���� Ŭ���� ��
        SceneManager.LoadSceneAsync("JYL_GameClearScene");
        yield return waitSec;
        // 3. ���� �Է� �� (�����ڵ� �� ��)
        // �̸��� ������ ���ӸŴ����� ������
        // while�� �̸� �Է��� �����ų�, ī��Ʈ�ٿ� �ڷ�ƾ�� ������ �� �������� ����
        if (PlayerData.Instance.score >= scores[9].score)
        {
            InputNewScore();
        }
        yield return waitSec;
        // 4. ���ڵ� ��
        // �̸��� ������ 1������ 10������ �����ؼ� ǥ���ϴ� ��
        SceneManager.LoadSceneAsync("JYL_RecordScene");
        yield return waitSec;
        SceneManager.LoadSceneAsync("TitleScene");
    }

    // ���� �ջ� ��
    // �ʿ� ���

    // UI  �� �� ��縶�� �̹��� X ���� = ������
    // �� �� ��縶�� �̹���= StageManager.Instance.GetSlayeeEnemyCountByGrade(EnemyGrade grade) == �� ���(grade)�� �� ������������ ���� ����(int)
    // EnemyManager �̱��� ��ũ��Ʈ == ��޸��� ����, �ƿ� ��� X ���� = ������ �Լ��� ����
    // ���� ������ŭ
    // -----------
    // ��������



    public void GameOver()
    {
        // �̸��� ��Ʈ������ �Է¹ޱ�
        // ���� ���� �� ����
        // ���� ���� UI -> ���� �ջ� â �� -> ���� ���� �� -> ���� ��(�����ڵ� �� ��) -> Ÿ��Ʋ ��
        // 1. ���� ���� UI
        // 2. ���� �ջ� â ��
        // 3. ���� ���� ��
        // 4. ���� ��(�� ���ڵ� �� ��)
        // 5. Ÿ��Ʋ ��
    }

    
    public void SortScore()
    {
        Array.Sort(scores, (scoreA, scoreB) => scoreB.score.CompareTo(scoreA.score));
    }
    public void InputNewScore()
    {
        SceneManager.LoadSceneAsync("JYL_InputRecordScene");
    }
}
