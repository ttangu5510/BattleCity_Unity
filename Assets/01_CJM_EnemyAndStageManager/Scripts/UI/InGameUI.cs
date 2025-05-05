using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InGameUI : MonoBehaviour
{
    private static InGameUI instance;
    public static InGameUI Instance { get { return instance; } }

    UIManager um;
    StageManager sm;
    PlayerManager pm;


    [SerializeField] private TextMeshProUGUI playerLifePointText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject enemyLifeCount_Panel;
    private List<Image> enemyIcons = new List<Image>();
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private Sprite fullSprite;
    [SerializeField] private Sprite emptySprite;
    private int currentIndex = 0;


    private void Awake()
    {
        if (gameOverUI.activeSelf) gameOverUI.SetActive(false);


        foreach (Transform child in enemyLifeCount_Panel.transform)
        {
            enemyIcons.Add(child.GetComponent<Image>());
        }
    }

    private void OnEnable()
    {
        um = UIManager.Instance;
        sm = StageManager.Instance;
        pm = PlayerManager.Instance;
    }


    public void GameOverUIStart()
    {
        gameOverUI.SetActive(true);
    }

    public void ShowEnemyLife()
    {
        for (int i = 0; i < enemyIcons.Count; i++)
        {
            enemyIcons[i].sprite = (i < sm.EnemyLifeCount) ? fullSprite : emptySprite;
        }
        currentIndex = 0;
    }

    public void HideEnemyLife()
    {
        if (currentIndex >= enemyIcons.Count)
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

    public void InitializeEnemyIcons(int count)
    {
        currentIndex = 0;

        for (int i = 0; i < enemyIcons.Count; i++)
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
