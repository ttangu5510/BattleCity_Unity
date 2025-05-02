using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager02 : MonoBehaviour
{
    public bool isPaused; // 게임의 Pause 상태를 관리
    public Canvas gameCanvas; // UI 요소가 포함된 Canvas
    public GameObject[] enemyIcons; // 적의 아이콘 배열
    public Sprite playerIcon; // 플레이어의 상태 아이콘

    [SerializeField] private Image targetImage;
    [SerializeField] private TextMeshProUGUI playerLifePointText;
    [SerializeField] private Player player;

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

    }
    public void ShowEnemyLife()
    {
        foreach (GameObject icon in enemyIcons)
        {
            icon.SetActive(true); // 적 아이콘 활성화
        }
    }

    public void HideEnemyLife()
    {
        foreach (GameObject icon in enemyIcons)
        {
            icon.SetActive(false); // 적 아이콘 비활성화
        }
    }

    public void ShowPlayerLife()
    {
        if (player != null && playerLifePointText != null)
        {
            playerLifePointText.text = $"x {player.Life}";
        }
    }

    public void ShowScore(int score)
    {
        // 점수 표시

    }

    public void ChangeImage()
    {
        Sprite newSprite = Resources.Load<Sprite>("Sprites/playerTank"); // Resources 폴더 기준
        if (newSprite != null)
        {
            targetImage.sprite = newSprite;
        }
        else
        {
            Debug.LogWarning("Sprite 로드 실패!");
        }
    }
}
