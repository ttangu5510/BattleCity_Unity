using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool isPaused; // ������ Pause ���¸� ����
    public Canvas gameCanvas; // UI ��Ұ� ���Ե� Canvas
    public GameObject[] enemyIcons; // ���� ������ �迭
    public GameObject playerIcon; // �÷��̾��� ���� ������
    public Sprite itemIcon; // ������ ������(Sprite)

    private void Update()
    {
        // ���� ���¸� üũ�ϰ� UI�� �������� ������Ʈ
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            gameCanvas.enabled = !gameCanvas.enabled; // ĵ���� Ȱ��/��Ȱ��
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
        // UI ��ҿ� Ư�� ������ ǥ��
        uiElement.sprite = icon;
        uiElement.enabled = true;
    }

    public void ShowEnemyLife()
    {
        foreach (GameObject icon in enemyIcons)
        {
            icon.SetActive(true); // �� ������ Ȱ��ȭ
        }
    }

    public void HideEnemyLife()
    {
        foreach (GameObject icon in enemyIcons)
        {
            icon.SetActive(false); // �� ������ ��Ȱ��ȭ
        }
    }
    public void ShowPlayerLife(int life)
    {
        // �÷��̾��� �������� UI�� ǥ��
        playerIcon.GetComponentInChildren<Text>().text = $"Life: {life}";
    }
    public void ShowScore(int score)
    {
        // ���� ǥ��
        gameCanvas.transform.Find("ScoreScene").GetComponent<Text>().text = $"Score: {score}";
    }
}
