using UnityEngine;

public class IceSlideController : MonoBehaviour
{
    public Vector3 lastMoveDir { get; private set; } = Vector3.zero;

    private PlayerController playerController;
    private float slideStartTime;
    private float collisionIgnoreDuration = 0.2f;
    private float slideCooldownEndTime = 0f;
    public bool IsSliding { get; private set; } = false;
    private bool canChooseDirection = false;
    private Rigidbody rb;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 슬라이딩 중 → 막혀서 정지한 경우만 방향 선택 허용
        if (IsSliding && rb.velocity.magnitude < 0.1f && !canChooseDirection)
        {
            // Ray로 진행 방향에 장애물 있는지 검사
            Ray ray = new Ray(transform.position, lastMoveDir);
            float checkDistance = 0.6f;

            if (Physics.Raycast(ray, out RaycastHit hit, checkDistance))
            {
                // 막힌 상태에서만 회전 허용
                Debug.Log($"막힌 오브젝트: {hit.collider.name} → 방향 선택 가능");
                canChooseDirection = true;
                playerController.enabled = true;
            }
        }

        // 슬라이딩 중이 아닐 때는 방향 미리 저장
        if (!IsSliding && playerController != null && playerController.dir != Vector3.zero)
        {
            lastMoveDir = playerController.dir;
        }

        // 슬라이딩 중에도 공격은 가능
        if (Input.GetKeyDown(KeyCode.X))
        {
            var player = GetComponent<PlayerController>();
            if (player != null)
            {
                player.Attack(); // 또는 Fire() 등 실제 메서드 이름
            }
        }
    }

    public void StartSliding()
    {
        if (!IsSliding)
        {
            IsSliding = true;
            slideStartTime = Time.time;

            if (playerController != null)
                playerController.enabled = false;

            // 2초 후 슬라이딩 자동 종료
            
        }
    }
    public void RestoreControl()
    {
        IsSliding = false;
        if (playerController != null)
            playerController.enabled = true;
    }
    public void StopSliding()
    {
        IsSliding = false;
        canChooseDirection = true;
        if (playerController != null)
            playerController.enabled = true;

        Debug.Log("슬라이딩 종료 → 방향 선택 가능");
    }

    private void ForceStopSliding()
    {
        if (IsSliding)
        {
            Debug.Log("슬라이딩 시간 만료 → 강제 종료");
            StopSliding();
        }
    }

}