using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance { get { return instance; } }

    [Header("[Normal] 등급 설정")]
    [SerializeField] public int hp_N;
    [SerializeField] public float moveSpeed_N;
    [SerializeField] public float shotSpeed_N;

    [Header("[Elite] 등급 설정")]
    [SerializeField] public int hp_E;
    [SerializeField] public float moveSpeed_E;
    [SerializeField] public float shotSpeed_E;


    [Header("[Boss] 등급 설정")]
    [SerializeField] public int hp_B;
    [SerializeField] public float moveSpeed_B;
    [SerializeField] public float shotSpeed_B;


    [Header("적 등급 별, 처치 점수")]
    [SerializeField] public int score_Normal;
    [SerializeField] public int score_Elite;
    [SerializeField] public int score_Boss;


    private void Awake()
    {
        // 싱글톤 인스턴스 생성
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StatSetting(out int hp, out float moveSpeed, out float shotSpeed, out int score, EnemyGrade grade)
    {
        switch (grade)
        {
            case EnemyGrade.normal:
                hp = hp_N;
                moveSpeed = moveSpeed_N;
                shotSpeed = shotSpeed_N;
                score = score_Normal;
                break;
            case EnemyGrade.elite:
                hp = hp_E;
                moveSpeed = moveSpeed_E;
                shotSpeed = shotSpeed_E;
                score = score_Elite;
                break;
            case EnemyGrade.boss:
                hp = hp_B;
                moveSpeed = moveSpeed_B;
                shotSpeed = shotSpeed_B;
                score = score_Boss;
                break;
            default:
                hp = 0;
                moveSpeed = 0;
                shotSpeed = 0;
                score = 0;
                break;
        }
    }

}
