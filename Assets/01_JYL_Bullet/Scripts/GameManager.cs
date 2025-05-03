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

    #region �̱��� ����, �ʵ�
    private static GameManager instance;
    public static GameManager Instance
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
        scores = new ScoreBoard[10];
        for (int i = 2; i <= lastStageNum; i++)
        {
            stageSceneName.Enqueue($"STAGE {i}");

        }
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i].name = "BattleCity";
            scores[i].score = 500 * ((i + 1) * (i + 1));
        }
        SortScore();
        isInput = false;
        isScored = false;
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

    // �̱��� �ʵ�
    private int lastStageNum;
    private Coroutine waitRoutine;
    private YieldInstruction waitSec;
    public ScoreBoard[] scores;
    public bool isInput;
    public bool isScored;
    

    // �� ť
    private Queue<string> stageSceneName;
    #endregion




    // �̱��� �Լ�
    public void StageComplete()
    {
        // ���� ���������� ���� ���
        if (stageSceneName.Count == 0)
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
        yield return waitSec;
        SceneManager.LoadSceneAsync("StageResultScene");
        while (!isScored)
        {
            yield return waitSec;
        }
        isScored = false;
        // 1-1 �ε� ��� �� ���ٰ� �Ѿ��
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
        SceneManager.LoadSceneAsync("StageResultScene");
        yield return waitSec;
        // 2. ���� Ŭ���� ��
        SceneManager.LoadSceneAsync("GameClearScene");
        yield return waitSec;
        yield return waitSec;
        // 3. ���� �Է� �� (�����ڵ� �� ��)
        // �̸��� ������ ���ӸŴ����� ������
        // while�� �̸� �Է��� �����ų�, ī��Ʈ�ٿ� �ڷ�ƾ�� ������ �� �������� ����
        StartCoroutine(InputNewScore());

        // 4. ���ڵ� �� ž10 ����
        SceneManager.LoadSceneAsync("RecordScene");
        yield return waitSec;
        // 5. Ÿ��Ʋ ������ ���� �� �ʱ�ȭ(�˾Ƽ� ��)
        MySceneManager.Instance.ChangeScene("TitleScene");
    }

    public void GameOver()
    {
        // �̸��� ��Ʈ������ �Է¹ޱ�
        // ���� ���� �� ����
        // ���� ���� UI -> ���� �ջ� â �� -> ���� ���� �� -> ���� ��(�����ڵ� �� ��) -> Ÿ��Ʋ ��
        // 1. ���� ���� UI
        // ���� ���� UI�� UIManager���� �����Ų��. 

        // 2. ���� �ջ� â ��
        // ���� ���� UI�� �����Ѵ�
        //UI
        SceneManager.LoadSceneAsync("StageResultScene");

        // 3. ���� ���� ��

        // 4. ���� ��(�� ���ڵ� �� ��)

        // 5. Ÿ��Ʋ ��

    }


    public void SortScore()
    {
        Array.Sort(scores, (scoreA, scoreB) => scoreB.score.CompareTo(scoreA.score));
    }

    IEnumerator InputNewScore()
    {
        if (PlayerData.Instance.score >= scores[9].score)
        {
            SceneManager.LoadSceneAsync("InputRecordScene");
        }
        while (!isScored)
        {
            yield return waitSec;
        }
        isScored = false;
    }
}
