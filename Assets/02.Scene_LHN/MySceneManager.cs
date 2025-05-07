using System.Collections.Generic;
using UnityEngine;

public class MySceneManager : MonoBehaviour
{
    // ����ȯ UI �������� �������� �����صθ�, Ư�� ���� ���� �� ��ȯ ȿ���� �ٸ��� �������� �� ����
    //sceneChangeUI_Prefab_Bonus; ==> Ȳ������ ���ĳ��� �� ��ȯ
    //sceneChangeUI_Prefab_BossStage; ==> Danger!! ������ �� ��ȯ
    //sceneChangeUI_Prefab_PlayerDead; ==> ������ ���̵�ƿ� ���

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
        // ���� �ν��Ͻ� �����ϴ��� üũ
        if (TransSceneEffect_Instance != null)
        {
            // �����Ѵٸ�? ���� �ν��Ͻ��� ���� ǥ���� UI������� ������ üũ
            if (TransSceneEffect_Instance != TransSceneEffect_Prefab[index])
            {
                // ���� ���� �ٸ� UI���̵带 ���� ���� �� �ı� �� ����
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
        Debug.Log($"{sceneName}���� �̵� �õ�.");
        TransSceneEffect_Instance.StartLoading(sceneName);
    }

    public void MoveToFirstStage()
    {
        MySceneManager.Instance.TransSceneEffect_Instance.StartLoading(StageManager.Instance.stageDatas[0].StageName);
    }
}


