using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public struct ScoreBoard
{
    public string name;
    public int score;

}
public class GameManager : MonoBehaviour
{
    private GameManager instance;
    public GameManager Instance 
    #region 싱글톤 설정
    {
        get
        {
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
        lastStageNum = 25;
        scores = new ScoreBoard[10];
        for(int i = 2; i <= lastStageNum;i++)
        {
            //TODO: 스테이지 만들고 추가
            //stageSceneName.Enqueue($"STAGE {i}");
        }
        for(int i =0;i<scores.Length;i++)
        {
            scores[i].name = $"BattleCity{i+1}";
            scores[i].score = 500*(i+1);
        }
    }
    // TODO : Test SortScore
    private void Start()
    {
        SortScore(scores);
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
        // 씬 전환
        // 1. 점수 합산 창 씬 -> 2. 로딩창 함수 -> 3. 다음 스테이지 씬
        // 1. 점수 합산 창 씬 불러오기
        SceneManager.LoadSceneAsync("점수 합산 창 씬");
        // 1-1 로딩 결과 좀 보다가 넘어가기
        WaitForSeconds(3f);
        // 2. 로딩창 자동 - 씬의 이름을 넣어야함(해결)
        // 3. 다음 스테이지 씬 불러오기
        // 기존 씬 기준으로 다음 씬 판단이 필요함
        // 씬 큐가 필요 -> stageSceneName
        MySceneManager.Instance.ChangeScene(stageSceneName.Dequeue());
        // 플레이어 정보를 기준으로 다음 스테이지 씬에서 리스폰 포인트에 생성(씬 자체에 기능적으로 구현 되어 있음)
    }

    public void GameComplete()
    {
        // 마지막 스테이지 클리어 시 수행
        // 씬 전환 -> 1. 점수 합산 창 씬 -> 2. 게임 클리어(Congratulations) 씬 -> 3. 점수 씬 입력 (뉴레코드 일 때) -> 4. 레코드 씬 -> 5. 타이틀 씬
        // 초기화 작업
        // 스테이지 큐 다시 채우기

        // 1. 점수 합산 창
        SceneManager.LoadSceneAsync("점수 합산 창 씬");
        WaitForSeconds(3f);
        // 2. 게임 클리어 씬
        SceneManager.LoadSceneAsync("게임 클리어 씬");
        // 3. 점수 입력 씬 (뉴레코드 일 때)
        SceneManager.LoadSceneAsync("점수 입력 씬");
        // 4. 레코드 씬
        SceneManager.LoadSceneAsync("레코드 씬");
        WaitForSeconds(3f);
    }

    public void GameOver()
    {
        // 게임 오버 시 수행
        // 게임 오버 UI -> 점수 합산 창 씬 -> 게임 오버 씬 -> 점수 씬(뉴레코드 일 때) -> 타이틀 씬
        // 1. 게임 오버 UI
        // 2. 점수 합산 창 씬
        // 3. 게임 오버 씬
        // 4. 점수 씬(뉴 레코드 일 때)
        // 5. 타이틀 씬
    }

    // Countinue


    private void WaitForSeconds(float sec)
    {
        waitSec = new WaitForSeconds(sec);
        if(waitRoutine == null)
        {
            waitRoutine = StartCoroutine(WaitSecondsRoutine());
        }
        else
        {
            StopCoroutine(waitRoutine);
            waitRoutine = null;
            waitRoutine = StartCoroutine(WaitSecondsRoutine());
        }
    }
    IEnumerator WaitSecondsRoutine()
    {
        yield return waitSec;
    }
    public void SortScore(ScoreBoard[] scores)
    {
        Array.Sort(scores,(scoreA,scoreB)=>scoreB.score.CompareTo(scoreA.score));
        for (int i = 0; i < scores.Length; i++)
        {
            Debug.Log($"{i+1}위 {scores[i].name}  점수: {scores[i].score}");
        }
    }
}
