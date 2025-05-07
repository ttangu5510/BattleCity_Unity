using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageData : MonoBehaviour
{
    [Header("�������� �̸�")]
    [SerializeField] private string stageName;
    public string StageName { get { return stageName; } }
    //private StageManager sm;
    //private UIManager um;

    // �������� ������ ��ü�� ���������� ���� StageManager ������ ����ȭ�ϴ� �������� �ٲ���

    [Header("�������� ���� ����")]
    [Tooltip("�� �ȿ� ���ÿ� ������ �� �ִ� �ִ� �� ��")]
    [SerializeField] private int maxActiveEnemyCount;    // �� �� ���ÿ� ������ �� �ִ� �ִ� �� ��
    public int MaxActiveEnemyCount { get { return maxActiveEnemyCount; } }
    [Tooltip("óġ�ؾ� �Ǵ� ���� �� / �¸� ����")]
    [SerializeField] private int enemyLifeCount;         // óġ�ؾ� �Ǵ� ���� �� / �¸� ����
    public int EnemyLifeCount { get { return enemyLifeCount; } }

    /*private void Awake()
    {
        
    }

    // �������� �Ŵ��� �̱��� �ν��Ͻ��� Awake���� �Ҵ�ǹǷ� Start���� ���� ����
    private void OnEnable()
    {
        sm = StageManager.Instance;

        Debug.Log("�������� ������ �¿��̺�");
        // �������� �Ŵ��� ������ �ʱ�ȭ (���� �������� �� ����Ʈ ����)
        //sm.StageDataInit();
        // �������� �Ŵ����� ���� ������ ����ȭ
        //sm.SynchronizeStageData(maxActiveEnemyCount, enemyLifeCount);
    }

    private void Start()
    {
        um = UIManager.Instance;

        // UI�� óġ �ؾ��� ���� �� ���� �����ϱ�
        um.inGameUI_Instance.ShowEnemyLife();
    }*/
}
