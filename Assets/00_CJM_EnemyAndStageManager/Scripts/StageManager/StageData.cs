using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    private StageManager sm;
    private int maxActiveEnemyCount;    // �� �� ���ÿ� ������ �� �ִ� �ִ� �� ��
    private int enemyLifeCount;         // óġ�ؾ� �Ǵ� ���� �� / �¸� ����

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
