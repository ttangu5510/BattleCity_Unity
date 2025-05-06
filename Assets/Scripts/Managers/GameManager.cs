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
    private int lastStageNum;
    public ScoreBoard[] scores;

    public GameState state;
    public int stageIndex = 2;


    private float time_AfterStageClear;  // �������� Ŭ���� ���� ���� �ջ� â ��� �ð�
    private float time_AfterScoreSum;  // ���� ��� �Ϸ� �� ���� ���������� �Ѿ�� �� ���ð�
    private float time_ClearScene;     // ���� Ŭ���� �� ���� �ð�
    private float time_RankingBoard;   // ��ŷ���� �����ִ� �ð�


    #region �̱��� GM ����
    private static GameManager instance;
    public static GameManager Instance /*{ get { return instance; } }*/
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
    public GameManager CreateSingleTonGM()
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

    private void Awake()
    {
        CreateSingleTonGM();

        lastStageNum = 2;
        scores = new ScoreBoard[10];
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i].name = "BattleCity";
            scores[i].score = 500 * ((i + 1) * (i + 1));
        }
        SortScore();

        /********************************************
        time_AfterStageClear: �������� Ŭ���� ���� ���� �ջ� â ��� �ð�
        time_AfterScoreSum  : ���� ��� �Ϸ� �� ���� ���������� �Ѿ�� �� ���ð�
        time_ClearScene     : ���� Ŭ���� �� ���� �ð�
        time_RankingBoard   : ��ŷ���� �����ִ� �ð�
        ********************************************/
        time_AfterStageClear = 1f; // ����
        time_AfterScoreSum = 1f;
        time_ClearScene = 2f;
        time_RankingBoard = 3f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamePauseToggle();
        }
    }

    public void StageComplete()
    {
        // ���� ���������� ���� ���
        if (stageIndex > lastStageNum)
        {
            GameComplete();
        }
        else
        {
            StartCoroutine(InGameCloseRoutine());
        }
    }

    public void GameComplete()
    {
        stageIndex = 2;
        state = GameState.InGameComplete;
        StartCoroutine(InGameCloseRoutine());
    }

    public void GameOver()
    {
        state = GameState.InGameOver;
        StartCoroutine(InGameCloseRoutine());
    }

    public void GamePauseToggle()
    {
        if (state == GameState.InGameRun)
        {
            // TODO: �ΰ��� �Ͻ����� UI Ȱ��ȭ, �̺�Ʈ�� ����
            state = GameState.InGamePause;
            Time.timeScale = 0;
        }
        else if (state == GameState.InGamePause)
        {
            // TODO: �ΰ��� �Ͻ����� UI ��Ȱ��ȭ, �̺�Ʈ�� ����
            state = GameState.InGameRun;
            Time.timeScale = 1;
        }
        else
        {
            Debug.LogWarning("�Ͻ����� �Ұ��� ����!");
        }
    }

    public void SortScore()
    {
        Array.Sort(scores, (scoreA, scoreB) => scoreB.score.CompareTo(scoreA.score));
    }


    public IEnumerator InGameCloseRoutine()
    {
        Debug.Log("CalculateScoreRoutine ����");

        GameState currentState = state;

        if (currentState == GameState.InGameOver)
        {
            Debug.Log("���ӿ����� ����");
            state = GameState.OnGoUIPlayer;
            yield return new WaitUntil(() => state == GameState.CaculateScore);
            float a = Time.time;
            Debug.Log("â �ٳ���");

            yield return new WaitForSecondsRealtime(1f);
            Debug.Log("���Ϸ�");

            float b = Time.time;
            Debug.Log(a-b);

        }
        

        // ���� �ջ� â �� ����
        state = GameState.CaculateScore;
        yield return new WaitForSecondsRealtime(time_AfterStageClear);    // �������� Ŭ���� ���� ���� �ջ� â ��� �ð�

        SceneManager.LoadSceneAsync("StageResultScene", LoadSceneMode.Additive);

        yield return new WaitUntil(() => state == GameState.CaculateScore_Done);
        Debug.Log("�������� ��� ����");


        // 1. �ܼ� �������� Ŭ����
        if (currentState == GameState.InGameRun)
        {
            Debug.Log("�������� Ŭ���� �ڷ�ƾ ����");

            yield return new WaitForSecondsRealtime(time_AfterScoreSum);    // ���� ��� �Ϸ� �� ���� ���������� �Ѿ�� �� ���ð�

            MySceneManager.Instance.ChangeScene($"STAGE {stageIndex}");
            stageIndex++;
        }
        // 2. ���� ���� ��
        else if (currentState == GameState.InGameOver)
        {
            SceneManager.LoadSceneAsync("GameOverScene");

            yield return new WaitUntil(() => state == GameState.Quit || state == GameState.Continue);

            if (state == GameState.Continue)
            {
                PlayerManager.Instance.DataInit();
                stageIndex--;
                MySceneManager.Instance.ChangeScene($"STAGE {stageIndex}");
                stageIndex++;
                state = GameState.InGameRun;
            }
            // Quit�� ���� ���� ��
            else if (state == GameState.Quit)
            {
                StartCoroutine(InputRankingBoardRoutine());
            }
        }
        // 3. ���� Ŭ���� ��
        else if (currentState == GameState.InGameComplete)
        {
            SceneManager.LoadSceneAsync("GameClearScene");
            yield return new WaitForSecondsRealtime(time_ClearScene);    // ���� Ŭ���� �� ���� �ð�
            StartCoroutine(InputRankingBoardRoutine());
        }


    }

    public IEnumerator InputRankingBoardRoutine()
    {
        // �� ���ڵ� �� ��, InputRecordScene �ҷ�����
        if (PlayerManager.Instance.Score >= scores[9].score)
        {
            // �Է� ��� ����
            state = GameState.InputWaiting;
            SceneManager.LoadSceneAsync("InputRecordScene");

            // �Է� �Ϸ���� ���, �Է� �Ϸ� �� => �Է� �Ϸ� ����
            yield return new WaitUntil(() => state == GameState.InputComplete);
        }

        // �Է� �Ϸ� ��, ���ڵ� �� ��� �����ֱ� (ž10 ��ŷ����)
        SceneManager.LoadSceneAsync("RecordScene");
        yield return new WaitForSecondsRealtime(time_RankingBoard); // ��ŷ���� �����ִ� �ð�


        // �ٽ� Ÿ��Ʋ ������ ���ƿ���
        PlayerManager.Instance.DataInit();
        SceneManager.LoadSceneAsync("TitleScene");
        state = GameState.Title;
        stageIndex = 2;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}

public enum GameState { Title, InGameRun, InGamePause, InGameOver, OnGoUIPlayer, InGameComplete, CaculateScore, CaculateScore_Done, Quit, Continue, InputWaiting, InputComplete }
