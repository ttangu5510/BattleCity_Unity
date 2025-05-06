using TMPro;
using UnityEngine;

public class InputRecord : MonoBehaviour
{
    ScoreBoard scoreBoard;
    public TMP_Text inputText;
    int rankNum;
    #region ������ �ؽ�Ʈ
    public TMP_Text[] rank = new TMP_Text[10];

    #endregion
    private void Awake()
    {
        scoreBoard.name = "YOU";
        scoreBoard.score = PlayerManager.Instance.Score;
        for (int i = 0; i < GameManager.Instance.scores.Length; i++)
        {
            if (GameManager.Instance.scores[i].score < scoreBoard.score)
            {
                rankNum = i;
                break;
            }
        }
        // �÷��̾� ������ 100��
        // 1�� 1000�� 10�� 2��
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

            // ����Ű �Է� ������ ��ǲ ���� ����.
            GameManager.Instance.state = GameState.InputComplete;
        }
    }
    private void OnDisable()
    {
        rank[rankNum].color = Color.white;
    }
}
