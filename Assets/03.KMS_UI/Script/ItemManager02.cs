using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ��ӿ� ������ ���� ����ü
[Serializable]
public struct ItemPrefabEntry
{
    public int itemID;
    public GameObject prefab;
}

public class ItemManager02 : MonoBehaviour
{
    public static ItemManager02 Instance { get; private set; }

    [Header("Item Drop Prefab Mapping")]
    [SerializeField]
    private List<ItemPrefabEntry> itemPrefabs;  // �ν����� ID-������ ����

    private Dictionary<int, GameObject> _itemPrefabDict;
    private Dictionary<int, ItemData> itemDataDictionary = new Dictionary<int, ItemData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // ��ųʸ� ��ȯ 
        _itemPrefabDict = new Dictionary<int, GameObject>();
        foreach (var entry in itemPrefabs)
        {
            if (!_itemPrefabDict.ContainsKey(entry.itemID))
                _itemPrefabDict.Add(entry.itemID, entry.prefab);
        }

        InitializeItems();  // ������ ������ �ʱ�ȭ
    }

    private void InitializeItems()
    {
        // GameObject/������ �ε� (Resources ����)
        GameObject invincibilityIcon = Resources.Load<GameObject>("GameObject/Invincibility"); //�̹������� ������Ʈ�� ����
        GameObject upgradeIcon = Resources.Load<GameObject>("GameObject/Upgrade");             // �ּ��������� GameObject�� �ƴ� ItemPrefab/������.prefab���� ���氡��.
        GameObject lifeUpIcon = Resources.Load<GameObject>("GameObject/LifeUp");
        GameObject speedUpIcon = Resources.Load<GameObject>("GameObject/SpeedUp");
        GameObject destroyAllIcon = Resources.Load<GameObject>("GameObject/DestroyAllEnemies");
        GameObject baseUpIcon = Resources.Load<GameObject>("GameObject/BaseUp");

        // ����
        itemDataDictionary.Add(0, new ItemData(
            0, "Invincibility",
            player => EnableInvincibility(player, 5f),
            5f
        ));
        Debug.Log($"[Load Sprite] Invincibility: {(invincibilityIcon != null ? "OK" : "FAILED")}");

        // ���׷��̵�
        itemDataDictionary.Add(1, new ItemData(
            1, "Upgrade",
            player => player.GetComponent<Player>().Upgrade(10, 10, 1000),
            0f
        ));
        Debug.Log($"[Load Sprite] Upgrade: {(upgradeIcon != null ? "OK" : "FAILED")}");

        // ��� +1
        itemDataDictionary.Add(2, new ItemData(
            2, "LifeUp",
            player => player.GetComponent<Player>().GetLife(1),
            0f
        ));
        Debug.Log($"[Load Sprite] LifeUp: {(lifeUpIcon != null ? "OK" : "FAILED")}");

        // ���ǵ��
        itemDataDictionary.Add(3, new ItemData(
            3, "SpeedUp",
            player => player.GetComponent<Player>().SpeedControl(3, 3),
            0f
        ));
        Debug.Log($"[Load Sprite] SpeedUp: {(speedUpIcon != null ? "OK" : "FAILED")}");

        // ��� �� �ı�
        itemDataDictionary.Add(5, new ItemData(
            5, "DestroyAllEnemies",
            _ => DestroyAllEnemies(),
            0f
        ));
        Debug.Log($"[Load Sprite] DestroyAllEnemies: {(destroyAllIcon != null ? "OK" : "FAILED")}");

        // ���� ��ȭ
        itemDataDictionary.Add(6, new ItemData(
            6, "BaseUp",
            _ => StartFortification(8f),
            8f
        ));
        Debug.Log($"[Load Sprite] BaseUp: {(baseUpIcon != null ? "OK" : "FAILED")}");
    }

    // ������ ��� �� ȣ��
    public void ApplyEffect(Item item, GameObject player)
    {
        if (itemDataDictionary.TryGetValue(item.itemID, out var data))
        {
            Debug.Log($"Applying {data.name} to player");
            data.effect.Invoke(player);
        }
        else
        {
            Debug.LogWarning($"Unknown item ID: {item.itemID}");
        }
    }

    // ID�� �ش��ϴ� ������ ������ ����
    public void DropItem(int itemID, Vector3 position)
    {
        if (_itemPrefabDict.TryGetValue(itemID, out var prefab) && prefab != null)
        {
            Instantiate(prefab, position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"No prefab assigned for itemID {itemID}");
        }
    }

    // �� ��ӿ�. �ʿ��� ID�� �Ѱ� ȣ��
    public void DropItemFromEnemy(int itemID)
    {
        Vector3 spawnPos = GetRandomPositionInMap();
        DropItem(itemID, spawnPos);
    }

    private Vector3 GetRandomPositionInMap()
    {
        float x = UnityEngine.Random.Range(-8f, 8f);
        float z = UnityEngine.Random.Range(-8f, 8f);
        return new Vector3(x, 0f, z);
    }

    private void DestroyAllEnemies()
    {
        int mask = LayerMask.GetMask("Enemy");
        foreach (var col in Physics.OverlapSphere(Vector3.zero, 100f, mask))
        {
            Destroy(col.gameObject);
        }
    }

    private void StartFortification(float duration)
    {
        StartCoroutine(FortificationCoroutine(duration));
    }

    private IEnumerator FortificationCoroutine(float duration)
    {
        int mask = LayerMask.GetMask("defaultBase");
        var list = new List<(GameObject obj, int orig)>();
        foreach (var col in Physics.OverlapSphere(Vector3.zero, 100f, mask))
        {
            list.Add((col.gameObject, col.gameObject.layer));
            col.gameObject.layer = LayerMask.NameToLayer("fortificationBase");
        }
        yield return new WaitForSeconds(duration);
        foreach (var (obj, orig) in list)
            if (obj) obj.layer = orig;
    }

    private void EnableInvincibility(GameObject player, float duration)
    {
        StartCoroutine(InvincibilityCoroutine(player, duration));
    }

    private IEnumerator InvincibilityCoroutine(GameObject player, float duration)
    {
        var ps = player.GetComponent<Player>();
        ps.state = PlayerState.Invincible;
        yield return new WaitForSeconds(duration);
        ps.state = PlayerState.General;
    }
}
