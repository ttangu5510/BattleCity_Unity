using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable, IMovable
{
    // : IDamagable, IMoveable(이건 플레이어 컨트롤러에서 받겠습니다)
    [Header("Init Setting")] // 맨 처음 게임을 시작할 때. 기본 설정을 입력하는 칸입니다.
    [SerializeField] private int life_Init;
    [SerializeField] private float moveSpeed_Init;
    [SerializeField] private float shotSpeed_Init;
    [Tooltip("Waiting time to respawn")]
    [SerializeField] private float DamagedCoolTime;
    [SerializeField] private float respawningTime;


    [SerializeField] private RespawnPoint respawnPoint;

    [Header("Current Player Data")]
    [SerializeField] public UpgradeType grade;
    [SerializeField] public PlayerState state;
    [SerializeField] private int life;
    [SerializeField] public int Life { get { return life; } }
    [SerializeField] public float moveSpeed { get; private set; }
    [SerializeField] public float shotSpeed { get; private set; }
    [SerializeField] public int score { get; private set; }

    [Header("Setting Field")]
    [SerializeField] private Transform groupRender;
    [SerializeField] private PlayerController playerController;




    private PlayerData pd; // 등급 & 스코어 정보

    public MoveType moveType { get; set; }

    // private Item itemPossession; 아이템을 소지할 수 있게 만들고 싶다면 사용
    // public UnityEvent PlayerDeadEvent = new UnityEvent(); 게임 오버 이벤트로만 해도 충분할 듯. 플레이어 사망 시 특수 참조 필요할 시 활성화

    //*******************************************************//
    // 테스트용. 테스트 후 삭제할 예정
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Upgrade(0, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TakeDamage();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log( StageManager.Instance.GetSlayeeEnemyCountByGrade(EnemyGrade.normal));
        }
    }
    //*****************************************************//

    // 스테이지마다 플레이어 오브젝트가 활성화 될 때, 플레이어 데이터를 동기화 시킴
    // 스테이지 진행 중에는 현재 객체가 정보를 담당. 스테이지 종료 시, PlayerData에 현재 객체 정보 저장하는 구조
    private void Awake()
    {
        // 씬 불러와지고 바로 시작할지, 스테이지 시작 이벤트 받고 시작할지 고민 중
        pd = PlayerData.Instance;

        DataInit(); // 임시/테스트용, 첫 시작 판정에서 해줘야함

        // grade = pd.grade;
        life = pd.life;
        moveSpeed = pd.moveSpeed;
        shotSpeed = pd.shotSpeed;
        // score = pd.score; 스코어는 실시간으로 업데이트 해야함

        // 스테이지매니저.스테이지 종료 이벤트.AddListener(SavePlayerData);
    }

    private void Start()
    {
        // 스테이지 종료 시 플레이어 데이터 저장
        StageManager.Instance.StageCloseEvent.AddListener(SavePlayerData);


        // 등급 상태 테스트용.
        UpdateRender();

        // 필수 에러 디버깅
        if (respawnPoint == null) Debug.LogError("리스폰 포인트를 설정해주세요.");
    }

    private void OnDestroy()
    {
        // 스테이지매니저.스테이지 종료 이벤트.RemoveListener(SavePlayerData);
    }


    // Todo: 플레이어 데이터에서 구현하는걸로 변경
    // 맨 처음 코인을 시작하는 시점(GameStart)에서만 호출       ***GameStart와 StageStart는 구분되어야 함. 
    private void DataInit()
    {
        // 게임매니저 상태가 == 첫시작 return;
        // 초기 설정으로 저장
        pd.SaveData(life_Init, moveSpeed_Init, shotSpeed_Init, 0);
        pd.UpdateScore(0);
    }

    // 데미지 받음 => 죽음 판정
    public void TakeDamage()
    {
        if (state == PlayerState.Invincible)
        {
            Debug.Log("플레이어 무적 상태! 데미지 안받음.");
            return;
        }

        Debug.Log("플레이어 공격 판정");
        if (grade > 0)
        {
            grade -= 1;
            UpdateRender();

            // 무적 시간 추가
            StartCoroutine(TakeDamageCooling());

            // 피격 상태 이펙트

        }
        else Dead();
    }

    public IEnumerator TakeDamageCooling()
    {
        state = PlayerState.Invincible;
        yield return new WaitForSeconds(DamagedCoolTime);
        state = PlayerState.General;
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
        state = PlayerState.Respawning;

        // 플레이어 위치 이동 & 정지
        playerController.transform.position = respawnPoint.gameObject.transform.position;
        playerController.Body.transform.forward = respawnPoint.gameObject.transform.forward;
        playerController.dir = Vector3.zero;

        Debug.Log(respawnPoint.gameObject.transform.position);
        // 플레이어 초기값으로 재설정
        moveSpeed = moveSpeed_Init;
        shotSpeed = shotSpeed_Init;
        grade = 0;

        // 리스폰 이펙트 코루틴 실행
        StartCoroutine(RespawnEffect());
    }
    public IEnumerator RespawnEffect()
    {
        // 1초동안 이펙트 실행 or 셰이더 변경 후 코루틴 종료

        // 반짝이는 셰이더();
        Debug.Log("플레이어 리스폰 중...");
        respawnPoint.PlayerFBX(); // 이펙트 실행
        yield return new WaitForSeconds(respawningTime);
        // 셰이더 초기화();
        //respawnPoint.StopFBX(); // 이펙트 자동 종료 가능하니 주석처리함. 삭제해도 되면 삭제
        state = PlayerState.General;
        Debug.Log("플레이어 리스폰 완료");
    }


    public void UpdateRender()
    {
        // 렌더러 초기화
        for (int i = 0; i < 4; i++)
        {
            groupRender.GetChild(i).gameObject.SetActive(false);
        }

        // 등급에 맞는 그래픽 활성화
        switch (grade)
        {
            case UpgradeType.Grade01:
                groupRender.GetChild(0).gameObject.SetActive(true);
                break;
            case UpgradeType.Grade02:
                groupRender.GetChild(1).gameObject.SetActive(true);
                break;
            case UpgradeType.Grade03:
                groupRender.GetChild(2).gameObject.SetActive(true);
                break;
            case UpgradeType.Grade04:
                groupRender.GetChild(3).gameObject.SetActive(true);
                break;
        }
    }



    #region 아이템 & 환경 사용 호출 함수

    public void Upgrade(float moveSpeed, float shotSpeed, int score)
    {
        if (grade == UpgradeType.Grade04)
        {
            Debug.Log("이미 최고 등급입니다");
            pd.UpdateScore(score);
            // 점수 얻는걸로? 슈퍼마리오 버섯 처럼 이미 최종 단계면 점수로 치환
            return;
        }
        SpeedControl(moveSpeed, shotSpeed);
        grade += 1;
        UpdateRender();
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
        pd.SaveData(life, moveSpeed, shotSpeed, grade);
    }


    // 바닥 타일 연관 함수
    public void MoveTypeUpdate()
    {
        // 움직임 타입에 따라 이펙트&효과 업데이트
        // 이펙트는 플레이어 프리펩 안에 자식 오브젝트 폴더로 정리
        // 효과는 케이스별로 수치만 변경하면 됨
    }
}

// 플레이어 & Enemy 업그레이드 등급
public enum UpgradeType { Grade01, Grade02, Grade03, Grade04 }

public enum PlayerState { General, Invincible, Respawning } // {일반, 무적, ...상태가 더 필요하면 이곳에 추가}


