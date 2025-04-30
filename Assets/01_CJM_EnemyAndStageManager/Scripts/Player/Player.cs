using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    // : IDamagable, IMoveable(�̰� �÷��̾� ��Ʈ�ѷ����� �ްڽ��ϴ�)
    [Header("�ʱ� ����")] // �� ó�� ������ ������ ��. �⺻ ������ �Է��ϴ� ĭ�Դϴ�.
    [SerializeField] private int life_Init;
    [SerializeField] private float moveSpeed_Init;
    [SerializeField] private float shotSpeed_Init;

    [Header("�������� �� ���� ����Ʈ")]
    [SerializeField] private Transform respawnPoint;

    [Header("���� �÷��̾� ����")]
    [SerializeField] public UpgradeType grade;
    [SerializeField] public PlayerState state;
    [SerializeField] private int life;
    [SerializeField] public float moveSpeed { get; private set; }
    [SerializeField] public float shotSpeed { get; private set; }
    [SerializeField] private int score;

    private PlayerData pd; // ��� & ���ھ� ����

    public bool isDamagable { get; private set; } // �ǰ� ���� ���� ����  (������ �� ����, ������ ������� ���� ���� ����, ���)

    // private Item itemPossession; �������� ������ �� �ְ� ����� �ʹٸ� ���
    // public UnityEvent PlayerDeadEvent = new UnityEvent(); ���� ���� �̺�Ʈ�θ� �ص� ����� ��. �÷��̾� ��� �� Ư�� ���� �ʿ��� �� Ȱ��ȭ


    // ������������ �÷��̾� ������Ʈ�� Ȱ��ȭ �� ��, �÷��̾� �����͸� ����ȭ ��Ŵ
    // �������� ���� �߿��� ���� ��ü�� ������ ���. �������� ���� ��, PlayerData�� ���� ��ü ���� �����ϴ� ����
    private void Awake()
    {
        // �� �ҷ������� �ٷ� ��������, �������� ���� �̺�Ʈ �ް� �������� ��� ��
        pd = PlayerData.Instance;

        DataInit(); // �ӽ�/�׽�Ʈ��, ù ���� �������� �������

        grade = pd.grade;
        life = pd.life;
        moveSpeed = pd.moveSpeed;
        shotSpeed = pd.shotSpeed;
        score = pd.score;

        // ���������Ŵ���.�������� ���� �̺�Ʈ.AddListener(SavePlayerData);
    }

    private void Start()
    {
        // �������� ���� �� �÷��̾� ������ ����
        StageManager.Instance.StageCloseEvent.AddListener(SavePlayerData);
    }

    private void OnDestroy()
    {
        // ���������Ŵ���.�������� ���� �̺�Ʈ.RemoveListener(SavePlayerData);
    }

    // �� ó�� ������ �����ϴ� ����(GameStart)������ ȣ��       ***GameStart�� StageStart�� ���еǾ�� ��. 
    private void DataInit()
    {
        // �ʱ� �������� ����
        pd.SaveData(life_Init, moveSpeed_Init, shotSpeed_Init, UpgradeType.normal, 0);
    }

    // ������ ���� => ���� ����
    public void TakeDamage()
    {
        if (grade > UpgradeType.normal) grade -= 1;
        else Dead();
    }

    // ���� => ���ӿ��� ����
    public void Dead()
    {
        // ������ ����
        life -= 1;

        // �������� 0 �Ʒ��� �������� �й� ���� üũ
        if (life <= 0)
        {
            //GameManager.Instance.GameOver();
            return;
        }
        // �������� �������� ������
        else
        {
            Respawn();
        }
    }

    // ������ => �ʱ� ���������� �÷��̾� �ʱ�ȭ, ���� ����Ʈ�� ��ġ ����, ������ ȿ�� �ڷ�ƾ ����
    public void Respawn()
    {
        // �÷��̾� ��ġ �̵�
        transform.position = respawnPoint.position;

        // �÷��̾� �ʱⰪ���� �缳��
        moveSpeed = moveSpeed_Init;
        shotSpeed = shotSpeed_Init;
        grade = UpgradeType.normal;

        // ������ ����Ʈ �ڷ�ƾ ����
        StartCoroutine(RespawnEffect());
    }
    public IEnumerator RespawnEffect()
    {
        // 1�ʵ��� ȿ�� ���� or ���̴� ���� �� �ڷ�ƾ ����

        // ȿ��();
        // ���̴�();
        Debug.Log("�÷��̾� ������ ��... ���� ����!");
        yield return new WaitForSeconds(1f);
        // ȿ�� ����();
        // ���̴� �ʱ�ȭ();
        Debug.Log("�÷��̾� ������ �Ϸ�");
    }

    #region ������ & ȯ�� ��� ȣ�� �Լ�

    public void Upgrade(float moveSpeed, float shotSpeed)
    {
        if (grade == UpgradeType.boss)
        {
            Debug.Log("�̹� �ְ� ����Դϴ�");
            // ���� ��°ɷ�? ���۸����� ���� ó�� �̹� ���� �ܰ�� ������ ġȯ
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



    // �������� ���� �� ȣ��
    public void SavePlayerData()
    {
        pd.SaveData(life, moveSpeed, shotSpeed, grade, score);
    }
}

// �÷��̾� & Enemy ���׷��̵� ���
public enum UpgradeType { normal, elite, boss }

public enum PlayerState { General, Invincible } // {�Ϲ�, ����, ...���°� �� �ʿ��ϸ� �̰��� �߰�}


