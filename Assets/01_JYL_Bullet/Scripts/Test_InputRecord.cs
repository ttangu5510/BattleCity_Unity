using TMPro;
using UnityEngine;

public class Test_InputRecord : MonoBehaviour
{
    ScoreBoard scoreBoard;
    public TMP_Text inputText;
    int rankNum;
    #region 순위별 텍스트
    public TMP_Text[] rank = new TMP_Text[10];

    #endregion
    private void Awake()
    {
        scoreBoard.name = "YOU";
        scoreBoard.score = PlayerData.Instance.score;
        for (int i = 0; i < GameManager.Instance.scores.Length; i++)
        {
            if (GameManager.Instance.scores[i].score < scoreBoard.score)
            {
                rankNum = i;
                break;
            }
        }
        
        GameManager.Instance.scores[9] = scoreBoard;
        GameManager.Instance.SortScore();
        for (int i = 0; i < GameManager.Instance.scores.Length; i++)
        {
            if (i == rankNum)
            {
                rank[i].text = $"{i + 1} {GameManager.Instance.scores[i].name} {GameManager.Instance.scores[i].score}";
                rank[i].color = Color.red;
            }
            else
            {
                rank[i].text = $"{i + 1} {GameManager.Instance.scores[i].name} {GameManager.Instance.scores[i].score}";
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            GameManager.Instance.scores[rankNum].name = inputText.text;
            GameManager.Instance.isInput = true;
        }

    }
}
