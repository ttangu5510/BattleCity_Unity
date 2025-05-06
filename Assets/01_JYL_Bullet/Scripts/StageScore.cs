using System.Collections;
using TMPro;
using UnityEngine;

public class StageScore : MonoBehaviour
{
    [SerializeField] TMP_Text[] enemyScore = new TMP_Text[6];

    // 적 처치 수
    int n_Enemy_Slayed;
    int nf_Enemy_Slayed;
    int ns_Enemy_Slayed;
    int e_Enemy_Slayed;
    int b_Enemy_Slayed;

    // 적 합산 점수
    int n_Enemy_ScoreSum;
    int nf_Enemy_ScoreSum;
    int ns_Enemy_ScoreSum;
    int e_Enemy_ScoreSum;
    int b_Enemy_ScoreSum;
    Coroutine ScoreRoutine;
    WaitForSecondsRealtime waitSec;
    private void Awake()
    {
        waitSec = new WaitForSecondsRealtime(0.3f);
        GetScore();
    }
    private void Start()
    {
        ScoreRoutine = StartCoroutine(ScoreAnimation());
    }

    private void GetScore()
    {
        n_Enemy_Slayed = StageManager.Instance.GetSlayeeEnemyCountByGrade(EnemyGrade.normal);
        nf_Enemy_Slayed = StageManager.Instance.GetSlayeeEnemyCountByGrade(EnemyGrade.normalFast);
        ns_Enemy_Slayed = StageManager.Instance.GetSlayeeEnemyCountByGrade(EnemyGrade.normalStrong);
        e_Enemy_Slayed = StageManager.Instance.GetSlayeeEnemyCountByGrade(EnemyGrade.elite);
        b_Enemy_Slayed = StageManager.Instance.GetSlayeeEnemyCountByGrade(EnemyGrade.boss);

        n_Enemy_ScoreSum = EnemyManager.Instance.GetSumPointByGrade(n_Enemy_Slayed, EnemyGrade.normal);
        nf_Enemy_ScoreSum = EnemyManager.Instance.GetSumPointByGrade(nf_Enemy_Slayed, EnemyGrade.normalFast);
        ns_Enemy_ScoreSum = EnemyManager.Instance.GetSumPointByGrade(ns_Enemy_Slayed, EnemyGrade.normalStrong);
        e_Enemy_ScoreSum = EnemyManager.Instance.GetSumPointByGrade(e_Enemy_Slayed, EnemyGrade.elite);
        b_Enemy_ScoreSum = EnemyManager.Instance.GetSumPointByGrade(b_Enemy_Slayed, EnemyGrade.boss);
    }
    IEnumerator ScoreAnimation()
    {
        // 텍스트들 초기화
        for (int i = 0; i < enemyScore.Length; i++)
        {
            enemyScore[i].text = "";
        }

        // 한 줄씩 잡은 몬스터들 점수 합산 출력
        for (int i = 0; i < n_Enemy_Slayed; i++)
        {
            enemyScore[0].text = $"[Normal Enemy] X {i + 1} = {EnemyManager.Instance.score_Normal * (i + 1)}";
            yield return waitSec;
        }
        for (int i = 0; i < nf_Enemy_Slayed; i++)
        {
            enemyScore[1].text = $"[NormalF Enemy] X {i + 1} = {EnemyManager.Instance.score_Normal_Fast * (i + 1)}";
            yield return waitSec;
        }
        for (int i = 0; i < ns_Enemy_Slayed; i++)
        {
            enemyScore[2].text = $"[NormalS Enemy] X {i + 1} = {EnemyManager.Instance.score_Normal_Strong * (i + 1)}";
            yield return waitSec;
        }
        for (int i = 0; i < e_Enemy_Slayed; i++)
        {
            enemyScore[3].text = $"[Elite Enemy] X {i + 1} = {EnemyManager.Instance.score_Elite * (i + 1)}";
            yield return waitSec;
        }
        for (int i = 0; i < b_Enemy_Slayed; i++)
        {
            enemyScore[4].text = $"[Boss Enemy] X {i + 1} = {EnemyManager.Instance.score_Boss * (i + 1)}";
            yield return waitSec;
        }
        yield return new WaitForSecondsRealtime(2f);
        enemyScore[5].text = $"Total = {n_Enemy_ScoreSum + nf_Enemy_ScoreSum + ns_Enemy_ScoreSum + e_Enemy_ScoreSum + b_Enemy_ScoreSum}";
        GameManager.Instance.state = GameState.CaculateScore_Done;
    }
}
//