using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager Instance { get { return instance; } }


    [Header("Init Setting")] // �� ó�� ������ ������ ��. �⺻ ������ �Է��ϴ� ĭ�Դϴ�.
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
        // �̱��� �ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // �׽�Ʈ�� �ʱ�ȭ
        DataInit();
    }

    // �÷��̾� ��� �� �ʱ� �������� ������ �뵵
    public void PlayerInit()
    {
        this.grade = 0;
        this.state = 0;
        this.moveSpeed = MoveSpeed_Init;
        this.shotSpeed = ShotSpeed_Init;
    }
    
    // �÷��̾� ���� ���� �� ����� �� ��, ���ھ� �ʱ�ȭ �� �̾��ϱ� �뵵
    public void ScoreInit()
    {
        score = 0;
    }

    // Ÿ��Ʋ������ ���� �����ϴ� ��� �÷��̾� �����͸� �ʱ�ȭ �ϴ� �뵵
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

public enum PlayerState { General, Invincible, Respawning } // {�Ϲ�, ����, ...���°� �� �ʿ��ϸ� �̰��� �߰�}