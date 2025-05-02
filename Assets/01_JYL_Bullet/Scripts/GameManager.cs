using System;
using System.Collections;
using System.Collections.Generic;
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
    private static GameManager instance;
    public static GameManager Instance
    #region 싱글톤 설정
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
        //TODO: 스테이지 만들고 추가
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

    // 싱글톤 필드
    private int lastStageNum;
    private Coroutine waitRoutine;
    private YieldInstruction waitSec;
    public ScoreBoard[] scores;
    // 씬 큐
    private Queue<string> stageSceneName;
    // 싱글톤 함수
    public void StageComplete()
    {
        // 남은 스테이지가 없을 경우
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
        // 씬 전환
        // 1. 점수 합산 창 씬 -> 2. 로딩창 함수 -> 3. 다음 스테이지 씬
        // 1. 점수 합산 창 씬 불러오기
        SceneManager.LoadSceneAsync("JYL_StageResultScene");
        // 1-1 로딩 결과 좀 보다가 넘어가기
        yield return waitSec;
        // 2. 로딩창 자동 - 씬의 이름을 넣어야함(해결)
        // 3. 다음 스테이지 씬 불러오기
        // 기존 씬 기준으로 다음 씬 판단이 필요함
        // 씬 큐가 필요 -> stageSceneName
        // TODO: Test ChangeScene by GameManager
        //SceneManager.LoadSceneAsync(stageSceneName.Dequeue());
        MySceneManager.Instance.ChangeScene(stageSceneName.Dequeue());
        // 플레이어 정보를 기준으로 다음 스테이지 씬에서 리스폰 포인트에 생성(씬 자체에 기능적으로 구현 되어 있음)
    }
    public void GameComplete()
    {
        StartCoroutine(GameCompleteRoutine());

    }
    IEnumerator GameCompleteRoutine()
    {
        // 마지막 스테이지 클리어 시 수행
        // 씬 전환 -> 1. 점수 합산 창 씬 -> 2. 게임 클리어(Congratulations) 씬 -> 3. 점수 씬 입력 (뉴레코드 일 때) -> 4. 레코드 씬 -> 5. 타이틀 씬
        // 초기화 작업
        // 스테이지 큐 다시 채우기

        // 1. 점수 합산 창
        SceneManager.LoadSceneAsync("JYL_StageResultScene");
        yield return waitSec;
        // 2. 게임 클리어 씬
        SceneManager.LoadSceneAsync("JYL_GameClearScene");
        yield return waitSec;
        // 3. 점수 입력 씬 (뉴레코드 일 때)
        // 이름과 점수를 게임매니저에 저장함
        // while로 이름 입력이 끝나거나, 카운트다운 코루틴이 끝났을 시 다음으로 진행
        if (PlayerData.Instance.score >= scores[9].score)
        {
            InputNewScore();
        }
        yield return waitSec;
        // 4. 레코드 씬
        // 이름과 점수를 1위부터 10위까지 나열해서 표시하는 씬
        SceneManager.LoadSceneAsync("JYL_RecordScene");
        yield return waitSec;
        SceneManager.LoadSceneAsync("TitleScene");
    }

    // 점수 합산 씬
    // 필요 요소

    // UI  각 적 모양마다 이미지 X 점수 = 합점수
    // 각 적 모양마다 이미지= StageManager.Instance.GetSlayeeEnemyCountByGrade(EnemyGrade grade) == 적 등급(grade)을 한 스테이지에서 잡은 숫자(int)
    // EnemyManager 싱글톤 스크립트 == 등급마다 점수, 아예 등급 X 점수 = 합점수 함수가 있음
    // 몬스터 갯수만큼
    // -----------
    // 최종점수



    public void GameOver()
    {
        // 이름을 스트링으로 입력받기
        // 게임 오버 시 수행
        // 게임 오버 UI -> 점수 합산 창 씬 -> 게임 오버 씬 -> 점수 씬(뉴레코드 일 때) -> 타이틀 씬
        // 1. 게임 오버 UI
        // 2. 점수 합산 창 씬
        // 3. 게임 오버 씬
        // 4. 점수 씬(뉴 레코드 일 때)
        // 5. 타이틀 씬
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
