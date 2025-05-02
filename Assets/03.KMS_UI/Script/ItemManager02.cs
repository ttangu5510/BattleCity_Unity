using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemManager02 : MonoBehaviour
{
    public static ItemManager02 Instance { get; private set; }

    [SerializeField] private GameObject itemPrefab;
    private Dictionary<int, ItemData> itemDataDictionary = new Dictionary<int, ItemData>();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InitializeItems();
    }

    private void InitializeItems()
    {
        GameObject invincibilityIcon = Resources.Load<GameObject>("Sprites/Invincibility"); //�ϴ� �⺻���� ���ҽ������� ��������Ʈ�� �����ϰ� ����
        GameObject upgradeIcon = Resources.Load<GameObject>("Sprites/Upgrade"); //�ּҴ� �뷫������ Assets/Resources/Sprites.���ϸ�.png
        GameObject lifeUpIcon = Resources.Load<GameObject>("Sprites/LifeUp"); //�ʿ�� �ּҸ� �����Ͽ� ��밡��
        GameObject speedUpIcon = Resources.Load<GameObject>("Sprites/SpeedUp");
        GameObject stopEnemiesIcon = Resources.Load<GameObject>("Sprites/StopEnemies");
        GameObject destroyAllIcon = Resources.Load<GameObject>("Sprites/DestroyAllEnemies");
        GameObject baseUpIcon = Resources.Load<GameObject>("Sprites/BaseUp");

        //����
        itemDataDictionary.Add(0, new ItemData(0, "Invincibility", (player) =>
        {
            EnableInvincibility(player, 5f);
        }, 5f));
        Debug.Log($"[Load Sprite] invincibilityIcon: {(invincibilityIcon != null ? "OK" : "FAILED")}");

        //���׷��̵�
        itemDataDictionary.Add(1, new ItemData(1, "Upgrade", (player) =>
        {
            player.GetComponent<Player>().Upgrade(10, 10, 1000); // �ӽ� +10 +10 1000��
        }, 0f));
        Debug.Log($"[Load Sprite] Upgrade: {(upgradeIcon != null ? "OK" : "FAILED")}");

        // ��� + 1 
        itemDataDictionary.Add(2, new ItemData(2, "LifeUp", (player) =>
        {
            player.GetComponent<Player>().GetLife(1); //player�� GetLife �߰�����
        }, 0f));
        Debug.Log($"[Load Sprite] LifeUpIcon: {(lifeUpIcon != null ? "OK" : "FAILED")}");

        // ���ǵ��
        itemDataDictionary.Add(3, new ItemData(3, "SpeedUp", (player) =>
        {
            player.GetComponent<Player>().SpeedControl(3, 3); // �ӽ� +3 /+3
        }, 0f));
        Debug.Log($"[Load Sprite] SpeedUpIcon: {(speedUpIcon != null ? "OK" : "FAILED")}");

        // ��� �� ����
        // itemDataDictionary.Add(4, new ItemData(4, "StopEnemies", (player) =>
        // {
        //     StopEnemies(5f); // 5�� ���� �� ����
        // }, 0f));
        // Debug.Log($"[Load Sprite] StopEnemies: {(stopEnemiesIcon != null ? "OK" : "FAILED")}");

        // ��� �� �ı�
        itemDataDictionary.Add(5, new ItemData(5, "DestroyAllEnemies", (player) =>
        {
            DestroyAllEnemies(); // ��� ���� �ı�
        }, 0f));
        Debug.Log($"[Load Sprite] DestroyAll: {(destroyAllIcon != null ? "OK" : "FAILED")}");

        //���� ��ȭ //��ǥ ������Ŀ� ���� ���� �ʿ�
        itemDataDictionary.Add(6, new ItemData(6, "BaseUp", (player) =>
        {
            StartFortification(8f);
        }, 8f));

        Debug.Log($"[Load Sprite] BaseUp: {(baseUpIcon != null ? "OK" : "FAILED")}");

    }


    public void ApplyEffect(Item item, GameObject player)
    {
        if (itemDataDictionary.TryGetValue(item.itemID, out var itemData))
        {
            Debug.Log($"Applying {itemData.name} to player");
            itemData.effect.Invoke(player);
        }
        else
        {
            Debug.LogWarning("Unknown item ID");
        }
    }

   // private void StopEnemies(float duration)
   // {
   //     // Enemy ���̾ ��� ���Ǿ�� Ž��
   //     LayerMask enemyLayer = LayerMask.GetMask("Enemy");
   //     Collider[] enemies = Physics.OverlapSphere(Vector3.zero, 100f, enemyLayer);
   //
   //     foreach (Collider col in enemies)
   //     {
   //         if (col.TryGetComponent<Enemy>(out var enemyController))
   //         {
   //             // (1) �ش� Enemy�� ���� ���
   //             enemyController.StopMovement(duration);
   //             // (2) ����� �α�
   //             Debug.Log($"Stopped enemy: {enemyController.name} for {duration} seconds");
   //         }
   //     }
   // }

    private void DestroyAllEnemies()
    {
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");

        // Enemy ���̾��� ��� �� ã��
        Collider[] enemies = Physics.OverlapSphere(Vector3.zero, 100f, enemyLayer); //��ũ�� 16*16, ���� 100f
        foreach (Collider enemy in enemies)
        {
            Destroy(enemy.gameObject); // �� �ı�
            Debug.Log($"Destroyed enemy: {enemy.name}");
        }
    }

    private void StartFortification(float duration)
    {
        StartCoroutine(FortificationCoroutine(duration));
    }

    private IEnumerator FortificationCoroutine(float duration)
    {
        LayerMask defaultBaseMask = LayerMask.GetMask("defaultBase");
        Collider[] bases = Physics.OverlapSphere(Vector3.zero, 100f, defaultBaseMask);

        List<(GameObject obj, int originalLayer)> baseList = new List<(GameObject obj, int originalLayer)>();
        int forificationLayer = LayerMask.NameToLayer("fortificationBase");

        foreach (Collider col in bases)
        {
            GameObject baseObj = col.gameObject;
            baseList.Add((baseObj, baseObj.layer));
            baseObj.layer = forificationLayer;
        }
        Debug.Log($"[BaseUp] Changed layer of {baseList.Count} bases to fortificationBase");

        yield return new WaitForSeconds(duration);

        foreach (var (obj, originalLayer) in baseList)
        {
            if (obj != null)
                obj.layer = originalLayer;
        }
        Debug.Log("[BaseUp] Reverted base layers to original");
    }
    #region DropItem
    public void DropItemFromEnemy()
    {
        Vector3 spawnPos = GetRandomPositionInMap();
        Instantiate(itemPrefab, spawnPos, Quaternion.identity);
    }

    private Vector3 GetRandomPositionInMap() // ������ġ�� ������ ����
    {
        float x = Random.Range(-8f, 8f);
        float y = Random.Range(-8f, 8f);

        return new Vector3(x, 0f, y);
    }
    #endregion
    private void EnableInvincibility(GameObject player, float duration)
    {
        StartCoroutine(InvincibilityCoroutine(player, duration));
    }

    private IEnumerator InvincibilityCoroutine(GameObject player, float duration)
    {
        Player playerScript = player.GetComponent<Player>();

        playerScript.state = PlayerState.Invincible;

        yield return new WaitForSeconds(duration);

        playerScript.state = PlayerState.General;
    }
}
