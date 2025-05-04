using System;
using System.Collections;
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
        lastStageNum = 3;
        waitSec = new WaitForSecondsRealtime(2f);
        scores = new ScoreBoard[10];
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i].name = "BattleCity";
            scores[i].score = 500 * ((i + 1) * (i + 1));
        }
        SortScore();
        isInput = false;
        isScored = false;
        isChoice = false;
        isContinue = false;
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
    public Coroutine GameOverWaitRoutine;
    private WaitForSecondsRealtime waitSec;
    public ScoreBoard[] scores;
    public bool isInput;
    public bool isScored;
    public bool isChoice;
    public bool isContinue;
    public int stageIndex = 2;

    #endregion




    // �̱��� �Լ�
    public void StageComplete()
    {
        // ���� ���������� ���� ���
        if (stageIndex > lastStageNum)
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
            yield return null;
        }
        isScored = false;
        yield return waitSec;
        yield return waitSec;

        // 1-1 �ε� ��� �� ���ٰ� �Ѿ��
        // 2. �ε�â �ڵ� - ���� �̸��� �־����(�ذ�)
        // 3. ���� �������� �� �ҷ�����
        // ���� �� �������� ���� �� �Ǵ��� �ʿ���
        // �� ť�� �ʿ� -> stageSceneName
        // TODO: Test ChangeScene by GameManager
        //SceneManager.LoadSceneAsync(stageSceneName.Dequeue());
        
        MySceneManager.Instance.ChangeScene($"STAGE {stageIndex}");
        stageIndex++;
        // �÷��̾� ������ �������� ���� �������� ������ ������ ����Ʈ�� ����(�� ��ü�� ��������� ���� �Ǿ� ����)
    }

    public void GameComplete()
    {
        stageIndex = 2;
        StartCoroutine(GameCompleteRoutine());
    }
    IEnumerator GameCompleteRoutine()
    {
        // ������ �������� Ŭ���� �� ����
        // �� ��ȯ -> 1. ���� �ջ� â �� -> 2. ���� Ŭ����(Congratulations) �� -> 3. ���� �� �Է� (�����ڵ� �� ��) -> 4. ���ڵ� �� -> 5. Ÿ��Ʋ ��
        // �ʱ�ȭ �۾�
        // �������� ť �ٽ� ä���

        // 1. ���� �ջ� â
        yield return waitSec;
        SceneManager.LoadSceneAsync("StageResultScene");
        while (!isScored)
        {
            yield return null;
        }
        isScored = false;
        yield return waitSec;
        yield return waitSec;

        // 2. ���� Ŭ���� ��
        SceneManager.LoadSceneAsync("GameClearScene");
        yield return waitSec;
        yield return waitSec;

        // 3. ���� �Է� �� (�����ڵ� �� ��)
        // �̸��� ������ ���ӸŴ����� ������
        // while�� �̸� �Է��� �����ų�, ī��Ʈ�ٿ� �ڷ�ƾ�� ������ �� �������� ����
        if (PlayerManager.Instance.Score >= scores[9].score)
        {
            SceneManager.LoadSceneAsync("InputRecordScene");
            while (!isInput)
            {
                yield return null;
            }
            isInput = false;
        }
        // 4. ���ڵ� �� ž10 ����
        SceneManager.LoadSceneAsync("RecordScene");

        yield return waitSec;
        // 5. Ÿ��Ʋ ������ ���� �� �ʱ�ȭ(�˾Ƽ� ��)
        PlayerManager.Instance.DataInit();
        SceneManager.LoadSceneAsync("TitleScene");
    }

    public void GameOver()
    {
        GameOverWaitRoutine = StartCoroutine(GameOverRoutine());
    }
    IEnumerator GameOverRoutine()
    {
        // �̸��� ��Ʈ������ �Է¹ޱ�
        // ���� ���� �� ����
        // ���� ���� UI -> ���� �ջ� â �� -> ���� ���� �� -> ���� ��(�����ڵ� �� ��) -> ���ڵ� �� -> Ÿ��Ʋ ��

        // 1. ���� ���� UI
        // ���� ���� UI�� UIManager���� �����Ų��.
        UIManager.Instance.GameOverUIPlay();
        yield return waitSec;

        // 2. ���� �ջ� â ��
        yield return waitSec;
        SceneManager.LoadSceneAsync("StageResultScene");
        while (!isScored)
        {
            yield return null;
        }
        isScored = false;
        yield return waitSec;
        yield return waitSec;

        // 3. ���� ���� ��
        SceneManager.LoadSceneAsync("GameOverScene");
        while (!isChoice)
        {
            yield return null;
        }
        isChoice = false;
        // Continue�� ���� ���� ��

        // TODO : ����> �÷��̾� ���� �ʱ�ȭ
        if (isContinue)
        {
            PlayerManager.Instance.DataInit();
            stageIndex--;
            MySceneManager.Instance.ChangeScene($"STAGE {stageIndex}");
            stageIndex++;
            isContinue = false;
        }
        // Quit�� ���� ���� ��
        else
        {
            // 4. ���� ��(�� ���ڵ� �� ��)
            if (PlayerManager.Instance.Score >= scores[9].score)
            {
                SceneManager.LoadSceneAsync("InputRecordScene");
                while (!isInput)
                {
                    yield return null;
                }
                isInput = false;
            }
            yield return waitSec;
            // 5. ���ڵ� ��
            SceneManager.LoadSceneAsync("RecordScene");
            yield return waitSec;
            yield return waitSec;
            // 6. Ÿ��Ʋ ��
            PlayerManager.Instance.DataInit();
            SceneManager.LoadSceneAsync("TitleScene");
            stageIndex = 2;
        }
    }

    public void SortScore()
    {
        Array.Sort(scores, (scoreA, scoreB) => scoreB.score.CompareTo(scoreA.score));
    }
}
