using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// 게임 기록을 담을 구조체
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


    private float time_AfterStageClear;  // 스테이지 클리어 이후 점수 합산 창 대기 시간
    private float time_AfterScoreSum;  // 점수 계산 완료 후 다음 스테이지로 넘어가기 전 대기시간
    private float time_ClearScene;     // 게임 클리어 씬 진행 시간
    private float time_RankingBoard;   // 랭킹보드 보여주는 시간


    #region 싱글톤 GM 생성
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
        time_AfterStageClear: 스테이지 클리어 이후 점수 합산 창 대기 시간
        time_AfterScoreSum  : 점수 계산 완료 후 다음 스테이지로 넘어가기 전 대기시간
        time_ClearScene     : 게임 클리어 씬 진행 시간
        time_RankingBoard   : 랭킹보드 보여주는 시간
        ********************************************/
        time_AfterStageClear = 1f; // 고정
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
        // 남은 스테이지가 없을 경우
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
            // TODO: 인게임 일시정지 UI 활성화, 이벤트로 연동
            state = GameState.InGamePause;
            Time.timeScale = 0;
        }
        else if (state == GameState.InGamePause)
        {
            // TODO: 인게임 일시정지 UI 비활성화, 이벤트로 연동
            state = GameState.InGameRun;
            Time.timeScale = 1;
        }
        else
        {
            Debug.LogWarning("일시정지 불가능 상태!");
        }
    }

    public void SortScore()
    {
        Array.Sort(scores, (scoreA, scoreB) => scoreB.score.CompareTo(scoreA.score));
    }


    public IEnumerator InGameCloseRoutine()
    {
        Debug.Log("CalculateScoreRoutine 실행");

        GameState currentState = state;

        if (currentState == GameState.InGameOver)
        {
            Debug.Log("게임오버씬 진행");
            state = GameState.OnGoUIPlayer;
            yield return new WaitUntil(() => state == GameState.CaculateScore);
            float a = Time.time;
            Debug.Log("창 다나옴");

            yield return new WaitForSecondsRealtime(1f);
            Debug.Log("대기완료");

            float b = Time.time;
            Debug.Log(a-b);

        }
        

        // 점수 합산 창 씬 시작
        state = GameState.CaculateScore;
        yield return new WaitForSecondsRealtime(time_AfterStageClear);    // 스테이지 클리어 이후 점수 합산 창 대기 시간

        SceneManager.LoadSceneAsync("StageResultScene", LoadSceneMode.Additive);

        yield return new WaitUntil(() => state == GameState.CaculateScore_Done);
        Debug.Log("스테이지 결과 종료");


        // 1. 단순 스테이지 클리어
        if (currentState == GameState.InGameRun)
        {
            Debug.Log("스테이지 클리어 코루틴 실행");

            yield return new WaitForSecondsRealtime(time_AfterScoreSum);    // 점수 계산 완료 후 다음 스테이지로 넘어가기 전 대기시간

            MySceneManager.Instance.ChangeScene($"STAGE {stageIndex}");
            stageIndex++;
        }
        // 2. 게임 오버 씬
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
            // Quit을 선택 했을 때
            else if (state == GameState.Quit)
            {
                StartCoroutine(InputRankingBoardRoutine());
            }
        }
        // 3. 게임 클리어 씬
        else if (currentState == GameState.InGameComplete)
        {
            SceneManager.LoadSceneAsync("GameClearScene");
            yield return new WaitForSecondsRealtime(time_ClearScene);    // 게임 클리어 씬 진행 시간
            StartCoroutine(InputRankingBoardRoutine());
        }


    }

    public IEnumerator InputRankingBoardRoutine()
    {
        // 뉴 레코드 일 때, InputRecordScene 불러오기
        if (PlayerManager.Instance.Score >= scores[9].score)
        {
            // 입력 대기 상태
            state = GameState.InputWaiting;
            SceneManager.LoadSceneAsync("InputRecordScene");

            // 입력 완료까지 대기, 입력 완료 시 => 입력 완료 상태
            yield return new WaitUntil(() => state == GameState.InputComplete);
        }

        // 입력 완료 시, 레코드 씬 잠깐 보여주기 (탑10 랭킹보드)
        SceneManager.LoadSceneAsync("RecordScene");
        yield return new WaitForSecondsRealtime(time_RankingBoard); // 랭킹보드 보여주는 시간


        // 다시 타이틀 씬으로 돌아오기
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
