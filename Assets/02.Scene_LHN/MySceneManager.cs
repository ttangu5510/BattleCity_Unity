using System.Collections.Generic;
using UnityEngine;

public class MySceneManager : MonoBehaviour
{
    // 씬전환 UI 프리펩을 여러개로 설정해두면, 특정 조건 별로 씬 전환 효과를 다르게 변경해줄 수 있음
    //sceneChangeUI_Prefab_Bonus; ==> 황금으로 넘쳐나는 씬 전환
    //sceneChangeUI_Prefab_BossStage; ==> Danger!! 느낌의 씬 전환
    //sceneChangeUI_Prefab_PlayerDead; ==> 빨간색 페이드아웃 등등

    [SerializeField] private List<GameObject> fadeTransUI_Prefab;
    [SerializeField] private int index;
    private FadeTransition fadeTransUI_Instance;

    private static MySceneManager instance;
    public static MySceneManager Instance { get { return instance; } }
    /*{
        get
        {
            // Lazy Initialization
            if (instance == null)
            {
                GameObject go = new GameObject("MySceneManager");
                instance = go.AddComponent<MySceneManager>();
            }
            return instance;
        }
    }*/


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // 일단 기본 전환효과로 설정해두고 FadeTransitionSelect()으로 그때그때마다 효과 바꿔주기
            FadeTransitionSelect(index);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeTransitionSelect(int index)
    {
        if (fadeTransUI_Instance != null)
        {
            Destroy(fadeTransUI_Instance.gameObject);
            fadeTransUI_Instance = null;
        }

        fadeTransUI_Instance = Instantiate(fadeTransUI_Prefab[index]).GetComponent<FadeTransition>();
        DontDestroyOnLoad(fadeTransUI_Instance.gameObject);
    }


    public void ChangeScene(string sceneName)
    {
        fadeTransUI_Instance.StartLoading(sceneName);
    }

    public void MoveToFirstStage()
    {
        MySceneManager.Instance.fadeTransUI_Instance.StartLoading(StageManager.Instance.stageDatas[0].StageName);
    }
}


