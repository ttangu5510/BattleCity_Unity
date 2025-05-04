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

    #region 싱글톤 설정, 필드
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

    // 싱글톤 필드
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




    // 싱글톤 함수
    public void StageComplete()
    {
        // 남은 스테이지가 없을 경우
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

        // 1-1 로딩 결과 좀 보다가 넘어가기
        // 2. 로딩창 자동 - 씬의 이름을 넣어야함(해결)
        // 3. 다음 스테이지 씬 불러오기
        // 기존 씬 기준으로 다음 씬 판단이 필요함
        // 씬 큐가 필요 -> stageSceneName
        // TODO: Test ChangeScene by GameManager
        //SceneManager.LoadSceneAsync(stageSceneName.Dequeue());
        
        MySceneManager.Instance.ChangeScene($"STAGE {stageIndex}");
        stageIndex++;
        // 플레이어 정보를 기준으로 다음 스테이지 씬에서 리스폰 포인트에 생성(씬 자체에 기능적으로 구현 되어 있음)
    }

    public void GameComplete()
    {
        stageIndex = 2;
        StartCoroutine(GameCompleteRoutine());
    }
    IEnumerator GameCompleteRoutine()
    {
        // 마지막 스테이지 클리어 시 수행
        // 씬 전환 -> 1. 점수 합산 창 씬 -> 2. 게임 클리어(Congratulations) 씬 -> 3. 점수 씬 입력 (뉴레코드 일 때) -> 4. 레코드 씬 -> 5. 타이틀 씬
        // 초기화 작업
        // 스테이지 큐 다시 채우기

        // 1. 점수 합산 창
        yield return waitSec;
        SceneManager.LoadSceneAsync("StageResultScene");
        while (!isScored)
        {
            yield return null;
        }
        isScored = false;
        yield return waitSec;
        yield return waitSec;

        // 2. 게임 클리어 씬
        SceneManager.LoadSceneAsync("GameClearScene");
        yield return waitSec;
        yield return waitSec;

        // 3. 점수 입력 씬 (뉴레코드 일 때)
        // 이름과 점수를 게임매니저에 저장함
        // while로 이름 입력이 끝나거나, 카운트다운 코루틴이 끝났을 시 다음으로 진행
        if (PlayerManager.Instance.Score >= scores[9].score)
        {
            SceneManager.LoadSceneAsync("InputRecordScene");
            while (!isInput)
            {
                yield return null;
            }
            isInput = false;
        }
        // 4. 레코드 씬 탑10 나열
        SceneManager.LoadSceneAsync("RecordScene");

        yield return waitSec;
        // 5. 타이틀 씬으로 복귀 및 초기화(알아서 됨)
        PlayerManager.Instance.DataInit();
        SceneManager.LoadSceneAsync("TitleScene");
    }

    public void GameOver()
    {
        GameOverWaitRoutine = StartCoroutine(GameOverRoutine());
    }
    IEnumerator GameOverRoutine()
    {
        // 이름을 스트링으로 입력받기
        // 게임 오버 시 수행
        // 게임 오버 UI -> 점수 합산 창 씬 -> 게임 오버 씬 -> 점수 씬(뉴레코드 일 때) -> 레코드 씬 -> 타이틀 씬

        // 1. 게임 오버 UI
        // 게임 오버 UI는 UIManager에서 실행시킨다.
        UIManager.Instance.GameOverUIPlay();
        yield return waitSec;

        // 2. 점수 합산 창 씬
        yield return waitSec;
        SceneManager.LoadSceneAsync("StageResultScene");
        while (!isScored)
        {
            yield return null;
        }
        isScored = false;
        yield return waitSec;
        yield return waitSec;

        // 3. 게임 오버 씬
        SceneManager.LoadSceneAsync("GameOverScene");
        while (!isChoice)
        {
            yield return null;
        }
        isChoice = false;
        // Continue를 선택 했을 때

        // TODO : 선택> 플레이어 정보 초기화
        if (isContinue)
        {
            PlayerManager.Instance.DataInit();
            stageIndex--;
            MySceneManager.Instance.ChangeScene($"STAGE {stageIndex}");
            stageIndex++;
            isContinue = false;
        }
        // Quit을 선택 했을 때
        else
        {
            // 4. 점수 씬(뉴 레코드 일 때)
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
            // 5. 레코드 씬
            SceneManager.LoadSceneAsync("RecordScene");
            yield return waitSec;
            yield return waitSec;
            // 6. 타이틀 씬
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
