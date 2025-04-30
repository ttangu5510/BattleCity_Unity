using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnerState state;
    [SerializeField] private GameObject EffectPrefab;
    [SerializeField] private Enemy EnemyPrefab;
    
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
        Enemy enemy = Instantiate(EnemyPrefab, transform.position, transform.rotation);
        
        
    }

    // TODO:
    // �ݶ��̴��� ������ ���� �� ��/�÷��̾� ��ũ ������ ���� �Ұ� ���·� ����
    // ������ ���� �� �ƹ��� ������ �������� ���·� ����
}

public enum SpawnerState { Spawning, Spawnable, Disable }
