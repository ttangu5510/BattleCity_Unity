using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool isPaused; // 게임의 Pause 상태를 관리
    public Canvas gameCanvas; // UI 요소가 포함된 Canvas
    public GameObject[] enemyIcons; // 적의 아이콘 배열
    public GameObject playerIcon; // 플레이어의 상태 아이콘
    public Sprite itemIcon; // 아이템 아이콘(Sprite)

    private void Update()
    {
        // 게임 상태를 체크하고 UI를 동적으로 업데이트
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            gameCanvas.enabled = !gameCanvas.enabled; // 캔버스 활성/비활성
            if(isPaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void ShowIconOnUI(Image uiElement, Sprite icon)
    {
        // UI 요소에 특정 아이콘 표시
        uiElement.sprite = icon;
        uiElement.enabled = true;
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
    public void ShowPlayerLife(int life)
    {
        // 플레이어의 라이프를 UI에 표시
        playerIcon.GetComponentInChildren<Text>().text = $"Life: {life}";
    }
    public void ShowScore(int score)
    {
        // 점수 표시
        gameCanvas.transform.Find("ScoreScene").GetComponent<Text>().text = $"Score: {score}";
    }
}
