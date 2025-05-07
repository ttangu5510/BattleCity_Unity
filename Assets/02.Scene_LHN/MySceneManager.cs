using System.Collections.Generic;
using UnityEngine;

public class MySceneManager : MonoBehaviour
{
    // 씬전환 UI 프리펩을 여러개로 설정해두면, 특정 조건 별로 씬 전환 효과를 다르게 변경해줄 수 있음
    //sceneChangeUI_Prefab_Bonus; ==> 황금으로 넘쳐나는 씬 전환
    //sceneChangeUI_Prefab_BossStage; ==> Danger!! 느낌의 씬 전환
    //sceneChangeUI_Prefab_PlayerDead; ==> 빨간색 페이드아웃 등등

    [SerializeField] private List<GameObject> TransSceneEffect_Prefab;
    [SerializeField] private int index;
    private TransitionSceneEffect TransSceneEffect_Instance;

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
        // 현재 인스턴스 존재하는지 체크
        if (TransSceneEffect_Instance != null)
        {
            // 존재한다면? 현재 인스턴스가 내가 표시할 UI프리펩과 같은지 체크
            if (TransSceneEffect_Instance != TransSceneEffect_Prefab[index])
            {
                // 내가 만약 다른 UI페이드를 띄우고 싶을 땐 파괴 후 생성
                Destroy(TransSceneEffect_Instance.gameObject);
                TransSceneEffect_Instance = null;

                TransSceneEffect_Instance = Instantiate(TransSceneEffect_Prefab[index]).GetComponent<TransitionSceneEffect>();
                DontDestroyOnLoad(TransSceneEffect_Instance.gameObject);
            }
        }
        else
        {
            TransSceneEffect_Instance = Instantiate(TransSceneEffect_Prefab[index]).GetComponent<TransitionSceneEffect>();
            DontDestroyOnLoad(TransSceneEffect_Instance.gameObject);
        }
    }


    public void ChangeScene(string sceneName)
    {
        Debug.Log($"{sceneName}으로 이동 시도.");
        TransSceneEffect_Instance.StartLoading(sceneName);
    }

    public void MoveToFirstStage()
    {
        MySceneManager.Instance.TransSceneEffect_Instance.StartLoading(StageManager.Instance.stageDatas[0].StageName);
    }
}


