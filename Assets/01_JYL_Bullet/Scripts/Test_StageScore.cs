using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Test_StageScore : MonoBehaviour
{
    public TMP_Text[] enemyScore = new TMP_Text[3];
    int n_Enemy_Slayed;
    int e_Enemy_Slayed;
    int b_Enemy_Slayed;
    int n_Enemy_ScoreSum;
    int e_Enemy_ScoreSum;
    int b_Enemy_ScoreSum;
    private void Awake()
    {
        GetScore();
        enemyScore[0].text = $"[여기에 적 사진] X {n_Enemy_Slayed} = {n_Enemy_ScoreSum}";
        enemyScore[1].text = $"[여기에 적 사진] X {e_Enemy_Slayed} = {e_Enemy_ScoreSum}";
        enemyScore[2].text = $"[여기에 적 사진] X {b_Enemy_Slayed} = {b_Enemy_ScoreSum}";
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
}
