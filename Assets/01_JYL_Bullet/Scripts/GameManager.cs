using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager instance;
    public GameManager Instance
        
    #region 싱글톤 설정

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
    // 싱글톤 필드
    public int topScore;

    // 싱글톤 함수
    public void StageComplete()
    {
        // 스테이지 클리어 시 수행
    }
    public void GameComplete()
    {
        // 마지막 스테이지 클리어 시 수행
    }
    public void GameOver()
    {
        // 게임 오버 시 수행
    }
}
