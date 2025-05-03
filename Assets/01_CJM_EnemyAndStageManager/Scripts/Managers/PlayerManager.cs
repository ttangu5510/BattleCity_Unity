using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager Instance { get { return instance; } }


    [Header("Init Setting")] // 맨 처음 게임을 시작할 때. 기본 설정을 입력하는 칸입니다.
    [SerializeField] private int life_Init;
    public int Life_Init { get { return life_Init; } }

    [SerializeField] private float moveSpeed_Init;
    public float MoveSpeed_Init { get { return moveSpeed_Init; } }

    [SerializeField] private float shotSpeed_Init;
    public float ShotSpeed_Init { get { return shotSpeed_Init; } }

    [SerializeField] private float damagedCoolTime;
    public float DamagedCoolTime { get { return damagedCoolTime; } }

    [SerializeField] private float respawningTime;
    public float RespawningTime { get { return respawningTime; } }


    [Header("Current State")]
    [SerializeField] private int score;
    public int Score { get { return score; } }

    [SerializeField] private UpgradeType grade;
    public UpgradeType Grade { get { return grade; } }

    [SerializeField] private PlayerState state;
    public PlayerState State { get { return state; } }

    [SerializeField] private int life;
    public int Life { get { return life; } }

    [SerializeField] private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField] private float shotSpeed;
    public float ShotSpeed { get { return shotSpeed; } }

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

        // 테스트용 초기화
        DataInit();
    }

    // 플레이어 사망 후 초기 설정으로 리스폰 용도
    public void PlayerInit()
    {
        this.grade = 0;
        this.state = 0;
        this.moveSpeed = MoveSpeed_Init;
        this.shotSpeed = ShotSpeed_Init;
    }
    
    // 플레이어 게임 오버 후 재시작 할 때, 스코어 초기화 후 이어하기 용도
    public void ScoreInit()
    {
        score = 0;
    }

    // 타이틀씬에서 새로 시작하는 경우 플레이어 데이터를 초기화 하는 용도
    public void DataInit()
    {
        PlayerInit();
        ScoreInit();
        this.life = Life_Init;
    }


    public void CalculateLife(int life)
    {
        this.life += life;
    }

    public void SpeedControl(float moveSpeed, float shotSpeed)
    {
        this.moveSpeed += moveSpeed;
        this.shotSpeed += shotSpeed;
    }

    public void PlayerGradeUpdate(int a)
    {
        this.grade += a;
    }

    public void PlayerStateUpdate(PlayerState state)
    {
        this.state = state;
    }

    public void ScoreUpdate(int score)
    {
        this.score += score;
    }

    

}
public enum UpgradeType { Grade01, Grade02, Grade03, Grade04 }

public enum PlayerState { General, Invincible, Respawning } // {일반, 무적, ...상태가 더 필요하면 이곳에 추가}