using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 드롭용 프리팹 매핑 구조체
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
    private List<ItemPrefabEntry> itemPrefabs;  // 인스펙터 ID-프리팹 연결

    private Dictionary<int, GameObject> _itemPrefabDict;
    private Dictionary<int, ItemData> itemDataDictionary = new Dictionary<int, ItemData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // 딕셔너리 변환 
        _itemPrefabDict = new Dictionary<int, GameObject>();
        foreach (var entry in itemPrefabs)
        {
            if (!_itemPrefabDict.ContainsKey(entry.itemID))
                _itemPrefabDict.Add(entry.itemID, entry.prefab);
        }

        InitializeItems();  // 아이템 데이터 초기화
    }

    private void InitializeItems()
    {
        // GameObject/아이콘 로드 (Resources 폴더)
        GameObject invincibilityIcon = Resources.Load<GameObject>("GameObject/Invincibility"); //이미지에서 오브젝트로 변경
        GameObject upgradeIcon = Resources.Load<GameObject>("GameObject/Upgrade");             // 주소참조값을 GameObject가 아닌 ItemPrefab/아이템.prefab으로 변경가능.
        GameObject lifeUpIcon = Resources.Load<GameObject>("GameObject/LifeUp");
        GameObject speedUpIcon = Resources.Load<GameObject>("GameObject/SpeedUp");
        GameObject destroyAllIcon = Resources.Load<GameObject>("GameObject/DestroyAllEnemies");
        GameObject baseUpIcon = Resources.Load<GameObject>("GameObject/BaseUp");

        // 무적
        itemDataDictionary.Add(0, new ItemData(
            0, "Invincibility",
            player => EnableInvincibility(player, 5f),
            5f
        ));
        Debug.Log($"[Load Sprite] Invincibility: {(invincibilityIcon != null ? "OK" : "FAILED")}");

        // 업그레이드
        itemDataDictionary.Add(1, new ItemData(
            1, "Upgrade",
            player => player.GetComponent<Player>().Upgrade(10, 10, 1000),
            0f
        ));
        Debug.Log($"[Load Sprite] Upgrade: {(upgradeIcon != null ? "OK" : "FAILED")}");

        // 목숨 +1
        itemDataDictionary.Add(2, new ItemData(
            2, "LifeUp",
            player => player.GetComponent<Player>().GetLife(1),
            0f
        ));
        Debug.Log($"[Load Sprite] LifeUp: {(lifeUpIcon != null ? "OK" : "FAILED")}");

        // 스피드업
        itemDataDictionary.Add(3, new ItemData(
            3, "SpeedUp",
            player => player.GetComponent<Player>().SpeedControl(3, 3),
            0f
        ));
        Debug.Log($"[Load Sprite] SpeedUp: {(speedUpIcon != null ? "OK" : "FAILED")}");

        // 모든 적 파괴
        itemDataDictionary.Add(5, new ItemData(
            5, "DestroyAllEnemies",
            _ => DestroyAllEnemies(),
            0f
        ));
        Debug.Log($"[Load Sprite] DestroyAllEnemies: {(destroyAllIcon != null ? "OK" : "FAILED")}");

        // 기지 강화
        itemDataDictionary.Add(6, new ItemData(
            6, "BaseUp",
            _ => StartFortification(8f),
            8f
        ));
        Debug.Log($"[Load Sprite] BaseUp: {(baseUpIcon != null ? "OK" : "FAILED")}");
    }

    // 아이템 사용 시 호출
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

    // ID에 해당하는 아이템 프리팹 스폰
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

    // 적 드롭용. 필요한 ID를 넘겨 호출
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
