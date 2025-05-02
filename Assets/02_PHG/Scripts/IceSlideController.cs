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
        // �����̵� �� �� ������ ������ ��츸 ���� ���� ���
        if (IsSliding && rb.velocity.magnitude < 0.1f && !canChooseDirection)
        {
            // Ray�� ���� ���⿡ ��ֹ� �ִ��� �˻�
            Ray ray = new Ray(transform.position, lastMoveDir);
            float checkDistance = 0.6f;

            if (Physics.Raycast(ray, out RaycastHit hit, checkDistance))
            {
                // ���� ���¿����� ȸ�� ���
                Debug.Log($"���� ������Ʈ: {hit.collider.name} �� ���� ���� ����");
                canChooseDirection = true;
                playerController.enabled = true;
            }
        }

        // �����̵� ���� �ƴ� ���� ���� �̸� ����
        if (!IsSliding && playerController != null && playerController.dir != Vector3.zero)
        {
            lastMoveDir = playerController.dir;
        }

        // �����̵� �߿��� ������ ����
        if (Input.GetKeyDown(KeyCode.X))
        {
            var player = GetComponent<PlayerController>();
            if (player != null)
            {
                player.Attack(); // �Ǵ� Fire() �� ���� �޼��� �̸�
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

            // 2�� �� �����̵� �ڵ� ����
            
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

        Debug.Log("�����̵� ���� �� ���� ���� ����");
    }

    private void ForceStopSliding()
    {
        if (IsSliding)
        {
            Debug.Log("�����̵� �ð� ���� �� ���� ����");
            StopSliding();
        }
    }

}