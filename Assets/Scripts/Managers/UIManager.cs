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

    public bool isPaused; // ������ Pause ���¸� ����

    private List<string> stageNames = new List<string>();

    StageManager sm;

    // ������ �ν��Ͻÿ���Ʈ�� 
    [SerializeField] private GameObject inGameUI_Prefab;  // �ΰ��� UI
    public InGameUI inGameUI_Instance { get; private set; }  // �ΰ��� UI Ȱ��ȭ ���� Ȯ�ο� �ν��Ͻ�



    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sm = StageManager.Instance;

        // Ư�� ���������� ��ȯ ȿ�� ���� ���� (���ϴ� �������� -1 �� ����)
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
        // ���� ���¸� üũ�ϰ� UI�� �������� ������Ʈ
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
