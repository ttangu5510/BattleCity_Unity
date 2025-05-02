using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager02 : MonoBehaviour
{
    public bool isPaused; // ������ Pause ���¸� ����
    public Canvas gameCanvas; // UI ��Ұ� ���Ե� Canvas
    public GameObject[] enemyIcons; // ���� ������ �迭
    public Sprite playerIcon; // �÷��̾��� ���� ������

    [SerializeField] private Image targetImage;
    [SerializeField] private TextMeshProUGUI playerLifePointText;
    [SerializeField] private Player player;

    private void Update()
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

        ShowPlayerLife();

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

    public void ShowPlayerLife()
    {
        if (player != null && playerLifePointText != null)
        {
            playerLifePointText.text = $"x {player.Life}";
        }
    }

    public void ShowScore(int score)
    {
        // ���� ǥ��

    }

    public void ChangeImage()
    {
        Sprite newSprite = Resources.Load<Sprite>("Sprites/playerTank"); // Resources ���� ����
        if (newSprite != null)
        {
            targetImage.sprite = newSprite;
        }
        else
        {
            Debug.LogWarning("Sprite �ε� ����!");
        }
    }
}
