using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager02 : MonoBehaviour
{
    private static UIManager02 instance;
    public static UIManager02 Instance { get { return instance; } }

    public bool isPaused; // ������ Pause ���¸� ����
    public Canvas gameCanvas; // UI ��Ұ� ���Ե� Canvas

    [SerializeField] private TextMeshProUGUI playerLifePointText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image[] enemyIcons;
    [SerializeField] private Sprite fullSprite;
    [SerializeField] private Sprite emptySprite;

    public GameObject gameOverUI;

    //[TEST-ONLY]
    //[SerializeField] private bool useMockPlayerLife = true; //UI �׽�Ʈ��
    //[SerializeField] private int mockLife = 5; //UI �׽�Ʈ��
    //[SerializeField] private bool useMockScore = true;
    //[SerializeField] private int mockScore = 9999;
    // �׽�Ʈ �Ϸ�

    PlayerManager pm;
    StageManager sm;
    private int currentIndex = 0;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        gameOverUI.SetActive(false);
    }

    private void Start()
    {
        pm = PlayerManager.Instance;
        sm = StageManager.Instance;

        //ShowPlayerLife();
        //ShowCurrentScore();
    }

    private void Update()
    {
        HandlePauseToggle();
        //ShowPlayerLife();
        //ShowCurrentScore();
        //ShowHighScore(); //���̽��ھ� �κ��� ��� ����ɰ��� �̾߱��غ����ҵ�.

        //[TEST-ONLY]
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Debug.Log("[Test] ���ı��� ������ ����� Ȯ�ο�");
        //    OnEnemyKill();
        //}
    }
    public void ShowEnemyLife()
    {
        for (int i = 0; i < enemyIcons.Length; i++)
        {
            enemyIcons[i].sprite = (i < sm.EnemyLifeCount) ? fullSprite : emptySprite;
        }
        currentIndex = 0;
    }

    public void HideEnemyLife()
    {
        if (currentIndex >= enemyIcons.Length)
            return;

        enemyIcons[currentIndex].CrossFadeAlpha(0f, 0.3f, false);
        currentIndex++;
    }

    public void ShowPlayerLife()
    {
        if (playerLifePointText == null) return;

        //[TEST-ONLY]
        //int lifeToShow = (useMockPlayerLife || player == null) ? mockLife : player.Life;
        playerLifePointText.text = $"X {pm.Life}";
    }

    public void ShowCurrentScore()
    {
        if (scoreText == null) return;

        //[TEST-ONLY]
        //int scoreToShow = (useMockScore || player == null) ? mockScore : player.score;
        scoreText.text = $"SCORE\n{pm.Score}";
    }
    //public void ShowHighScore()
    //{
    //    if (scoreText == null) return;
    //
    //    int scoreToShow = (useMockScore || player == null) ? mockScore : player.score;
    //    scoreText.text = $"SCORE : {scoreToShow}";
    //}


    public void OnEnemyKill()
    {
        HideEnemyLife();
    }
    private void HandlePauseToggle()
    {
        // ���� ���¸� üũ�ϰ� UI�� �������� ������Ʈ
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            gameCanvas.enabled = !gameCanvas.enabled; // ĵ���� Ȱ��/��Ȱ��
            if (isPaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
    public void InitializeEnemyIcons(int count)
    {
        currentIndex = 0;

        for (int i = 0; i < enemyIcons.Length; i++)
        {
            Image icon = enemyIcons[i];

            icon.canvasRenderer.SetAlpha(0f);

            if (i < count)
            {
                icon.CrossFadeAlpha(1f, 0f, false);
            }
        }
    }

    public void GameOverUIPlay()
    {
        if (gameOverUI.activeSelf == true)
        {
            gameOverUI.SetActive(false);
        }
        else
        {
            gameOverUI.SetActive(true);
        }
    }
}
