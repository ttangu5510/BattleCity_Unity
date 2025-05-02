using System.Collections;
using TMPro;
using UnityEngine;

public class Test_InputRecord : MonoBehaviour
{
    Coroutine InputRoutine;
    ScoreBoard scoreBoard;
    TMP_Text inputText;
    bool isInput = false;
    int rankNum;
    private void Awake()
    {
        scoreBoard.name = "";
        scoreBoard.score = PlayerData.Instance.score;
        GameManager.Instance.scores[9] = scoreBoard;
        GameManager.Instance.SortScore();
        StartCoroutine(InputNameRoutine());
    }
    IEnumerator InputNameRoutine()
    {
        while (!isInput)
        {
            if(Input.GetKey(KeyCode.KeypadEnter))
            {
                //isInput = true;
                //for(int i = 0; i< )
                //GameManager.Instance.scores
            }
            yield return null;
        }
    }
}
