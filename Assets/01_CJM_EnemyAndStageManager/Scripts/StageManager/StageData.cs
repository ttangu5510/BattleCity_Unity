using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    [SerializeField] private StageManager sm;
    [SerializeField] private int maxActiveEnemyCount;    // �� �� ���ÿ� ������ �� �ִ� �ִ� �� ��
    [SerializeField] private int enemyLifeCount;         // óġ�ؾ� �Ǵ� ���� �� / �¸� ����

    // �������� �Ŵ��� �̱��� �ν��Ͻ��� Awake���� �Ҵ�ǹǷ� Start���� ���� ����
    private void Start()
    {
        sm = StageManager.Instance;

        // �������� �Ŵ��� ������ �ʱ�ȭ (���� �������� �� ����Ʈ ����)
        sm.StageDataInit();
        // �������� �Ŵ����� ���� ������ ����ȭ
        sm.SynchronizeStageData(maxActiveEnemyCount, enemyLifeCount);
    }
}
