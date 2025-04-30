using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnerState state;
    [SerializeField] private Transform standByGroup;
    [SerializeField] private GameObject EffectPrefab;
    
    private StageManager sm;

    private void Start()
    {
        sm = StageManager.Instance;
        sm.SpawnerAddToList(this);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEnemy();
        }
    }
    

    public void SpawnEnemy()
    {
        // �ڽ� Ȱ��ȭ
        if (!standByGroup.GetChild(0).gameObject.activeSelf)
            standByGroup.GetChild(0).gameObject.SetActive(true);
    }

    // TODO:
    // �ݶ��̴��� ������ ���� �� ��/�÷��̾� ��ũ ������ ���� �Ұ� ���·� ����
    // ������ ���� �� �ƹ��� ������ �������� ���·� ����
}

public enum SpawnerState { Spawning, Spawnable, Disable }
