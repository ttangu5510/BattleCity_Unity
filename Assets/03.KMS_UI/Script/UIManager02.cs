using System.Runtime.InteropServices;
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

        ShowPlayerLife();
        ShowScore();

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
        if (currentIndex < enemyIcons.Length)
            return;

        enemyIcons[currentIndex].sprite = emptySprite;
        currentIndex++;
    }

    public void ShowPlayerLife()
    {
        if (player != null && playerLifePointText != null)
        {
            playerLifePointText.text = $"X {player.Life}";
        }
        
    }

    public void ShowScore()
    {
        if (player != null)
        {
            scoreText.text = $"SCORE : {player.score}";
        }

    }
}
