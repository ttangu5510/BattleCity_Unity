using UnityEngine;

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
    public int topScore;

    // �̱��� �Լ�
    public void StageComplete()
    {
        // �������� Ŭ���� �� ����
    }
    public void GameComplete()
    {
        // ������ �������� Ŭ���� �� ����
    }
    public void GameOver()
    {
        // ���� ���� �� ����
    }
}
