using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public struct ScoreBoard
{
    public string name;
    public int score;
}
public class GameManager : MonoBehaviour
{
    private GameManager instance;
    public GameManager Instance 
    #region �̱��� ����
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        CreateGameManager();
        lastStageNum = 25;
        scores = new ScoreBoard[10];
        for(int i = 2; i <= lastStageNum;i++)
        {
            stageSceneName.Enqueue($"STAGE {i}");
        }
        for(int i =0;i<scores.Length;i++)
        {
            scores[i].name = "BattleCity";
            scores[i].score = 10+10*i;
        }
    }

    public GameManager CreateGameManager()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        return instance;
    }
    #endregion

    // �̱��� �ʵ�
    private int lastStageNum;
    private Coroutine waitRoutine;
    private YieldInstruction waitSec;
    public ScoreBoard[] scores;
    // �� ť
    private Queue<string> stageSceneName; 
    // �̱��� �Լ�
    public void StageComplete()
    {
        // �� ��ȯ
        // 1. ���� �ջ� â �� -> 2. �ε�â �Լ� -> 3. ���� �������� ��
        // 1. ���� �ջ� â �� �ҷ�����
        SceneManager.LoadSceneAsync("���� �ջ� â ��");
        // 1-1 �ε� ��� �� ���ٰ� �Ѿ��
        WaitForSeconds(3f);
        // 2. �ε�â �ڵ� - ���� �̸��� �־����(�ذ�)
        // 3. ���� �������� �� �ҷ�����
        // ���� �� �������� ���� �� �Ǵ��� �ʿ���
        // �� ť�� �ʿ� -> stageSceneName
        MySceneManager.Instance.ChangeScene(stageSceneName.Dequeue());
        // �÷��̾� ������ �������� ���� �������� ������ ������ ����Ʈ�� ����(�� ��ü�� ��������� ���� �Ǿ� ����)
    }

    public void GameComplete()
    {
        // ������ �������� Ŭ���� �� ����
        // �� ��ȯ -> 1. ���� �ջ� â �� -> 2. ���� Ŭ����(Congratulations) �� -> 3. ���� �� �Է� (�����ڵ� �� ��) -> 4. ���ڵ� �� -> 5. Ÿ��Ʋ ��
        // �ʱ�ȭ �۾�
        // �������� ť �ٽ� ä���

        // 1. ���� �ջ� â
        SceneManager.LoadSceneAsync("���� �ջ� â ��");
        WaitForSeconds(3f);
        // 2. ���� Ŭ���� ��
        SceneManager.LoadSceneAsync("���� Ŭ���� ��");
        // 3. ���� �Է� �� (�����ڵ� �� ��)
        SceneManager.LoadSceneAsync("���� �Է� ��");
        // 4. ���ڵ� ��
        SceneManager.LoadSceneAsync("���ڵ� ��");
        WaitForSeconds(3f);
    }

    public void GameOver()
    {
        // ���� ���� �� ����
        // ���� ���� UI -> ���� �ջ� â �� -> ���� ���� �� -> ���� ��(�����ڵ� �� ��) -> Ÿ��Ʋ ��
        // 1. ���� ���� UI
        // 2. ���� �ջ� â ��
        // 3. ���� ���� ��
        // 4. ���� ��(�� ���ڵ� �� ��)
        // 5. Ÿ��Ʋ ��
    }

    // Countinue


    private void WaitForSeconds(float sec)
    {
        waitSec = new WaitForSeconds(sec);
        if(waitRoutine == null)
        {
            waitRoutine = StartCoroutine(WaitSecondsRoutine());
        }
        else
        {
            StopCoroutine(waitRoutine);
            waitRoutine = null;
            waitRoutine = StartCoroutine(WaitSecondsRoutine());
        }
    }
    IEnumerator WaitSecondsRoutine()
    {
        yield return waitSec;
    }
}
