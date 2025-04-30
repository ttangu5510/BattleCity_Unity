using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    // : IDamagable, IMoveable(이건 플레이어 컨트롤러에서 받겠습니다)
    [Header("초기 설정")] // 맨 처음 게임을 시작할 때. 기본 설정을 입력하는 칸입니다.
    [SerializeField] private int life_Init;
    [SerializeField] private float moveSpeed_Init;
    [SerializeField] private float shotSpeed_Init;

    [Header("스테이지 별 스폰 포인트")]
    [SerializeField] private Transform respawnPoint;

    [Header("현재 플레이어 정보")]
    [SerializeField] public UpgradeType grade;
    [SerializeField] public PlayerState state;
    [SerializeField] private int life;
    [SerializeField] public float moveSpeed { get; private set; }
    [SerializeField] public float shotSpeed { get; private set; }
    [SerializeField] private int score;

    private PlayerData pd; // 등급 & 스코어 정보

    public bool isDamagable { get; private set; } // 피격 가능 상태 여부  (리스폰 중 무적, 아이템 사용으로 인한 무적 상태, 등등)

    // private Item itemPossession; 아이템을 소지할 수 있게 만들고 싶다면 사용
    // public UnityEvent PlayerDeadEvent = new UnityEvent(); 게임 오버 이벤트로만 해도 충분할 듯. 플레이어 사망 시 특수 참조 필요할 시 활성화


    // 스테이지마다 플레이어 오브젝트가 활성화 될 때, 플레이어 데이터를 동기화 시킴
    // 스테이지 진행 중에는 현재 객체가 정보를 담당. 스테이지 종료 시, PlayerData에 현재 객체 정보 저장하는 구조
    private void Awake()
    {
        // 씬 불러와지고 바로 시작할지, 스테이지 시작 이벤트 받고 시작할지 고민 중
        pd = PlayerData.Instance;

        DataInit(); // 임시/테스트용, 첫 시작 판정에서 해줘야함

        grade = pd.grade;
        life = pd.life;
        moveSpeed = pd.moveSpeed;
        shotSpeed = pd.shotSpeed;
        score = pd.score;

        // 스테이지매니저.스테이지 종료 이벤트.AddListener(SavePlayerData);
    }

    private void Start()
    {
        // 스테이지 종료 시 플레이어 데이터 저장
        StageManager.Instance.StageCloseEvent.AddListener(SavePlayerData);
    }

    private void OnDestroy()
    {
        // 스테이지매니저.스테이지 종료 이벤트.RemoveListener(SavePlayerData);
    }

    // 맨 처음 코인을 시작하는 시점(GameStart)에서만 호출       ***GameStart와 StageStart는 구분되어야 함. 
    private void DataInit()
    {
        // 초기 설정으로 저장
        pd.SaveData(life_Init, moveSpeed_Init, shotSpeed_Init, UpgradeType.normal, 0);
    }

    // 데미지 받음 => 죽음 판정
    public void TakeDamage()
    {
        if (grade > UpgradeType.normal) grade -= 1;
        else Dead();
    }

    // 죽음 => 게임오버 판정
    public void Dead()
    {
        // 라이프 감소
        life -= 1;

        // 라이프가 0 아래로 떨어지면 패배 조건 체크
        if (life <= 0)
        {
            //GameManager.Instance.GameOver();
            return;
        }
        // 라이프가 남았으면 리스폰
        else
        {
            Respawn();
        }
    }

    // 리스폰 => 초기 설정값으로 플레이어 초기화, 스폰 포인트로 위치 변경, 리스폰 효과 코루틴 실행
    public void Respawn()
    {
        // 플레이어 위치 이동
        transform.position = respawnPoint.position;

        // 플레이어 초기값으로 재설정
        moveSpeed = moveSpeed_Init;
        shotSpeed = shotSpeed_Init;
        grade = UpgradeType.normal;

        // 리스폰 이펙트 코루틴 실행
        StartCoroutine(RespawnEffect());
    }
    public IEnumerator RespawnEffect()
    {
        // 1초동안 효과 실행 or 셰이더 변경 후 코루틴 종료

        // 효과();
        // 셰이더();
        Debug.Log("플레이어 리스폰 중... 무적 상태!");
        yield return new WaitForSeconds(1f);
        // 효과 삭제();
        // 셰이더 초기화();
        Debug.Log("플레이어 리스폰 완료");
    }

    #region 아이템 & 환경 사용 호출 함수

    public void Upgrade(float moveSpeed, float shotSpeed)
    {
        if (grade == UpgradeType.boss)
        {
            Debug.Log("이미 최고 등급입니다");
            // 점수 얻는걸로? 슈퍼마리오 버섯 처럼 이미 최종 단계면 점수로 치환
            return;
        }
        SpeedControl(moveSpeed, shotSpeed);
        grade += 1;
    }

    public void GetLife(int life)
    {
        this.life += life;
    }

    public void SpeedControl(float moveSpeed, float shotSpeed)
    {
        this.moveSpeed += moveSpeed;
        this.shotSpeed += shotSpeed;
    }

    #endregion



    // 스테이지 종료 시 호출
    public void SavePlayerData()
    {
        pd.SaveData(life, moveSpeed, shotSpeed, grade, score);
    }
}

// 플레이어 & Enemy 업그레이드 등급
public enum UpgradeType { normal, elite, boss }

public enum PlayerState { General, Invincible } // {일반, 무적, ...상태가 더 필요하면 이곳에 추가}


