using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager02 : MonoBehaviour
{
    public static UIManager02 Instance { get; private set; }

    public bool isPaused; // 게임의 Pause 상태를 관리
    public Canvas gameCanvas; // UI 요소가 포함된 Canvas

    [SerializeField] private TextMeshProUGUI playerLifePointText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Player player;
    [SerializeField] private Image[] enemyIcons;
    [SerializeField] private Sprite fullSprite;
    [SerializeField] private Sprite emptySprite;

    //[TEST-ONLY]
    //[SerializeField] private bool useMockPlayerLife = true; //UI 테스트용
    //[SerializeField] private int mockLife = 5; //UI 테스트용
    //[SerializeField] private bool useMockScore = true;
    //[SerializeField] private int mockScore = 9999;
    // 테스트 완료


    private int currentIndex = 0;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(gameObject);
    }
    private void Update()
    {
        HandlePauseToggle();
        ShowPlayerLife();
        ShowCurrentScore();
        //ShowHighScore(); //하이스코어 부분은 어디에 저장될건지 이야기해봐야할듯.

        //[TEST-ONLY]
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Debug.Log("[Test] 적파괴시 아이콘 사라짐 확인용");
        //    OnEnemyKill();
        //}
    }
    public void ShowEnemyLife(int count)
    {
        for (int i = 0; i < enemyIcons.Length; i++)
        {
            enemyIcons[i].sprite = (i < count) ? fullSprite : emptySprite;
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

        //[TEST-ONLY] int lifeToShow = (useMockPlayerLife || player == null) ? mockLife : player.Life;
        playerLifePointText.text = $"X {player.Life}";
    }

    public void ShowCurrentScore()
    {
        if (scoreText == null) return;

        //[TEST-ONLY] int scoreToShow = (useMockScore || player == null) ? mockScore : player.score;
        scoreText.text = $"SCORE : {player.score}";
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
        // 게임 상태를 체크하고 UI를 동적으로 업데이트
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            gameCanvas.enabled = !gameCanvas.enabled; // 캔버스 활성/비활성
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
}
