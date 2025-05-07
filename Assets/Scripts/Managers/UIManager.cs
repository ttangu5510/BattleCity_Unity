using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    public bool isPaused; // 게임의 Pause 상태를 관리

    private List<string> stageNames = new List<string>();

    StageManager sm;

    // 프리펩 인스턴시에이트로 
    [SerializeField] private GameObject inGameUI_Prefab;  // 인게임 UI
    public InGameUI inGameUI_Instance { get; private set; }  // 인게임 UI 활성화 여부 확인용 인스턴스



    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sm = StageManager.Instance;

        // 특정 스테이지의 전환 효과 적용 가능 (원하는 스테이지 -1 로 설정)
        /*if (scene.name.Contains("BOSS"))
        {
            MySceneManager.Instance.FadeTransitionSelect(1);
        }*/

        if (sm.stageNames.Contains(scene.name))
        {
            if (inGameUI_Instance != null) return;
            inGameUI_Instance = Instantiate(inGameUI_Prefab).GetComponent<InGameUI>();
            DontDestroyOnLoad(inGameUI_Instance.gameObject);
        }
        else
        {
            if (inGameUI_Instance != null)
                Destroy(inGameUI_Instance.gameObject);
        }
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }


    private void OnEnable()
    {

    }
        

    private void Update()
    {
        HandlePauseToggle();
    }
    
    
    private void HandlePauseToggle()
    {
        // 게임 상태를 체크하고 UI를 동적으로 업데이트
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
    
    public void GameOverUIPlay()
    {
        if (inGameUI_Instance != null)
        inGameUI_Instance.GetComponent<InGameUI>().GameOverUIStart();
    }
}
