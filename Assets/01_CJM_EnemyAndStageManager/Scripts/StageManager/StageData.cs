using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    private StageManager sm;
    [Header("�������� ���� ����")]
    [Tooltip("�� �ȿ� ���ÿ� ������ �� �ִ� �ִ� �� ��")]
    [SerializeField] private int maxActiveEnemyCount;    // �� �� ���ÿ� ������ �� �ִ� �ִ� �� ��
    [Tooltip("óġ�ؾ� �Ǵ� ���� �� / �¸� ����")]
    [SerializeField] private int enemyLifeCount;         // óġ�ؾ� �Ǵ� ���� �� / �¸� ����

    // �������� �Ŵ��� �̱��� �ν��Ͻ��� Awake���� �Ҵ�ǹǷ� Start���� ���� ����
    private void OnEnable()
    {
        sm = StageManager.Instance;

        // �������� �Ŵ��� ������ �ʱ�ȭ (���� �������� �� ����Ʈ ����)
        sm.StageDataInit();
        // �������� �Ŵ����� ���� ������ ����ȭ
        sm.SynchronizeStageData(maxActiveEnemyCount, enemyLifeCount);
    }
}
