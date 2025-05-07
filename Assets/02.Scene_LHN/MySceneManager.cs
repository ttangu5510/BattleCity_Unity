using System.Collections.Generic;
using UnityEngine;

public class MySceneManager : MonoBehaviour
{
    // ����ȯ UI �������� �������� �����صθ�, Ư�� ���� ���� �� ��ȯ ȿ���� �ٸ��� �������� �� ����
    //sceneChangeUI_Prefab_Bonus; ==> Ȳ������ ���ĳ��� �� ��ȯ
    //sceneChangeUI_Prefab_BossStage; ==> Danger!! ������ �� ��ȯ
    //sceneChangeUI_Prefab_PlayerDead; ==> ������ ���̵�ƿ� ���

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

            // �ϴ� �⺻ ��ȯȿ���� �����صΰ� FadeTransitionSelect()���� �׶��׶����� ȿ�� �ٲ��ֱ�
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


