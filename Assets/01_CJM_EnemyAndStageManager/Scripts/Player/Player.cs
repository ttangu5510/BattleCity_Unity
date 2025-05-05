using KMS;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IDamagable, IMovable
{
    private float DamagedCoolTime;
    private float respawningTime;
    [SerializeField] private RespawnPoint respawnPoint;

    [Header("Current Player Data")]
    //[SerializeField] public UpgradeType grade;
    //[SerializeField] public PlayerState state;
    /*[SerializeField] public int Life { get { return life; } }
    [SerializeField] private int life;
    [SerializeField] public float MoveSpeed { get { return moveSpeed; } }
    [SerializeField] private float moveSpeed;
    [SerializeField] public float ShotSpeed { get { return shotSpeed; } }
    [SerializeField] private float shotSpeed;*/

    [Header("Setting Field")]
    [SerializeField] private Transform groupRender;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject explosionFBX;

    private PlayerManager pm;
    private StageManager sm;
    private UIManager um;


    /*private MoveType ontileMove;
    public MoveType moveType { get { return ontileMove; } set { ontileMove = value; } }*/
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
    // 스테이지 진행 중에는 현재 객체가 정보를 담당. 스테이지 종료 시, PlayerManager에 현재 객체 정보 저장하는 구조
    private void Start()
    {
        // 씬 불러와지고 바로 시작할지, 스테이지 시작 이벤트 받고 시작할지 고민 중
        pm = PlayerManager.Instance;
        sm = StageManager.Instance;
        um = UIManager.Instance;

        // 초기값 그대로
        DamagedCoolTime = pm.DamagedCoolTime;
        respawningTime = pm.RespawningTime;

        // 동기화된 초기 설정 UI에 반영
        um.inGameUI_Instance.ShowPlayerLife();
        um.inGameUI_Instance.ShowCurrentScore();


        // 등급 상태 테스트용.
        UpdateRender();

        // 필수 에러 디버깅
        if (respawnPoint == null) Debug.LogError("리스폰 포인트를 설정해주세요.");
    }

    private void OnDestroy()
    {

    }

    // 데미지 받음 => 죽음 판정
    public void TakeDamage()
    {
        if (pm.State == PlayerState.Invincible)
        {
            Debug.Log("플레이어 무적 상태! 데미지 안받음.");
            return;
        }

        if (pm.Grade > 0)
        {
            pm.PlayerGradeUpdate(-1);
            UpdateRender();

            // 무적 시간 추가
            StartCoroutine(TakeDamageCooling());

            // 피격 상태 이펙트

        }
        else Dead();
    }

    public IEnumerator TakeDamageCooling()
    {
        pm.PlayerStateUpdate(PlayerState.Invincible);
        // TODO : 플레이어 피격 이펙트 or 셰이더 실행
        yield return new WaitForSeconds(DamagedCoolTime);
        pm.PlayerStateUpdate(PlayerState.General);
        // TODO : 플레이어 피격 이펙트 or 셰이더 초기화
    }

    // 죽음 => 게임오버 판정
    public void Dead()
    {
        // 라이프 감소
        pm.CalculateLife(-1);

        // TODO: 펑 터지는 효과 실행 [도전과제]
        // 플레이어 잠깐 비활성화?
        GameObject explosion =  Instantiate(explosionFBX);
        explosion.transform.position = playerController.transform.position;
        groupRender.gameObject.SetActive(false);
        playerController.gameObject.SetActive(false);

        // 라이프가 0 아래로 떨어지면 패배 조건 체크
        if (pm.Life < 0)
        {
            // TODO: For Test of TestScene
            gameObject.SetActive(false);
            sm.StageFail();
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
        

        // 플레이어 초기값으로 재설정
        pm.PlayerInit();

        // 플레이어 위치 이동 & 정지
        playerController.transform.position = respawnPoint.gameObject.transform.position;
        playerController.Body.transform.forward = respawnPoint.gameObject.transform.forward;
        playerController.dir = Vector3.zero;

        // 리스폰 이펙트 코루틴 실행
        StartCoroutine(RespawnEffect());
    }
    public IEnumerator RespawnEffect()
    {
        // 1초동안 이펙트 실행 or 셰이더 변경 후 코루틴 종료

        // 반짝이는 셰이더();
        pm.PlayerStateUpdate(PlayerState.Respawning);
        
        respawnPoint.PlayerFBX(); // 이펙트 실행
        yield return new WaitForSeconds(respawningTime);
        // 셰이더 초기화();

        // 소환
        groupRender.gameObject.SetActive(true);
        playerController.gameObject.SetActive(true);
        //respawnPoint.StopFBX(); // 이펙트 자동 종료 가능하니 주석처리함. 삭제해도 되면 삭제
        pm.PlayerStateUpdate(PlayerState.General);
    }


    public void UpdateRender()
    {
        // 렌더러 초기화
        for (int i = 0; i < 4; i++)
        {
            groupRender.GetChild(i).gameObject.SetActive(false);
        }

        // 등급에 맞는 그래픽 활성화
        switch (pm.Grade)
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
        if (pm.Grade == UpgradeType.Grade04)
        {
            Debug.Log("이미 최고 등급입니다");
            pm.ScoreUpdate(score);
            // 점수 얻는걸로? 슈퍼마리오 버섯 처럼 이미 최종 단계면 점수로 치환
            return;
        }
        pm.SpeedControl(moveSpeed, shotSpeed);
        pm.PlayerGradeUpdate(+1);
        UpdateRender();
    }


    

    

    #endregion



    // 바닥 타일 연관 함수
    public void MoveTypeUpdate()
    {
        // 움직임 타입에 따라 이펙트&효과 업데이트
        // 이펙트는 플레이어 프리펩 안에 자식 오브젝트 폴더로 정리
        // 효과는 케이스별로 수치만 변경하면 됨
        Debug.Log(moveType);
    }
}

// 플레이어 & Enemy 업그레이드 등급



