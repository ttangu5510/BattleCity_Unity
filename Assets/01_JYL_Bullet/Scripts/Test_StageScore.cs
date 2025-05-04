using System.Collections;
using TMPro;
using UnityEngine;

public class Test_StageScore : MonoBehaviour
{
    [SerializeField] TMP_Text[] enemyScore = new TMP_Text[4];
    int n_Enemy_Slayed;
    int e_Enemy_Slayed;
    int b_Enemy_Slayed;
    int n_Enemy_ScoreSum;
    int e_Enemy_ScoreSum;
    int b_Enemy_ScoreSum;
    Coroutine ScoreRoutine;
    YieldInstruction waitSec;
    private void Awake()
    {
        waitSec = new WaitForSeconds(0.5f);
        GetScore();
    }
    private void Start()
    {
        ScoreRoutine = StartCoroutine(ScoreAnimation());
    }

    private void GetScore()
    {
        n_Enemy_Slayed = StageManager.Instance.GetSlayeeEnemyCountByGrade(EnemyGrade.normal);
        e_Enemy_Slayed = StageManager.Instance.GetSlayeeEnemyCountByGrade(EnemyGrade.elite);
        b_Enemy_Slayed = StageManager.Instance.GetSlayeeEnemyCountByGrade(EnemyGrade.boss);

        n_Enemy_ScoreSum = EnemyManager.Instance.GetSumPointByGrade(n_Enemy_Slayed, EnemyGrade.normal);
        e_Enemy_ScoreSum = EnemyManager.Instance.GetSumPointByGrade(e_Enemy_Slayed, EnemyGrade.elite);
        b_Enemy_ScoreSum = EnemyManager.Instance.GetSumPointByGrade(b_Enemy_Slayed, EnemyGrade.boss);
    }
    IEnumerator ScoreAnimation()
    {
        for(int i = 0;i<3;i++)
        {
            enemyScore[i].text = "";
        }
        for (int i = 0; i < n_Enemy_Slayed; i++)
        {
            enemyScore[0].text = $"[Normal Enemy] X {i + 1} = {EnemyManager.Instance.score_Normal * (i + 1)}";
            yield return waitSec;
        }
        for (int i = 0; i < e_Enemy_Slayed; i++)
        {
            enemyScore[1].text = $"[Elite Enemy] X {i + 1} = {EnemyManager.Instance.score_Elite * (i + 1)}";
            yield return waitSec;
        }
        for (int i = 0; i < b_Enemy_Slayed; i++)
        {
            enemyScore[2].text = $"[Boss Enemy] X {i + 1} = {EnemyManager.Instance.score_Boss * (i + 1)}";
            yield return waitSec;
        }
        enemyScore[3].text = $"Total = {n_Enemy_ScoreSum + e_Enemy_ScoreSum + b_Enemy_ScoreSum}";
        GameManager.Instance.isScored = true;
    }
}
//